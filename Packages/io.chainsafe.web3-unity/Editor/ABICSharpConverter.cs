
using Nethereum.ABI.ABIDeserialisation;
using Nethereum.ABI.Model;
using UnityEditor;
using UnityEngine;


public class ABICSharpConverter : EditorWindow
{
    private DefaultAsset _targetFolder;
    private string _abi;
    private string _contractName;
    private bool _abiIsValid;
    private ContractABI _contractABI;
    
    [MenuItem("ChainSafe SDK/ABI to C# Contract Converter")]
    // Show our window
    public static void ShowWindow()
    {
        GetWindow<ABICSharpConverter>("ABI to C# Contract Converter");
    }



    private void OnGUI()
    {
        // Ensure our labels are using Rich text for added customization
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;

        // Window Header
        GUILayout.Space(10);
        GUILayout.Label("<b><size=15>ChainSafe ABI to C# Contract Converter</size></b>", style);
        GUILayout.Space(10);

        _contractName = EditorGUILayout.TextField("Contract Name", _contractName);

        _targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Select Folder", 
            _targetFolder, 
            typeof(DefaultAsset), 
            false);

        if (_targetFolder == null)
        {
            EditorGUILayout.HelpBox("No folder selected, please select a folder first", MessageType.Error);
            return;
        }
        
        EditorGUI.BeginChangeCheck();
        _abi = EditorGUILayout.TextField("ABI", _abi);
        if (EditorGUI.EndChangeCheck())
            _abiIsValid = IsValidAbi(_abi);

        if (!_abiIsValid)
        {
            EditorGUILayout.HelpBox("Invalid ABI", MessageType.Error);
            return;
        }  
        
        if (GUILayout.Button("Convert"))
        {
            var textAsset = Resources.Load<TextAsset>("ABIContractClassTemplate");
        }
    }
    
    private bool IsValidAbi(string abi)
    {
        abi = abi.Trim();
        _contractABI = ABIDeserialiserFactory.DeserialiseContractABI(abi);

        return !string.IsNullOrEmpty(abi) && (_contractABI.Functions.Length > 0 || _contractABI.Events.Length > 0 ||
                                              _contractABI.Errors.Length > 0);
    }
    
    
}