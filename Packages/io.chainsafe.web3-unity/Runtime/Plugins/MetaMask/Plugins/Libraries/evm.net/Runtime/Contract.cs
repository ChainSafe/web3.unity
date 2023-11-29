﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using evm.net.Factory;
using Nethereum.ABI;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using evm.net.Models;
using Newtonsoft.Json;

namespace evm.net
{
    public class Contract : DynamicObject
    {
        public class LegacyWrapperProvider : IProvider
        {
            private readonly ILegacyProvider _provider;

            public long ChainId => _provider.ChainId;

            public string ConnectedAddress => _provider.ConnectedAddress;

            public LegacyWrapperProvider(ILegacyProvider provider)
            {
                _provider = provider;
            }

            public Task<TR> Request<TR>(string method, object[] parameters = null)
            {
                throw new NotImplementedException();
            }

            public object Request(string method, object[] parameters = null)
            {
                return _provider.Request(method, parameters);
            }
        }

        private EvmAddress _address;
        private readonly IProvider _provider;
        private readonly Type _interfaceType;
        private readonly ABIEncode _encoder = new ABIEncode();

        private readonly Dictionary<MethodBase, FunctionABI> _abiCache = new Dictionary<MethodBase, FunctionABI>();

        public ParameterConverter ParameterConverter { get; set; } = new();

        public EvmAddress Address
        {
            get { return _address; }
        }

        public bool IsDeployed
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_address);
            }
        }

        public IProvider CurrentProvider
        {
            get { return _provider; }
        }

        public Type ContractInterfaceType
        {
            get { return _interfaceType; }
        }

        private static IContractFactory _contractFactory =
#if !ENABLE_MONO
            new BackedTypeContractFactory();
#else
            new ImpromptuContractFactory();
