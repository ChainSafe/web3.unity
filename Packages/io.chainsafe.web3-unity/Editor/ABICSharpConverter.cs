using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Nethereum.ABI;
using Nethereum.ABI.ABIDeserialisation;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using UnityEditor;
using UnityEngine;

public class ABICSharpConverter : EditorWindow
{
    private string _abi;
    private bool _abiIsValid;
    private ContractABI _contractABI;
    private string _contractName;
    private DefaultAsset _targetFolder;
    private Dictionary<string, List<Parameter>> _customClassDict;

    public Dictionary<string, List<Parameter>> CustomClassDict => _customClassDict;

    private static ABICSharpConverter _instance;

    public static ABICSharpConverter Instance
    {
        get
        {
            if (_instance == null)
                _instance = GetWindow<ABICSharpConverter>("ABI to C# Contract Converter");
            return _instance;
        }
        set => _instance = value;
    }

    // UI rendering method
        private void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { richText = true };

        GUILayout.Space(10);
        GUILayout.Label("<b><size=15>ChainSafe ABI to C# Contract Converter</size></b>", style);
        GUILayout.Space(10);

        _contractName = EditorGUILayout.TextField("Contract Name", _contractName);
        _targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Select Folder", _targetFolder, typeof(DefaultAsset), false);

        if (_targetFolder == null)
        {
            EditorGUILayout.HelpBox("No folder selected, please select a folder first", MessageType.Error);
            return;
        }

        EditorGUI.BeginChangeCheck();
        _abi = EditorGUILayout.TextField("ABI", _abi, EditorStyles.textArea, GUILayout.Height(200));
        if (EditorGUI.EndChangeCheck())
            _abiIsValid = IsValidAbi(_abi);

        if (!_abiIsValid)
        {
            EditorGUILayout.HelpBox("Invalid ABI", MessageType.Error);
            return;
        }

        if (string.IsNullOrEmpty(_contractName.Trim()))
        {
            EditorGUILayout.HelpBox("Contract Name cannot be empty", MessageType.Error);
            return;
        }

        if (Regex.IsMatch(_contractName, @"[^a-zA-Z0-9_]|(?<=^)[0-9]"))
        {
            EditorGUILayout.HelpBox("Contract name is invalid", MessageType.Error);
            return;
        }

