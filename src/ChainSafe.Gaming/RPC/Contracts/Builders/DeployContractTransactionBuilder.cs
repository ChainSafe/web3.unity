using System;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.ABI.ABIDeserialisation;
using Nethereum.ABI.FunctionEncoding;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    public class DeployContractTransactionBuilder
    {
        private readonly ConstructorCallEncoder constructorCallEncoder = new();

        public static void EnsureByteCodeDoesNotContainPlaceholders(string byteCode)
        {
            ByteCodeLibraryLinker.EnsureDoesNotContainPlaceholders(byteCode);
        }

        public string GetData(string contractByteCode, string abi, params object[] values)
        {
            var contract = ABIDeserialiserFactory.DeserialiseContractABI(abi);
            return constructorCallEncoder.EncodeRequest(contractByteCode, contract.Constructor.InputParameters, values);
        }

        public string GetData<TConstructorParams>(string contractByteCode, TConstructorParams inputParams)
        {
            return constructorCallEncoder.EncodeRequest(inputParams, contractByteCode);
        }

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