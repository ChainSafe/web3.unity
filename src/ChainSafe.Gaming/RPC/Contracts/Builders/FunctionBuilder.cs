using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionTypes;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// Main builder for function commands.
    /// </summary>
    public class FunctionBuilder : Nethereum.Contracts.FunctionBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBuilder"/> class.
        /// </summary>
        /// <param name="contractAddress">The contract address.</param>
        /// <param name="function">The function of the ABI.</param>
        public FunctionBuilder(string contractAddress, FunctionABI function)
            : base(contractAddress, function)
        {
        }

        /// <summary>
        /// Create call input with function inputs.
        /// </summary>
        /// <param name="functionInput">The call input parameters.</param>
        /// <returns>An instance of CallInput class.</returns>
        public CallInput CreateCallInput(params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return base.CreateCallInput(encodedInput);
        }

        /// <summary>
        /// Create call input with given parameters.
        /// </summary>
        /// <param name="from">Transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Amount of WEI to send with the transaction.</param>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <returns>An instance of CallInput class.</returns>
        public CallInput CreateCallInput(
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return base.CreateCallInput(encodedInput, from, gas, value);
        }

        /// <summary>
        /// Encodes the function call using the input parameters.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <returns>String representation of the encoded function call.</returns>
        public string GetData(params object[] functionInput)
        {
            return FunctionCallEncoder.EncodeRequest(
                FunctionABI.Sha3Signature, FunctionABI.InputParameters, functionInput);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="functionInput">Array of objects that represent function input parameters.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(string from, params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return base.CreateTransactionInput(encodedInput, from, null, null);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <param name="functionInput">Array of objects that represent function input parameters.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return base.CreateTransactionInput(encodedInput, from, gas, value);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="gasPrice">Estimated gas price in WEI.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <param name="functionInput">Array of objects that represent function input parameters.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            string from,
            HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value,
            params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return base.CreateTransactionInput(encodedInput, from, gas, gasPrice, value);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="type">Hex representation of the transaction type.</param>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <param name="maxFeePerGas">Maximum fee per gas (EIP-1559).</param>
        /// <param name="maxPriorityFeePerGas">Maximum priority fee per gas (EIP-1559).</param>
        /// <param name="functionInput">Array of objects that represent function input parameters.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            HexBigInteger type,
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            HexBigInteger maxFeePerGas,
            HexBigInteger maxPriorityFeePerGas,
            params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(type, encodedInput, from, gas, value, maxFeePerGas, maxPriorityFeePerGas);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <param name="maxFeePerGas">Maximum fee per gas (EIP-1559).</param>
        /// <param name="maxPriorityFeePerGas">Maximum priority fee per gas (EIP-1559).</param>
        /// <param name="functionInput">Array of objects that represent function input parameters.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            HexBigInteger maxFeePerGas,
            HexBigInteger maxPriorityFeePerGas,
            params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(TransactionType.EIP1559.AsHexBigInteger(), encodedInput, from, gas, value, maxFeePerGas, maxPriorityFeePerGas);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="input">Another transaction input to copy values from.</param>
        /// <param name="functionInput">Array of objects that represent function input parameters.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(TransactionInput input, params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return base.CreateTransactionInput(encodedInput, input);
        }
    }

    /// <summary>
    /// Generic version of <see cref="FunctionBuilder"/>.
    /// </summary>
    /// <typeparam name="TFunctionInput">Parametrisation of the function input.</typeparam>
    public class FunctionBuilder<TFunctionInput> : Nethereum.Contracts.FunctionBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBuilder{TFunctionInput}"/> class.
        /// </summary>
        /// <param name="contractAddress">The contract address.</param>
        public FunctionBuilder(string contractAddress)
            : base(contractAddress)
        {
            FunctionABI = ABITypedRegistry.GetFunctionABI<TFunctionInput>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBuilder{TFunctionInput}"/> class.
        /// </summary>
        /// <param name="contractAddress">The contract address.</param>
        /// <param name="function">The function of the ABI.</param>
        public FunctionBuilder(string contractAddress, FunctionABI function)
            : base(contractAddress, function)
        {
        }

        /// <summary>
        /// Create call input with parameterless function input.
        /// </summary>
        /// <returns>An instance of CallInput class.</returns>
        public CallInput CreateCallInputParameterless()
        {
            return CreateCallInput(FunctionCallEncoder.EncodeRequest(FunctionABI.Sha3Signature));
        }

        /// <summary>
        /// Create call input with parameterized function input.
        /// </summary>
        /// <param name="functionInput">The call input parameters.</param>
        /// <returns>An instance of CallInput class.</returns>
        public CallInput CreateCallInput(TFunctionInput functionInput)
        {
            var encodedInput = GetData(functionInput);
            return CreateCallInput(encodedInput);
        }

        /// <summary>
        /// Create call input with parameterized function input.
        /// </summary>
        /// <param name="functionInput">The call input parameters.</param>
        /// <param name="from">Transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Amount of WEI to send with the transaction.</param>
        /// <returns>An instance of CallInput class.</returns>
        public CallInput CreateCallInput(
            TFunctionInput functionInput,
            string from,
            HexBigInteger gas,
            HexBigInteger value)
        {
            var encodedInput = GetData(functionInput);
            return CreateCallInput(encodedInput, from, gas, value);
        }

        /// <summary>
        /// Encodes the function call using parameterized input parameters as a string.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <returns>String representation of the encoded function call.</returns>
        public string GetData(TFunctionInput functionInput)
        {
            return FunctionCallEncoder.EncodeRequest(functionInput, FunctionABI.Sha3Signature);
        }

        /// <summary>
        /// Encodes the function call using parameterized input parameters as a byte array.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <returns>Encoded function call as a byte array.</returns>
        public byte[] GetDataAsBytes(TFunctionInput functionInput)
        {
            return GetData(functionInput).HexToByteArray();
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="transactionInput">Transaction input to merge with.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TFunctionInput DecodeFunctionInput(TFunctionInput functionInput, TransactionInput transactionInput)
        {
            return FunctionCallDecoder.DecodeFunctionInput(functionInput, FunctionABI.Sha3Signature, transactionInput.Data);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="data">Data that needs to be parsed.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TFunctionInput DecodeFunctionInput(TFunctionInput functionInput, string data)
        {
            return FunctionCallDecoder.DecodeFunctionInput(functionInput, FunctionABI.Sha3Signature, data);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(encodedInput, from);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            TFunctionInput functionInput,
            string from,
            HexBigInteger gas,
            HexBigInteger value)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(encodedInput, from, gas, value);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="gasPrice">Estimated gas price in WEI.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            TFunctionInput functionInput,
            string from,
            HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(encodedInput, from, gas, gasPrice, value);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="type">Hex representation of the transaction type.</param>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <param name="maxFeePerGas">Maximum fee per gas (EIP-1559).</param>
        /// <param name="maxPriorityFeePerGas">Maximum priority fee per gas (EIP-1559).</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            TFunctionInput functionInput,
            HexBigInteger type,
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            HexBigInteger maxFeePerGas,
            HexBigInteger maxPriorityFeePerGas)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(type, encodedInput, from, gas, value, maxFeePerGas, maxPriorityFeePerGas);
        }

        /// <summary>
        /// Encodes function input parameters and creates a transaction input.
        /// </summary>
        /// <param name="functionInput">Input parameters for the function call.</param>
        /// <param name="from">Outgoing transaction sender address.</param>
        /// <param name="gas">Amount of gas allocated for this specific call.</param>
        /// <param name="value">Value in native currency to send.</param>
        /// <param name="maxFeePerGas">Maximum fee per gas (EIP-1559).</param>
        /// <param name="maxPriorityFeePerGas">Maximum priority fee per gas (EIP-1559).</param>
        /// <returns>Object that is ready to be passed to the signer and transaction executor.</returns>
        public TransactionInput CreateTransactionInput(
            TFunctionInput functionInput,
            string from,
            HexBigInteger gas,
            HexBigInteger value,
            HexBigInteger maxFeePerGas,
            HexBigInteger maxPriorityFeePerGas)
        {
            var encodedInput = GetData(functionInput);
            return CreateTransactionInput(TransactionType.EIP1559.AsHexBigInteger(), encodedInput, from, gas, value, maxFeePerGas, maxPriorityFeePerGas);
        }
    }
}