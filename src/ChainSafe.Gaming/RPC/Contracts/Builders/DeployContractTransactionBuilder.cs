using System;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.ABI.ABIDeserialisation;
using Nethereum.ABI.FunctionEncoding;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// A class for building deployment transactions for Ethereum contracts.
    /// </summary>
    public class DeployContractTransactionBuilder
    {
        private readonly ConstructorCallEncoder constructorCallEncoder = new();

        /// <summary>
        /// Ensures the given contract bytecode does not contain any placeholders.
        /// </summary>
        /// <param name="byteCode">The contract bytecode to check.</param>
        public static void EnsureByteCodeDoesNotContainPlaceholders(string byteCode)
        {
            ByteCodeLibraryLinker.EnsureDoesNotContainPlaceholders(byteCode);
        }

        /// <summary>
        /// Gets the encoded data for deploying a contract.
        /// </summary>
        /// <param name="contractByteCode">The contract's bytecode.</param>
        /// <param name="abi">The contract's ABI.</param>
        /// <param name="values">The constructor's parameter values.</param>
        /// <returns>The encoded data.</returns>
        public string GetData(string contractByteCode, string abi, params object[] values)
        {
            var contract = ABIDeserialiserFactory.DeserialiseContractABI(abi);
            return constructorCallEncoder.EncodeRequest(contractByteCode, contract.Constructor.InputParameters, values);
        }

        /// <summary>
        /// Gets the encoded data for deploying a contract.
        /// </summary>
        /// <typeparam name="TConstructorParams">The type of the constructor parameters.</typeparam>
        /// <param name="contractByteCode">The contract's bytecode.</param>
        /// <param name="inputParams">The constructor's parameters.</param>
        /// <returns>The encoded data.</returns>
        public string GetData<TConstructorParams>(string contractByteCode, TConstructorParams inputParams)
        {
            return constructorCallEncoder.EncodeRequest(inputParams, contractByteCode);
        }

        /// <summary>
        /// Builds the encoded data.
        /// </summary>
        /// <param name="contractByteCode">The contract's bytecode.</param>
        /// <param name="abi">The contract's ABI.</param>
        /// <param name="values">The constructor's parameter values.</param>
        /// <returns>The encoded data.</returns>
        private string BuildEncodedData(string contractByteCode, string abi, object[] values)
        {
            EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);

            if (values == null || values.Length == 0)
            {
                return constructorCallEncoder.EncodeRequest(contractByteCode, string.Empty);
            }

            var contract = ABIDeserialiserFactory.DeserialiseContractABI(abi);
            if (contract.Constructor == null)
            {
                throw new Exception(
                    "Parameters supplied for a constructor but ABI does not contain a constructor definition");
            }

            var encodedData = constructorCallEncoder.EncodeRequest(
                contractByteCode, contract.Constructor.InputParameters, values);
            return encodedData;
        }

        /// <summary>
        /// Builds a transaction request for deploying a contract.
        /// </summary>
        /// <param name="contractByteCode">The contract's bytecode.</param>
        /// <param name="abi">The contract's ABI.</param>
        /// <param name="values">The constructor's parameter values.</param>
        /// <returns>The transaction request.</returns>
        public TransactionRequest BuildTransaction(
            string contractByteCode,
            string abi,
            object[] values)
        {
            var encodedData = BuildEncodedData(contractByteCode, abi, values);
            return new TransactionRequest()
            {
                Data = encodedData,
            };
        }

        /// <summary>
        /// Builds a transaction request for deploying a contract.
        /// </summary>
        /// <typeparam name="TConstructorParams">The type of the constructor parameters.</typeparam>
        /// <param name="contractByteCode">The contract's bytecode.</param>
        /// <param name="from">The sender's Ethereum address.</param>
        /// <param name="inputParams">The constructor's parameters.</param>
        /// <returns>The transaction request.</returns>
        public TransactionRequest BuildTransaction<TConstructorParams>(
            string contractByteCode,
            string from,
            TConstructorParams inputParams)
        {
            var encodedData = constructorCallEncoder.EncodeRequest(inputParams, contractByteCode);
            return new TransactionRequest()
            {
                Data = encodedData,
            };
        }
    }
}