        if (GUILayout.Button("Convert"))
        {
            ConvertAbiToCSharp();
        }
    }

    [MenuItem("ChainSafe SDK/ABI to C# Contract Converter")]
    public static void ShowWindow()
    {
        Instance = GetWindow<ABICSharpConverter>("ABI to C# Contract Converter");
    }

    // Method to handle ABI conversion
    private void ConvertAbiToCSharp()
    {
        _contractABI = ABIDeserialiserFactory.DeserialiseContractABI(_abi);
        var text = Resources.Load<TextAsset>("ABIContractClassTemplate").text;
        var className = Regex.Replace(_contractName, @"[^a-zA-Z0-9_]|(?<=^)[0-9]", string.Empty);

        text = text.Replace("{CLASS_NAME}", className);
        var minifiedJson = JsonDocument.Parse(_abi).RootElement.GetRawText();
        var escapedJson = minifiedJson.Replace("\"", "\\\"");
        text = text.Replace("{CONTRACT_ABI}", escapedJson);
        text = Regex.Replace(text, @"\s*\{CUSTOM_CLASSES\}", "\n\n" + ParseCustomClasses());
        text = Regex.Replace(text, @"\s*\{EVENT_CLASSES\}", "\n\n" + ParseEventClasses());
        text = Regex.Replace(text, @"\s*\{METHODS\}", "\n\n" + ParseMethods());
        text = Regex.Replace(text, @"\s*\{EVENT_SUBSCRIPTION\}", "\n\n" + ParseEventSubscription());
        text = Regex.Replace(text, @"\s*\{EVENT_UNSUBSCRIPTION\}", "\n\n" + ParseEventUnsubscription());
        var path = AssetDatabase.GetAssetPath(_targetFolder);
        var fullPath = $"{path}/{className}.cs";
        File.WriteAllText(fullPath, text);
        AssetDatabase.Refresh();
    }

    // Method to check if the ABI is valid
    private bool IsValidAbi(string abi)
    {
        abi = abi.Trim();
        var contractABI = ABIDeserialiserFactory.DeserialiseContractABI(abi);
        return !string.IsNullOrEmpty(abi) && (contractABI.Functions.Length > 0 || contractABI.Events.Length > 0 || contractABI.Errors.Length > 0);
    }

    // Method to parse and extract custom classes
    private string ParseCustomClasses()
    {
        var sb = new StringBuilder();
        _customClassDict = ExtractCustomClassesWithParams();

        var customClassTemplate = Resources.Load<TextAsset>("CustomClassTemplate").text;
        var paramTemplate = Resources.Load<TextAsset>("ParamTemplate").text;

        foreach (var key in _customClassDict.Keys)
        {
            var classStringBuilder = new StringBuilder(customClassTemplate);
            classStringBuilder.Replace("{CLASSNAME}", key);
            classStringBuilder.Replace("{PARAMETERS}", ParseParameters(_customClassDict[key], paramTemplate).ToString());
            sb.Append(classStringBuilder);
        }

        return sb.ToString();
    }

    private Dictionary<string, List<Parameter>> ExtractCustomClassesWithParams()
    {
        var dict = new Dictionary<string, List<Parameter>>();

        foreach (var functionABI in _contractABI.Functions)
        {
            foreach (var inputParameter in functionABI.InputParameters) ProcessParameter(inputParameter, dict);
            foreach (var outputParameter in functionABI.OutputParameters) ProcessParameter(outputParameter, dict);
        }

        foreach (var abiEvent in _contractABI.Events)
            foreach (var inputParameter in abiEvent.InputParameters) ProcessParameter(inputParameter, dict);
        return dict;
    }

    private void ProcessParameter(Parameter parameter, Dictionary<string, List<Parameter>> dict)
    {
        switch (parameter.ABIType)
        {
            case TupleType tupleType:
                ProcessTupleComponents(parameter, tupleType.Components, dict);
                break;
            case ArrayType { ElementType: TupleType elementTupleType }:
                ProcessTupleComponents(parameter, elementTupleType.Components, dict, true);
                break;
        }
    }

    private void ProcessTupleComponents(Parameter parameter, IReadOnlyList<Parameter> components, Dictionary<string, List<Parameter>> dict, bool isArray = false)
    {
        var className = parameter.Type.Split(".")[^1].RemoveFirstUnderscore().Capitalize();
        if (isArray) className = className.Replace("[]", "");

        if (dict.ContainsKey(className))
        {
            Debug.LogError("Trying to process the same class " + className + " multiple times. Skipping...");
            return;
        }

        dict[className] = new List<Parameter>();

        foreach (var component in components)
        {
            ProcessParameter(component, dict);
            dict[className].Add(component);
        }
    }

    private StringBuilder ParseParameters(IList<Parameter> parameters, string paramTemplate)
    {
        var stringBuilder = new StringBuilder();
        for (var index = 0; index < parameters.Count; index++)
        {
            var param = parameters[index];
            var eventParamStringBuilder = new StringBuilder(paramTemplate);
            eventParamStringBuilder.Replace("{TRUE_TYPE}", param.Type);
            eventParamStringBuilder.Replace("{TRUE_NAME}", param.Name);
            eventParamStringBuilder.Replace("{CSHARP_TYPE}", param.Type.ToCSharpType());
            eventParamStringBuilder.Replace("{CSHARP_NAME}", param.Name.RemoveFirstUnderscore().Capitalize());
            eventParamStringBuilder.Replace("{PARAM_ORDER}", index.ToString());
            eventParamStringBuilder.Replace("{PARAM_INDEXED}", param.Indexed.ToString().ToLowerInvariant());
            stringBuilder.Append(eventParamStringBuilder);
            stringBuilder.Append("\n");
        }

        return stringBuilder;
    }

    private string ParseMethods()
    {
        var functionTemplateBase = Resources.Load<TextAsset>("FunctionTemplate").text;
        var result = new StringBuilder();

        foreach (var functionABI in _contractABI.Functions)
        {
            var functionNoTransactionReceipt = GenerateMethodWithFunctionABI(functionABI, functionTemplateBase, false);
            var functionWithTransactionReceipt = "";
            if (!functionABI.Constant)
                functionWithTransactionReceipt = GenerateMethodWithFunctionABI(functionABI, functionTemplateBase, true);
            result.Append(functionNoTransactionReceipt);
            result.Append("\n");
            result.Append(functionWithTransactionReceipt);
            result.Append("\n\n");
        }

        return result.ToString();
    }

    private static string GenerateMethodWithFunctionABI(FunctionABI functionABI, string functionTemplateBase, bool useTransactionReceipt)
    {
        var functionStringBuilder = new StringBuilder(functionTemplateBase);

        ReplaceMethodName(functionStringBuilder, functionABI, useTransactionReceipt);
        ReplaceInputParameters(functionStringBuilder, functionABI);
        ReplaceContractMethodCall(functionStringBuilder, functionABI);
        ReplaceReturnType(functionStringBuilder, functionABI, useTransactionReceipt);
        ReplaceFunctionCall(functionStringBuilder, functionABI, useTransactionReceipt);
        ReplaceInputParamNames(functionStringBuilder, functionABI);
        ReplaceReturnStatement(functionStringBuilder, functionABI, useTransactionReceipt);

        return functionStringBuilder.ToString();
    }

    private static void ReplaceMethodName(StringBuilder functionStringBuilder, FunctionABI functionABI, bool useTransactionReceipt)
    {
        functionStringBuilder.Replace("{METHOD_NAME}", functionABI.Name.RemoveFirstUnderscore().Capitalize() + (useTransactionReceipt ? "WithReceipt" : ""));
    }

    private static void ReplaceInputParameters(StringBuilder functionStringBuilder, FunctionABI functionABI)
    {
        functionStringBuilder.Replace("{INPUT_PARAMS}", string.Join(", ", functionABI.InputParameters.Select(x => $"{x.Type.ToCSharpType()} {(string.IsNullOrEmpty(x.Name.ReplaceReservedNames()) ? $"{x.Type}" : $"{x.Name.ReplaceReservedNames()}")}")));
    }

    private static void ReplaceContractMethodCall(StringBuilder functionStringBuilder, FunctionABI functionABI)
    {
        functionStringBuilder.Replace("{CONTRACT_METHOD_CALL}", functionABI.Name);
    }

    private static void ReplaceReturnType(StringBuilder functionStringBuilder, FunctionABI functionABI, bool useTransactionReceipt)
    {
        if (useTransactionReceipt)
        {
            if (functionABI.OutputParameters.Length >= 1)
                functionStringBuilder.Replace("{RETURN_TYPE}", "(" + string.Join(", ", functionABI.OutputParameters.Select(x => $"{x.Type.ToCSharpType()} {x.Name.ReplaceReservedNames()}")) + (functionABI.OutputParameters.Length != 0 ? ", " : "") + "TransactionReceipt receipt)");
            else
                functionStringBuilder.Replace("{RETURN_TYPE}", "TransactionReceipt");
        }
        else
        {
            if (functionABI.OutputParameters.Length > 1)
                functionStringBuilder.Replace("{RETURN_TYPE}", "(" + string.Join(", ", functionABI.OutputParameters.Select(x => $"{x.Type.ToCSharpType()} {x.Name.ReplaceReservedNames()}")) + ")");
            else if (functionABI.OutputParameters.Length == 1)
                functionStringBuilder.Replace("{RETURN_TYPE}", functionABI.OutputParameters[0].Type.ToCSharpType());
            else
                functionStringBuilder.Replace("<{RETURN_TYPE}>", "");
        }
    }

    private static void ReplaceFunctionCall(StringBuilder functionStringBuilder, FunctionABI functionABI, bool useTransactionReceipt)
    {
        var sb = new StringBuilder();

        if (functionABI.Constant)
            sb.Append("Call");
        else
            sb.Append(useTransactionReceipt ? "SendWithReceipt" : "Send");
        if (functionABI.OutputParameters.Length == 1)
            sb.Append($"<{functionABI.OutputParameters[0].Type.ToCSharpType()}>");
        functionStringBuilder.Replace("{FUNCTION_CALL}", sb.ToString());
    }

    private static void ReplaceInputParamNames(StringBuilder functionStringBuilder, FunctionABI functionABI)
    {
        functionStringBuilder.Replace("{INPUT_PARAM_NAMES}", string.Join(", ", functionABI.InputParameters.Select(x => x.Name.ReplaceReservedNames())));
    }

    private static void ReplaceReturnStatement(StringBuilder functionStringBuilder, FunctionABI functionABI, bool useTransactionReceipt)
    {
        var sb = new StringBuilder();

        if (functionABI.OutputParameters.Length > 1)
        {
            sb.Append("(");
            for (var i = 0; i < functionABI.OutputParameters.Length; i++)
            {
                sb.Append($"({functionABI.OutputParameters[i].Type.ToCSharpType()})response" + $"{(useTransactionReceipt ? ".response" : "")}[{i}]");
                if (i != functionABI.OutputParameters.Length - 1) sb.Append(", ");
            }
            sb.Append(useTransactionReceipt ? ", response.receipt)" : ")");
        }
        else if (functionABI.OutputParameters.Length == 1)
        {
            if (useTransactionReceipt) sb.Append("(");
            sb.Append($"response{(useTransactionReceipt ? ".response" : "")}");
            sb.Append(useTransactionReceipt ? ", response.receipt)" : "");
        }

        if (functionABI.OutputParameters.Length != 0)
            functionStringBuilder.Replace("{RETURN_STATEMENT}", sb.ToString());
        else
            functionStringBuilder.Replace("return {RETURN_STATEMENT};", useTransactionReceipt ? "return response.receipt;" : "");
    }

    private string ParseEventSubscription()
    {
        var sb = new StringBuilder();

        foreach (var eventABI in _contractABI.Events)
        {
            var varName = eventABI.Name.RemoveFirstUnderscore().Capitalize();
            sb.Append($"\t\t\tvar filter{varName}Event = Event<{varName}EventDTO>.GetEventABI().CreateFilterInput(ContractAddress);\n");
            var eventSubscription = new StringBuilder(Resources.Load<TextAsset>("SubscriptionTemplate").text);
            eventSubscription.Replace("{ETH_LOG_CLIENT_NAME}", $"event{varName}");
            eventSubscription.Replace("{FILTER}", $"filter{varName}Event");
            eventSubscription.Replace("{CLASS_DTO_NAME}", $"{varName}EventDTO");
            eventSubscription.Replace("{EVENT_NAME}", $"On{varName}");
            sb.Append(eventSubscription);
            sb.Append("\n");
        }

        return sb.ToString();
    }

    private string ParseEventUnsubscription()
    {
        var sb = new StringBuilder();

        foreach (var eventABI in _contractABI.Events)
        {
            var varName = eventABI.Name.RemoveFirstUnderscore().Capitalize();
            sb.Append($"\t\t\tawait event{varName}.UnsubscribeAsync();\n");
            sb.Append($"\t\t\tOn{varName} = null;\n");
        }

        return sb.ToString();
    }

    private string ParseEventClasses()
    {
        var sb = new StringBuilder();
        var eventTemplateBase = Resources.Load<TextAsset>("EventTemplate").text;
        var paramTemplate = Resources.Load<TextAsset>("ParamTemplate").text;

        foreach (var eventABI in _contractABI.Events)
        {
            var eventStringBuilder = new StringBuilder(eventTemplateBase);
            eventStringBuilder.Replace("{EVENT_NAME}", eventABI.Name);
            eventStringBuilder.Replace("{EVENT_NAME_CSHARP}", eventABI.Name.RemoveFirstUnderscore().Capitalize());
            var stringBuilder = ParseParameters(eventABI.InputParameters, paramTemplate);

            eventStringBuilder.Replace("{EVENT_PARAMS}", stringBuilder.ToString());
            sb.Append(eventStringBuilder);
            sb.Append("\n\n");
            sb.Replace("{EVENT_LOG_SUBSCRIPTION}", $"EthLogsObservableSubscription event{eventABI.Name.RemoveFirstUnderscore().Capitalize()};");
            sb.Replace("{EVENT_ACTION_SUBSCRIPTION}", $"public event Action<{eventABI.Name.RemoveFirstUnderscore().Capitalize()}EventDTO> On{eventABI.Name.RemoveFirstUnderscore().Capitalize()};");
        }

        return sb.ToString();
    }
}