#endif

        public static IContractFactory ContractFactory
        {
            get { return _contractFactory; }
            set { _contractFactory = value; }
        }

        public static object Attach(IProvider provider, Type interfaceType, EvmAddress address = null)
        {
            if (!interfaceType.IsInterface)
                throw new ArgumentException("interfaceType must be an Interface");
            
            var genericContractFactoryMethod = ContractFactory.GetType().GetMethod("BuildNewInstance")?.MakeGenericMethod(interfaceType);

            if (genericContractFactoryMethod == null)
                throw new Exception("Could not get generic contract factory method BuildNewInstance for type " +
                                    interfaceType.FullName);
            
            return genericContractFactoryMethod.Invoke(ContractFactory, new object[] { provider, address });
        }

        public static T Attach<T>(IProvider provider, EvmAddress address = null) where T : class, IContract
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException("T must be an Interface");

            return ContractFactory.BuildNewInstance<T>(provider, address);
        }

        public static T Attach<T>(ILegacyProvider provider, EvmAddress address = null) where T : class, IContract
        {
            return Attach<T>(new LegacyWrapperProvider(provider), address);
        }

        public Contract(IProvider provider, EvmAddress address, Type interfaceType)
        {
            this._address = address;
            this._provider = provider;
            this._interfaceType = interfaceType;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
            return base.TryGetMember(binder, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                // Step 1: Inspect this call, Determine function selector, Encode parameters with ABI
                var methodInfo = CallToInterfaceMethod(binder, args);
                return RunInvokeMethod(methodInfo, args, out result);
            }
            catch (Exception e)
            {
                result = null;
                throw new TargetInvocationException(
                    "Error invoking proxy function for contract of type " + _interfaceType.FullName + " at address " +
                    _address, e);
            }
        }
        
        protected virtual MethodInfo CallToInterfaceMethod(InvokeMemberBinder binder, object[] args, Type lookup = null)
        {
            if (lookup == null)
                lookup = _interfaceType;
            
            if (args == null)
                args = Array.Empty<object>();
            // Get method info for current invocation
            var parameterTypes = args.Select(a => a.GetType()).ToArray();
            var methodInfo = lookup.GetMethod(binder.Name, parameterTypes);

            if (methodInfo == null)
            {
                if (lookup.IsInterface)
                {
                    var otherTypes = lookup.GetInterfaces();
                    foreach (var t in otherTypes)
                    {
                        try
                        {
                            return CallToInterfaceMethod(binder, args, t);
                        }
                        catch (TargetInvocationException)
                        {
                        }
                    }
                }
                
                throw new TargetInvocationException(new Exception("No method " + binder.Name +
                                                                  " with parameter types (" +
                                                                  string.Join(',',
                                                                      parameterTypes.Select(t => t.FullName)) +
                                                                  ") found in interface type " +
                                                                  lookup.FullName));
            }

            return methodInfo;
        }

        protected T FindCustomMethodAttribute<T>(MethodBase methodInfo, out MethodBase correctMethod) where T : System.Attribute
        {
            var methodInfoAttribute = methodInfo.GetCustomAttribute<T>();

            if (methodInfoAttribute != null)
            {
                correctMethod = methodInfo;
                return methodInfoAttribute;
            }

            if (methodInfo.DeclaringType != null)
            {
                var possibleMethod = methodInfo.DeclaringType.GetInterfaces().Select(i =>
                        i.GetMethod(methodInfo.Name, GetParametersNoOptions(methodInfo).Select(p => p.ParameterType).ToArray()))
                    .Where(m => m != null)
                    .FirstOrDefault(m => m.GetCustomAttribute<T>()  != null);

                if (possibleMethod != null)
                {
                    correctMethod = possibleMethod;
                    return correctMethod.GetCustomAttribute<T>();
                }
            }

            correctMethod = methodInfo;
            return default;
        }

        protected EvmMethodInfoAttribute FindEvmMethodInfoAttribute(MethodBase methodInfo, out MethodBase correctMethod)
        {
            return FindCustomMethodAttribute<EvmMethodInfoAttribute>(methodInfo, out correctMethod);
        }

        protected virtual string GetEvmMethodName(MethodBase methodInfo, out MethodBase correctMethod)
        {
            var methodInfoAttribute = FindEvmMethodInfoAttribute(methodInfo, out correctMethod);

            if (methodInfoAttribute != null && !string.IsNullOrWhiteSpace(methodInfoAttribute.Name))
            {
                return methodInfoAttribute.Name;
            }

            return methodInfo.Name;
        }

        protected virtual bool IsMethodConstant(MethodBase methodInfo, out MethodBase correctMethod)
        {
            var methodInfoAttribute = FindEvmMethodInfoAttribute(methodInfo, out correctMethod);

            return methodInfoAttribute is { View: true };
        }

        protected virtual bool IsMethodConstructor(MethodBase methodBase, out MethodBase correctMethod)
        {
            var methodConstructorAttribute =
                FindCustomMethodAttribute<EvmConstructorMethodAttribute>(methodBase, out correctMethod);

            return methodConstructorAttribute != null;
        }

        protected virtual ABIType ParameterToAbiType(ParameterInfo parameterInfo)
        {
            return ParameterConverter.ConvertParameterInfo(parameterInfo);
        }

        protected List<Parameter> BuildInputParameters(MethodBase correctMethod)
        {
            List<Parameter> inputParameters = new List<Parameter>();
            int order = 1;
            foreach (var parameter in GetParametersNoOptions(correctMethod))
            {
                var evmAbiType = ParameterToAbiType(parameter);
                var evmParamterAttribute = parameter.GetCustomAttributes<EvmParameterInfoAttribute>()
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Name));
                var name = parameter.Name;
                if (evmParamterAttribute != null && !string.IsNullOrWhiteSpace(evmParamterAttribute.Name))
                {
                    name = evmParamterAttribute.Name;
                }

                // TODO support structs and internalType
                var evmParameter = new Parameter(evmAbiType.CanonicalName, name, order);
                inputParameters.Add(evmParameter);
            }

            return inputParameters;
        }

        protected List<Parameter> BuildOutputParameters(MethodBase correctMethod)
        {
            List<Parameter> outputParameters = new List<Parameter>();
            var order = 1;

            var methodInfoAttribute = correctMethod.GetCustomAttributes<EvmMethodInfoAttribute>().FirstOrDefault();
            if (methodInfoAttribute != null && methodInfoAttribute.Returns != null && methodInfoAttribute.Returns.Length > 0)
            {
                order = 0;

                outputParameters = methodInfoAttribute.Returns
                    .Select(et => ABIType.CreateABIType(et?.ToLower()))
                    .Select(at => new Parameter(at.CanonicalName, order += 1)).ToList();
            }
            else if (correctMethod is MethodInfo info)
            {
                if (info.ReturnParameter != null &&
                    info.ReturnParameter.ParameterType != typeof(Transaction) &&
                    info.ReturnParameter.ParameterType != typeof(Task<Transaction>))
                {
                    var abiType = ParameterToAbiType(info.ReturnParameter);
                    outputParameters.Add(new Parameter(abiType.CanonicalName, order));
                }
            }

            return outputParameters;
        }

        protected FunctionABI MethodInfoToFunctionAbi(MethodBase methodBase)
        {
            // Build FunctionABI
            var evmMethodName = GetEvmMethodName(methodBase, out var correctMethod);
            
            if (_abiCache.ContainsKey(correctMethod))
                return _abiCache[correctMethod];

            var abiFunction = new FunctionABI(evmMethodName, IsMethodConstant(correctMethod, out correctMethod));

            // Build input parameters
            var inputParameters = BuildInputParameters(correctMethod);
            abiFunction.InputParameters = inputParameters.ToArray();

            // Build output parameters
            var outputParameters = BuildOutputParameters(correctMethod);
            abiFunction.OutputParameters = outputParameters.ToArray();

            _abiCache.Add(correctMethod, abiFunction);

            return abiFunction;
        }

        protected virtual string EncodeInput(Parameter[] inputParameters, object[] args, string functionSelector = null, bool prefixHex = true)
        {
            if (inputParameters.Length != args.Length)
                throw new ArgumentException("Invalid number of arguments, expected " +
                                            inputParameters.Length + ", got " + args.Length);

            var convertedArgs = args.Select(a => a is IConvertableType ctype ? ctype.Convert() : a).ToArray();

            // ABI Encode parameters
            var abiValues = inputParameters.Select(p => p.ABIType).Zip(convertedArgs, (type, o) => new Tuple<ABIType, object>(type, o))
                .Select(pargs => new ABIValue(pargs.Item1, pargs.Item2)).ToArray();

            var encodedValues = _encoder.GetABIEncoded(abiValues);

            // Create input data
            var input = encodedValues.ToHex();
            
            if (!string.IsNullOrWhiteSpace(functionSelector))
                input = $"{functionSelector}{input}";

            if (prefixHex)
                input = $"0x{input}";

            return input;
        }

        protected object InvokeMethod(MethodBase methodInfo, object[] args = null)
        {
            object result;
            var success = RunInvokeMethod(methodInfo, args, out result);
            if (success)
                return result;
            
            throw new TargetInvocationException("Could not invoke contract method for MethodBase " + methodInfo.Name, new Exception("RunInvokeMethod call failed"));
        }

        private bool IsTypeTask(Type type)
        {
            return (type.IsGenericType &&
                    (type.GetGenericTypeDefinition() == typeof(Task<>) ||
                     type.GetGenericTypeDefinition() ==
                     typeof(AsyncTaskMethodBuilder<>))) || type == typeof(Task);
        }

        private object DecodeOutputResult(object taskResult, Type expectedReturnType, bool isAsyncCall,
            FunctionABI functionAbi)
        {
            if (taskResult.GetType() == expectedReturnType)
            {
                if (taskResult is not string s || !s.StartsWith("0x"))
                {
                    return taskResult;
                }
            }

            string stringData = null;
            if (taskResult is JsonElement jsonTaskResult)
            {
                // Is this a JSON object?
                if (jsonTaskResult.ValueKind == JsonValueKind.Object)
                {
                    // Convert JsonElement to expectedReturnType using Json.NET
                    var jsonString = JsonObject.Create(jsonTaskResult)?.ToJsonString();
                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        return JsonConvert.DeserializeObject(jsonString, expectedReturnType);
                    }
                }

                stringData = jsonTaskResult.GetString();
            }

            if (string.IsNullOrWhiteSpace(stringData))
            {
                // Try to parse string?
                stringData = taskResult.ToString();
            }

            if (expectedReturnType == typeof(string) && !stringData.StartsWith("0x"))
            {
                // If their expecting a string, give them the ToString version of this object
                // but only if it's not a hex string
                return stringData;
            }

            // Check to see if we got a null value back
            if (stringData == "0x")
                return null;

            if (stringData.StartsWith("0x"))
            {
                // If this string starts with 0x, it could be ABI encoded data
                // Or a transaction hash

                if (expectedReturnType == typeof(Transaction))
                {
                    if (!isAsyncCall)
                    {
                        // If we're expecting a Transaction object, then perhaps we need to grab the transaction
                        // sync

                        var transactionTask = Transaction.FromHash(stringData, _provider);
                        transactionTask.Wait();

                        return transactionTask.Result;
                    }

                    return Transaction.FromHash(stringData, _provider);
                }

                // TODO Convert string hex to objects using ABI

                // Step 4: Convert output(s) to requested return type

                if (functionAbi != null)
                {

                    if (functionAbi.OutputParameters.Length == 0)
                    {
                        return null;
                    }

                    if (functionAbi.OutputParameters.Length == 1)
                    {
                        var parm = functionAbi.OutputParameters[0];
                        var parmType = parm.ABIType;

                        byte[] data;

                        // TODO Figure out why we need to trim first 32 bytes of string ABI data
                        if (parmType.CanonicalName == "string")
                            data = stringData.HexToByteArray().Skip(32).ToArray();
                        else
                            data = stringData.HexToByteArray();

                        var decodedResult = parmType.Decode(data, expectedReturnType);

                        if (decodedResult != null)
                        {
                            return decodedResult;
                        }

                        throw new SerializationException(
                            $"Could not decode response {stringData} to ABI type {parmType.Name}");
                    }

                    // TODO Decode multiple output values into a Tuple (if the return type is a Tuple)
                    throw new NotImplementedException(
                        "Cannot decode multiple output values yet..");
                }
            }
            
            throw new SerializationException(
                $"Could not decode provider response {stringData} of type {taskResult.GetType().FullName} for function unknown, expected return type {expectedReturnType.FullName}");
        }

        private async void WaitForTaskCompleted(Task resultTask, object taskSource, bool isVoidAsyncCall,
            Type expectedReturnType, FunctionABI functionAbi)
        {
            try
            {
                // await inside try block to catch any errors
                await resultTask;
                
                if (isVoidAsyncCall)
                {
                    ((TaskCompletionSource<bool>)taskSource).SetResult(true);
                }
                else
                {
                    var taskResult = resultTask.GetType().GetProperty("Result").GetValue(resultTask);
                    var setResultMethod = taskSource.GetType().GetMethod("SetResult");

                    var finalResult = DecodeOutputResult(taskResult, expectedReturnType, true, functionAbi);
                    var finalResultType = finalResult?.GetType() ?? expectedReturnType;

                    var isFinalResultTask = IsTypeTask(finalResultType);

                    if (isFinalResultTask)
                    {
                        Task finalResultTask = (Task)finalResult;
                        finalResultTask?.ContinueWith(f =>
                        {
                            var ffr = f.GetType().GetProperty("Result").GetValue(f);

                            setResultMethod?.Invoke(taskSource, new[] { ffr });
                        });
                    }
                    else
                    {
                        setResultMethod?.Invoke(taskSource, new[] { finalResult });
                    }
                }
            }
            catch (Exception e)
            {
                if (isVoidAsyncCall)
                {
                    ((TaskCompletionSource<bool>)taskSource).SetException(e);
                }
                else
                {
                    var setExceptionMethod = taskSource.GetType()
                        .GetMethod("SetException", new[] { typeof(Exception) });
                    if (setExceptionMethod != null) setExceptionMethod.Invoke(taskSource, new[] { e });
                    else throw;
                }
            }
        }

        private bool RunInvokeAsyncMethod(bool isVoidAsyncCall, bool isResultTask, Type expectedReturnType, object providerResult, FunctionABI functionAbi, out object result)
        {
            object taskSource = new TaskCompletionSource<bool>();
            if (!isVoidAsyncCall)
            {
                // TODO Create generic version of TaskCompletionSource
                var genericTaskCompletionSourceType =
                    typeof(TaskCompletionSource<>).MakeGenericType(expectedReturnType);

                taskSource = (object)Activator.CreateInstance(genericTaskCompletionSourceType);

                if (taskSource == null)
                    throw new Exception("Could not create new generic TaskCompletionSource<" +
                                        expectedReturnType.Name +
                                        ">");
            }

            if (isResultTask)
            {
                // All generic versions of task inherit from non-generic Task
                Task returnedTask = (Task)taskSource.GetType().GetProperty("Task").GetValue(taskSource);
                //Task returnedTask = ((dynamic)taskSource).Task;
                Task resultTask = (Task)providerResult;


                result = returnedTask;
                //If the provider's request function did return a task, use ContinueWith

                WaitForTaskCompleted(resultTask, taskSource, isVoidAsyncCall, expectedReturnType, functionAbi);
                
                return true;
            }
            else
            {
                // If the provider's request function didn't return a task, resolve right away with return value
                result = taskSource.GetType().GetProperty("Task").GetValue(taskSource);
                // result = ((dynamic)taskSource).Task;

                // If the proxy function is a void task (no return value), resolve with no value
                if (isVoidAsyncCall)
                {
                    ((TaskCompletionSource<bool>)taskSource).SetResult(true);
                    return true;
                }

                var setResultMethod = taskSource.GetType().GetMethod("SetResult");
                var finalResult = DecodeOutputResult(providerResult, expectedReturnType, true, functionAbi);

                var finalResultType = finalResult?.GetType() ?? expectedReturnType;

                var isFinalResultTask = IsTypeTask(finalResultType);

                if (isFinalResultTask)
                {
                    Task finalResultTask = (Task)finalResult;
                    finalResultTask?.ContinueWith(f =>
                    {
                        var ffr = f.GetType().GetProperty("Result").GetValue(f);

                        setResultMethod?.Invoke(taskSource, new[] { ffr });
                    });
                }
                else
                {
                    setResultMethod?.Invoke(taskSource, new[] { finalResult });
                }

                return true;
            }
        }

        private bool RunInvokeMethodSync(bool isResultTask, Type resultType,
            Type expectedReturnType, object providerResult, FunctionABI functionAbi, out object result)
        {
            // TODO Run the request task sync, and save the return value inside of result
            object encodedResult = null;

            if (isResultTask)
            {
                var waitMethod = resultType.GetMethod("Wait", Type.EmptyTypes);
                if (waitMethod != null)
                {
                    waitMethod.Invoke(providerResult, Array.Empty<object>());

                    var resultProperty = typeof(Task<>).MakeGenericType(expectedReturnType)
                        .GetProperty("Result");

                    if (resultProperty != null)
                    {
                        encodedResult = resultProperty.GetValue(providerResult, null);
                        if (encodedResult == null)
                            throw new Exception("Failed to get result property of type " + resultType.FullName);
                    }
                    else
                    {
                        throw new Exception("Provider returned result object of type " + resultType.FullName +
                                            " which is a Task, but no Result property could be found");
                    }
                }
                else
                {
                    throw new Exception("Provider returned result object of type " + resultType.FullName +
                                        " which is a Task, but no Wait method could be found");
                }
            }
            else
            {
                encodedResult = providerResult;
            }

            var finalResult = DecodeOutputResult(encodedResult, expectedReturnType, false, functionAbi);
            var finalResultType = finalResult?.GetType() ?? expectedReturnType;

            var isFinalResultTask = IsTypeTask(finalResultType);

            if (isFinalResultTask)
            {
                // Wait for task sync, we are inside a sync call
                Task finalResultTask = (Task)finalResult;
                finalResultTask?.Wait();
                finalResult = finalResultType?.GetProperty("Result")?.GetValue(finalResult);
            }

            result = finalResult;
            return true;
        }

        private (object, Type, bool) SendProviderRequest(object[] requestParameters, Type expectedReturnType)
        {
            // First make sure expectedReturnType: Transaction => string

            if (expectedReturnType == typeof(Transaction))
                expectedReturnType = typeof(string);
            
            object rawResult = null;
            var requestMethod = (from m in _provider.GetType().GetMethods()
                where m.Name == "Request" && m.IsGenericMethodDefinition
                let typeParams = m.GetGenericArguments()
                let normalParams = GetParametersNoOptions(m)

                where typeParams.Length == 1 && normalParams.Length == 2
                select m).FirstOrDefault();


            if (requestMethod != null)
            {
                var genericRequestMethod = requestMethod.MakeGenericMethod(expectedReturnType);
                try
                {
                    rawResult = genericRequestMethod.Invoke(_provider, requestParameters);
                }
                catch (TargetInvocationException e)
                {
                    if (e.InnerException == null || e.InnerException.GetType() != typeof(NotImplementedException))
                    {
                        throw;
                    }
                    // Ignore, this provider may not implement the generic version (ILegacyProvider)
                }
            }

            if (rawResult == null)
            {
                // If requestMethod was null, or the provider didn't support generic invocation
                // then try using ILegacyProvider
                requestMethod = (from m in _provider.GetType().GetMethods()
                    where m.Name == "Request" && !m.IsGenericMethodDefinition
                    let normalParams = GetParametersNoOptions(m)

                    where normalParams.Length == 2
                    select m).FirstOrDefault();

                if (requestMethod != null)
                {
                    rawResult = requestMethod.Invoke(_provider, requestParameters);
                }
            }

            // If there still is no rawResult, then fail
            if (rawResult == null)
                throw new Exception("Got no result object back from provider");

            var resultType = rawResult.GetType();
            var isResultTask = IsTypeTask(resultType);

            return (rawResult, resultType, isResultTask);
        }

        private (Type expectedReturnType, bool isAsyncCall, bool isVoidAsyncCall) InspectMethodReturnType(MethodBase methodInfo)
        {
            // Step 2a: We need to return the correct type, and also ensure we return a Task if the return type
            // is wrapped in a Task.
            var methodReturnType = methodInfo is MethodInfo info ? info.ReturnType : typeof(void);
            var isAsyncCall = IsTypeTask(methodReturnType);

            var expectedReturnType = methodReturnType;

            var isVoidAsyncCall = methodReturnType == typeof(Task);
            if (isAsyncCall)
            {
                if (!isVoidAsyncCall)
                {
                    expectedReturnType = methodReturnType.GetGenericArguments()[0];
                }
            }
            
            return (expectedReturnType, isAsyncCall, isVoidAsyncCall);
        }

        private object[] BuildRequestParameters(FunctionABI functionAbi, CallOptions? options, object[] methodCallArgs)
        {
            var input = EncodeInput(functionAbi.InputParameters, methodCallArgs, functionAbi.Sha3Signature);
            
            // Step 2: Create RPC request for eth_sendTransaction OR eth_call if View function
            var isView = functionAbi.Constant;
            var method = isView ? "eth_call" : "eth_sendTransaction";

            object[] requestData;
            if (isView)
            {
                if (!string.IsNullOrWhiteSpace(options?.BlockTag))
                {
                    requestData = new object[]
                    {
                        new EthCall()
                        {
                            From = _provider.ConnectedAddress,
                            To = _address.Value,
                            Data = input,
                            Gas = options?.Gas,
                            GasPrice = options?.GasPrice,
                            Value = options?.Value,
                        },
                        options?.BlockTag
                    };
                }
                else
                {
                    requestData = new object[]
                    {
                        new EthCall()
                        {
                            From = _provider.ConnectedAddress,
                            To = _address.Value,
                            Data = input,
                            Gas = options?.Gas,
                            GasPrice = options?.GasPrice,
                            Value = options?.Value
                        },
                        "latest"
                    };
                }
            }
            else
            {
                requestData = new object[]
                {
                    new EthSendTransaction()
                    {
                        From = _provider.ConnectedAddress,
                        To = _address.Value,
                        Data = input,
                        Gas = options?.Gas,
                        GasPrice = options?.GasPrice,
                        Nonce = options?.Nonce,
                        Value = options?.Value,
                    }
                };
            }
            return new object[]
            {
                method,
                requestData,
            };
        }

        private bool RunRequestMethodInvoke(MethodBase methodInfo, CallOptions? options, object[] args, out object result)
        {
            var functionAbi = MethodInfoToFunctionAbi(methodInfo);

            var requestParameters = BuildRequestParameters(functionAbi, options, args);

            var (expectedReturnType, isAsyncCall, isVoidAsyncCall) = InspectMethodReturnType(methodInfo);

            var (rawResult, resultType, isResultTask) = SendProviderRequest(requestParameters, expectedReturnType);

            if (isAsyncCall)
            {
                return RunInvokeAsyncMethod(isVoidAsyncCall, isResultTask, 
                    expectedReturnType, rawResult, 
                    functionAbi, out result);
            }

            return RunInvokeMethodSync(isResultTask, resultType, 
                expectedReturnType, rawResult, 
                functionAbi, out result);
        }

        protected string ByteCodeFromInterface(MethodBase methodBase)
        {
            var field = _interfaceType.GetField("Bytecode", BindingFlags.Static | BindingFlags.Public);

            if (field == null)
            {
                // Try to get from attribute
                var constructorAttribute =
                    FindCustomMethodAttribute<EvmConstructorMethodAttribute>(methodBase, out _);

                if (constructorAttribute != null)
                {
                    return constructorAttribute.Bytecode;
                }

                throw new Exception("No static field Bytecode found for Type " + _interfaceType.FullName);
            }

            return (string)field.GetValue(null);
        }

        private bool RunConstructorMethodInvoke(MethodBase methodInfo, CallOptions? options, object[] args, out object result)
        {
            var constructorArgs = BuildInputParameters(methodInfo);
            var constructorInput = EncodeInput(constructorArgs.ToArray(), args, null, false);

            var bytecode = ByteCodeFromInterface(methodInfo);

            if (!bytecode.StartsWith("0x"))
                bytecode = $"0x{bytecode}";

            var input = $"{bytecode}{constructorInput}";
            
            var method = "eth_sendTransaction";
            object[] requestData = {
                new EthSendTransaction()
                {
                    From = _provider.ConnectedAddress,
                    Data = input,
                    Gas = options?.Gas,
                    GasPrice = options?.GasPrice,
                    Nonce = options?.Nonce,
                    Value = options?.Value,
                }
            };

            object[] request = {
                method,
                requestData
            };
            
            var (expectedReturnType, isAsyncCall, isVoidAsyncCall) = InspectMethodReturnType(methodInfo);

            if (expectedReturnType != _interfaceType || isVoidAsyncCall)
                throw new Exception("Constructor methods must return the same interface type they deploy");

            // Always grab the Transaction from the provider, this way we can
            // grab the new contract address
            var (rawResult, resultType, isResultTask) = SendProviderRequest(request, typeof(Transaction));

            bool invokeResult = RunInvokeAsyncMethod(false, true, 
                typeof(Transaction), rawResult, 
                null, out var providerResult);

            if (invokeResult)
            {
                var providerResultTask = (Task<Transaction>)providerResult;

                if (isAsyncCall)
                {
                    var genericTaskCompletionSourceType =
                        typeof(TaskCompletionSource<>).MakeGenericType(expectedReturnType);

                    var taskSource = (object)Activator.CreateInstance(genericTaskCompletionSourceType);
                    var setResultMethod = taskSource.GetType().GetMethod("SetResult");

                    if (taskSource == null || setResultMethod == null)
                        throw new Exception("Could not create new generic TaskCompletionSource<" +
                                            expectedReturnType.Name +
                                            ">");
                    
                    // All generic versions of task inherit from non-generic Task
                    Task returnedTask = (Task)taskSource.GetType().GetProperty("Task").GetValue(taskSource);

                    result = returnedTask;

                    providerResultTask.ContinueWith(async p =>
                    {
                        var receipt = await p.Result.FetchReceipt();

                        var contractAddress = receipt.ContractAddress;

                        var contract = Attach(_provider, _interfaceType, contractAddress);

                        setResultMethod.Invoke(taskSource, new[] { contract });
                    });

                    return true;
                }
                else
                {
                    providerResultTask.Wait();
                    var transaction = providerResultTask.Result;

                    var receiptTask = transaction.FetchReceipt();

                    receiptTask.Wait();

                    var receipt = receiptTask.Result;

                    var contractAddress = receipt.ContractAddress;

                    result = Attach(_provider, _interfaceType, contractAddress);

                    return true;
                }
            }

            result = null;
            return false;
        }

        private ParameterInfo[] GetParametersNoOptions(MethodBase m)
        {
            return m.GetParameters().Where(p => p.ParameterType != typeof(CallOptions)).ToArray();
        }

        private bool RunInvokeMethod(MethodBase methodInfo, object[] args, out object result)
        {
            if (args == null)
                args = Array.Empty<object>();

            CallOptions? options = null;
            if (args.Length > 0 && args[args.Length - 1] is CallOptions)
            {
                options = (CallOptions)args[args.Length - 1];
                args = args.Take(args.Length - 1).ToArray();
            }

            if (IsMethodConstructor(methodInfo, out var correctMethod))
            {
                return RunConstructorMethodInvoke(correctMethod, options, args, out result);
            }
            else
            {
                return RunRequestMethodInvoke(methodInfo, options, args, out result);
            }
        }
    }
}