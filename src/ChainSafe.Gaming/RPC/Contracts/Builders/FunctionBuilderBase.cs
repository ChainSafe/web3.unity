using System;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json.Linq;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>Represents a Function Builder Base.</summary>
    public abstract class FunctionBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBuilderBase"/> class with function ABI.
        /// </summary>
        protected FunctionBuilderBase(string contractAddress, FunctionABI functionAbi)
            : this(contractAddress)
        {
            FunctionABI = functionAbi;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBuilderBase"/> class.
        /// </summary>
        protected FunctionBuilderBase(string contractAddress)
        {
            ContractAddress = contractAddress;
            FunctionCallDecoder = new FunctionCallDecoder();
            FunctionCallEncoder = new FunctionCallEncoder();
        }

        /// <summary>
        /// SetUp and get ContractAddress.
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// SetUp and get FunctionCallDecoder.
        /// </summary>
        protected FunctionCallDecoder FunctionCallDecoder { get; set; }

        /// <summary>
        /// SetUp and get FunctionCallEncoder.
        /// </summary>
        protected FunctionCallEncoder FunctionCallEncoder { get; set; }

        /// <summary>
        /// SetUp and get FunctionABI.
        /// </summary>
        public FunctionABI FunctionABI { get; protected set; }

        private static Parameter GetFirstParameterOrNull(Parameter[] parameters)
        {
            if (parameters == null)
            {
                return null;
            }

            if (parameters.Length == 0)
            {
                return null;
            }

            return parameters[0];
        }

        /// <summary>
        /// Checks if Transaction Input Data is for Function.
        /// </summary>
        /// <returns>True if Transaction Input Data is meant for Function.</returns>
        public bool IsTransactionInputDataForFunction(string data)
        {
            return FunctionCallDecoder.IsDataForFunction(FunctionABI, data);
        }

        /// <summary>
        /// Decode provided string data.
        /// </summary>
        /// <returns>List of function output parameters.</returns>
        public List<ParameterOutput> DecodeInput(string data)
        {
            return FunctionCallDecoder.DecodeFunctionInput(
                FunctionABI.Sha3Signature, data, FunctionABI.InputParameters);
        }

        /// <summary>
        /// Converts stringified JSON to Object Input parameters.
        /// </summary>
        /// <returns>Array of object input parameters ready to be used in a function call.</returns>
        public object[] ConvertJsonToObjectInputParameters(string json)
        {
            var jObject = JObject.Parse(json);
            return jObject.ConvertToFunctionInputParameterValues(FunctionABI);
        }

        /// <summary>
        /// Converts JSON to Object Input Parameters array.
        /// </summary>
        /// <returns>Array of object input parameters ready to be used in a function call.</returns>
        public object[] ConvertJsonToObjectInputParameters(JObject jObject)
        {
            return jObject.ConvertToFunctionInputParameterValues(FunctionABI);
        }

        /// <summary>
        /// Decode output stringify data to JObject.
        /// </summary>
        /// <returns>Resulted JObject.</returns>
        public JObject DecodeOutputToJObject(string data)
        {
            return DecodeOutput(data).ConvertToJObject();
        }

        /// <summary>
        /// Decode stingified output data to List of parameters.
        /// </summary>
        /// <returns>List of parameters.</returns>
        public List<ParameterOutput> DecodeOutput(string data)
        {
            return FunctionCallDecoder.DecodeDefaultData(
                data, FunctionABI.OutputParameters);
        }

        /// <summary>
        /// Decode type output based on provided generic type.
        /// </summary>
        /// <returns>Output casted to provided generic type.</returns>
        /// <typeparam name="TReturn">Type of output.</typeparam>
        public TReturn DecodeTypeOutput<TReturn>(string output)
        {
            var function = FunctionOutputAttribute.GetAttribute<TReturn>();
            if (function != null)
            {
                var instance = Activator.CreateInstance(typeof(TReturn));
                return DecodeDTOTypeOutput<TReturn>((TReturn)instance, output);
            }
            else
            {
                return FunctionCallDecoder.DecodeSimpleTypeOutput<TReturn>(
                GetFirstParameterOrNull(FunctionABI.OutputParameters), output);
            }
        }

        /// <summary>
        /// Decode DTO type output.
        /// </summary>
        /// <returns>Output casted to provided generic type.</returns>
        /// <typeparam name="TReturn">Type of output.</typeparam>
        public TReturn DecodeDTOTypeOutput<TReturn>(TReturn functionOuput, string output)
        {
            return FunctionCallDecoder.DecodeFunctionOutput(functionOuput, output);
        }

        /// <summary>
        /// Decode DTO type output.
        /// </summary>
        /// <returns>Output casted to provided generic type.</returns>
        /// <typeparam name="TReturn">Type of output.</typeparam>
        public TReturn DecodeDTOTypeOutput<TReturn>(string output)
            where TReturn : new()
        {
            return FunctionCallDecoder.DecodeFunctionOutput<TReturn>(output);
        }

        /// <summary>
        /// Creates Transaction Input.
        /// </summary>
        /// <returns>Constructed transaction input object.</returns>
        public TransactionInput CreateTransactionInput(
            string from,
            HexBigInteger gas,
            HexBigInteger value)
        {
            var encodedInput = FunctionCallEncoder.EncodeRequest(FunctionABI.Sha3Signature);
            return new TransactionInput(encodedInput, from, gas, value);
        }

        /// <summary>
        /// Creates Transaction Input.
        /// </summary>
        /// <returns>Constructed transaction input object.</returns>
        public TransactionInput CreateTransactionInput(
            HexBigInteger type,
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            HexBigInteger maxFeePerGas,
            HexBigInteger maxPriorityFeePerGas)
        {
            var encodedInput = FunctionCallEncoder.EncodeRequest(FunctionABI.Sha3Signature);
            return new TransactionInput(type, encodedInput, ContractAddress, from, gas, value, maxFeePerGas, maxPriorityFeePerGas);
        }

        protected CallInput CreateCallInput(string encodedFunctionCall)
        {
            return new CallInput(encodedFunctionCall, ContractAddress);
        }

        protected CallInput CreateCallInput(
            string encodedFunctionCall,
            string from,
            HexBigInteger gas,
            HexBigInteger value)
        {
            return new CallInput(encodedFunctionCall, ContractAddress, from, gas, value);
        }

        protected TransactionInput CreateTransactionInput(string encodedFunctionCall, string from)
        {
            var tx = new TransactionInput(encodedFunctionCall, ContractAddress) { From = from };
            return tx;
        }

        protected TransactionInput CreateTransactionInput(
            string encodedFunctionCall,
            string from,
            HexBigInteger gas,
            HexBigInteger value)
        {
            return new TransactionInput(encodedFunctionCall, ContractAddress, from, gas, value);
        }

        protected TransactionInput CreateTransactionInput(
            string encodedFunctionCall,
            string from,
            HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value)
        {
            return new TransactionInput(encodedFunctionCall, ContractAddress, from, gas, gasPrice, value);
        }

        protected TransactionInput CreateTransactionInput(HexBigInteger type, string encodedFunctionCall, string from, HexBigInteger gas, HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas)
        {
            return new TransactionInput(type, encodedFunctionCall, ContractAddress, from, gas, value, maxFeePerGas, maxPriorityFeePerGas);
        }

        protected TransactionInput CreateTransactionInput(string encodedFunctionCall, TransactionInput input)
        {
            input.Data = encodedFunctionCall;
            input.To = ContractAddress;
            return input;
        }
    }
}