public static class ABIToCSHarpTypesExtension
{
    public static string ToCSharpType(this string parameter)
    {
        return parameter switch
        {
            "uint256" => "BigInteger",
            "uint256[]" => "BigInteger[]",
            "uint8" => "byte",
            "uint8[]" => "byte[]",
            "uint16" => "ushort",
            "uint16[]" => "ushort[]",
            "uint32" => "uint",
            "uint32[]" => "uint[]",
            "uint64" => "ulong",
            "uint64[]" => "ulong[]",
            "uint128" => "BigInteger",
            "uint128[]" => "BigInteger[]",
            "uint" => "BigInteger", // Alias for uint256
            "uint[]" => "BigInteger[]",
            "int256" => "BigInteger",
            "int256[]" => "BigInteger[]",
            "int8" => "sbyte",
            "int8[]" => "sbyte[]",
            "int16" => "short",
            "int16[]" => "short[]",
            "int32" => "int",
            "int32[]" => "int[]",
            "int64" => "long",
            "int64[]" => "long[]",
            "int128" => "BigInteger",
            "int128[]" => "BigInteger[]",
            "int" => "BigInteger", // Alias for int256
            "int[]" => "BigInteger[]",
            "address" => "string",
            "address[]" => "string[]",
            "bool" => "bool",
            "bool[]" => "bool[]",
            "string" => "string",
            "string[]" => "string[]",
            "bytes" => "byte[]",
            "bytes[]" => "byte[][]",
            "bytes1" => "byte[]",
            "bytes2" => "byte[]",
            "bytes3" => "byte[]",
            "bytes4" => "byte[]",
            "bytes5" => "byte[]",
            "bytes6" => "byte[]",
            "bytes7" => "byte[]",
            "bytes8" => "byte[]",
            "bytes9" => "byte[]",
            "bytes10" => "byte[]",
            "bytes11" => "byte[]",
            "bytes12" => "byte[]",
            "bytes13" => "byte[]",
            "bytes14" => "byte[]",
            "bytes15" => "byte[]",
            "bytes16" => "byte[]",
            "bytes17" => "byte[]",
            "bytes18" => "byte[]",
            "bytes19" => "byte[]",
            "bytes20" => "byte[]",
            "bytes21" => "byte[]",
            "bytes22" => "byte[]",
            "bytes23" => "byte[]",
            "bytes24" => "byte[]",
            "bytes25" => "byte[]",
            "bytes26" => "byte[]",
            "bytes27" => "byte[]",
            "bytes28" => "byte[]",
            "bytes29" => "byte[]",
            "bytes30" => "byte[]",
            "bytes31" => "byte[]",
            "bytes32" => "byte[]",
            _ => GetParameterTypeFromDictionary(parameter)
        };
    }

    private static string GetParameterTypeFromDictionary(string parameter)
    {
        var dict = ABICSharpConverter.Instance.CustomClassDict;
        var isArray = parameter.Contains("[]");
        var paramName = parameter.Split(".")[^1].RemoveFirstUnderscore().Capitalize().Replace("[]", "");
        if (!dict.ContainsKey(paramName))
        {
            Debug.LogError("Can't find " + paramName + " in the dictionary");
            return "object";
        }

        return paramName + (isArray ? "[]" : "");
    }

    public static string Capitalize(this string str)
    {
        return string.IsNullOrEmpty(str) ? str : char.ToUpper(str[0]) + str[1..];
    }

    public static string RemoveFirstUnderscore(this string str)
    {
        return str[0] == '_' ? str[1..] : str;
    }

    public static string ReplaceReservedNames(this string str)
    {
        return str switch
        {
            "operator" => "@operator",
            "params" => "@params",
            "event" => "@event",
            "delegate" => "@delegate",
            "object" => "@object",
            _ => str
        };
    }
}