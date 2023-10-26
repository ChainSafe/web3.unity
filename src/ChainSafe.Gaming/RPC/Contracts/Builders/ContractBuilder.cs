using System;
using System.Linq;
using Nethereum.ABI.ABIDeserialisation;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// Represents a builder pattern class used to build a contract.
    /// </summary>
    public class ContractBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBuilder"/> class using ABI string and contract address.
        /// </summary>
        /// <param name="abi">ABI string of the contract.</param>
        /// <param name="contractAddress">Ethereum address of the contract.</param>
        public ContractBuilder(string abi, string contractAddress)
        {
            ContractABI = ABIDeserialiserFactory.DeserialiseContractABI(abi);
            Address = contractAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBuilder"/> class using a contract message type and contract address.
        /// </summary>
        /// <param name="contractMessageType">Type containing attributes representing the contract ABI.</param>
        /// <param name="contractAddress">Ethereum address of the contract.</param>
        public ContractBuilder(Type contractMessageType, string contractAddress)
        {
            var abiExtractor = new AttributesToABIExtractor();
            ContractABI = abiExtractor.ExtractContractABI(contractMessageType);
            Address = contractAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBuilder"/> class using multiple contract message types and contract address.
        /// </summary>
        /// <param name="contractMessagesTypes">Array of Types containing attributes representing the contract ABI.</param>
        /// <param name="contractAddress">Ethereum address of the contract.</param>
        public ContractBuilder(Type[] contractMessagesTypes, string contractAddress)
        {
            var abiExtractor = new AttributesToABIExtractor();
            ContractABI = abiExtractor.ExtractContractABI(contractMessagesTypes);
            Address = contractAddress;
        }

        /// <summary>
        /// Gets or sets the ABI of the contract.
        /// </summary>
        public ContractABI ContractABI { get; set; }

        /// <summary>
        /// Gets or sets the Ethereum address of the contract.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Retrieves the default filter input for the contract.
        /// </summary>
        /// <param name="fromBlock">Starting block for the filter. Optional.</param>
        /// <param name="toBlock">Ending block for the filter. Optional.</param>
        /// <returns>The default filter input for the contract.</returns>
        public NewFilterInput GetDefaultFilterInput(BlockParameter fromBlock = null, BlockParameter toBlock = null)
        {
            return FilterInputBuilder.GetDefaultFilterInput(Address, fromBlock, toBlock);
        }

        /// <summary>
        /// Retrieves the function builder for a specific function using its type.
        /// </summary>
        /// <typeparam name="TFunction">Type of the function.</typeparam>
        /// <returns>The function builder for the specified function type.</returns>
        /// <exception cref="Exception">Thrown when the specified type lacks a required Function Attribute.</exception>
        public FunctionBuilder<TFunction> GetFunctionBuilder<TFunction>()
        {
            var function = FunctionAttribute.GetAttribute<TFunction>() ??
                throw new Exception("Invalid TFunction required a Function Attribute");
            return new FunctionBuilder<TFunction>(Address, GetFunctionAbi(function.Name));
        }

        /// <summary>
        /// Retrieves the function builder for a specific function using its name.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <returns>The function builder for the specified function name.</returns>
        public FunctionBuilder GetFunctionBuilder(string name)
        {
            return new FunctionBuilder(Address, GetFunctionAbi(name));
        }

        /// <summary>
        /// Retrieves the function builder for a specific function using its signature.
        /// </summary>
        /// <param name="signature">Signature of the function.</param>
        /// <returns>The function builder for the specified function signature.</returns>
        public FunctionBuilder GetFunctionBuilderBySignature(string signature)
        {
            return new FunctionBuilder(Address, GetFunctionAbiBySignature(signature));
        }

        /// <summary>
        /// Retrieves the error ABI for a specific error using its name.
        /// </summary>
        /// <param name="name">Name of the error.</param>
        /// <returns>The error ABI for the specified name.</returns>
        /// <exception cref="Exception">Thrown when the Contract ABI is not initialized or the error is not found.</exception>
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

        /// <summary>
        /// Retrieves the event ABI for a specific event using its name.
        /// </summary>
        /// <param name="name">Name of the event.</param>
        /// <returns>The event ABI for the specified name.</returns>
        /// <exception cref="Exception">Thrown when the Contract ABI is not initialized or the event is not found.</exception>
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

        /// <summary>
        /// Retrieves the event ABI for a specific event using its signature.
        /// </summary>
        /// <param name="signature">Signature of the event.</param>
        /// <returns>The event ABI for the specified signature.</returns>
        /// <exception cref="Exception">Thrown when the Contract ABI is not initialized or the event signature is not found.</exception>
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

        /// <summary>
        /// Retrieves the function ABI for a specific function using its name.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <returns>The function ABI for the specified name.</returns>
        /// <exception cref="Exception">Thrown when the Contract ABI is not initialized or the function is not found.</exception>
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

        /// <summary>
        /// Retrieves the function ABI for a specific function using its signature.
        /// </summary>
        /// <param name="signature">Signature of the function.</param>
        /// <returns>The function ABI for the specified signature.</returns>
        /// <exception cref="Exception">Thrown when the Contract ABI is not initialized or the function signature is not found.</exception>
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