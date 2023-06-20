using System;
using System.Linq;
using Nethereum.ABI.ABIDeserialisation;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;

namespace Web3Unity.Scripts.Library.Ethers.Contracts.Builders
{
    public class ContractBuilder
    {
        public ContractBuilder(string abi, string contractAddress)
        {
            ContractABI = ABIDeserialiserFactory.DeserialiseContractABI(abi);
            Address = contractAddress;
        }

        public ContractBuilder(Type contractMessageType, string contractAddress)
        {
            var abiExtractor = new AttributesToABIExtractor();
            ContractABI = abiExtractor.ExtractContractABI(contractMessageType);
            Address = contractAddress;
        }

        public ContractBuilder(Type[] contractMessagesTypes, string contractAddress)
        {
            var abiExtractor = new AttributesToABIExtractor();
            ContractABI = abiExtractor.ExtractContractABI(contractMessagesTypes);
            Address = contractAddress;
        }

        public ContractABI ContractABI { get; set; }

        public string Address { get; set; }

        public NewFilterInput GetDefaultFilterInput(BlockParameter fromBlock = null, BlockParameter toBlock = null)
        {
            return FilterInputBuilder.GetDefaultFilterInput(Address, fromBlock, toBlock);
        }

        public FunctionBuilder<TFunction> GetFunctionBuilder<TFunction>()
        {
            var function = FunctionAttribute.GetAttribute<TFunction>() ??
                throw new Exception("Invalid TFunction required a Function Attribute");
            return new FunctionBuilder<TFunction>(Address, GetFunctionAbi(function.Name));
        }

        public FunctionBuilder GetFunctionBuilder(string name)
        {
            return new FunctionBuilder(Address, GetFunctionAbi(name));
        }

        public FunctionBuilder GetFunctionBuilderBySignature(string signature)
        {
            return new FunctionBuilder(Address, GetFunctionAbiBySignature(signature));
        }

        public ErrorABI GetErrorAbi(string name)
        {
            if (ContractABI == null)
            {
                throw new Exception("Contract abi not initialised");
            }

            var errorAbi = ContractABI.Errors.FirstOrDefault(x => x.Name == name) ??
                throw new Exception("Error not found");
            return errorAbi;
        }

        public EventABI GetEventAbi(string name)
        {
            if (ContractABI == null)
            {
                throw new Exception("Contract abi not initialised");
            }

            var eventAbi = ContractABI.Events.FirstOrDefault(x => x.Name == name) ??
                throw new Exception("Event not found");
            return eventAbi;
        }

        public EventABI GetEventAbiBySignature(string signature)
        {
            if (ContractABI == null)
            {
                throw new Exception("Contract abi not initialised");
            }

            var eventAbi =
                ContractABI.Events.FirstOrDefault(
                    x =>
                        x.Sha3Signature.ToLowerInvariant().EnsureHexPrefix() ==
                        signature.ToLowerInvariant().EnsureHexPrefix()) ??
                throw new Exception("Event not found for signature:" + signature);
            return eventAbi;
        }

        public FunctionABI GetFunctionAbi(string name)
        {
            if (ContractABI == null)
            {
                throw new Exception("Contract abi not initialised");
            }

            var functionAbi = ContractABI.Functions.FirstOrDefault(x => x.Name == name) ??
                throw new Exception("Function not found:" + name);
            return functionAbi;
        }

        public FunctionABI GetFunctionAbiBySignature(string signature)
        {
            if (ContractABI == null)
            {
                throw new Exception("Contract abi not initialised");
            }

            var functionAbi =
                ContractABI.Functions.FirstOrDefault(
                    x =>
                        x.Sha3Signature.ToLowerInvariant().EnsureHexPrefix() ==
                        signature.ToLowerInvariant().EnsureHexPrefix()) ??
                throw new Exception("Function not found for signature:" + signature);
            return functionAbi;
        }
    }
}