using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.Gaming.Evm.Contracts
{
    /// <summary>
    /// Defines the <see cref="ContractBuilderConfig"/> class.
    /// </summary>
    public class ContractBuilderConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBuilderConfig"/> class.
        /// Prevents a default instance of the <see cref="ContractBuilderConfig"/> class from being created.
        /// </summary>
        internal ContractBuilderConfig()
        {
        }

        /// <summary>
        /// Gets an internal list of registered contracts.
        /// </summary>
        internal List<ContractData> RegisteredContracts { get; } = new List<ContractData>();

        /// <summary>
        /// Registers a new contract.
        /// </summary>
        /// <param name="name">Name of the contract.</param>
        /// <param name="abi">The Application Binary Interface (ABI) for the contract.</param>
        /// <param name="address">The address of the contract.</param>
        /// <returns>Returns an instance of the ContractBuilderConfig class.</returns>
        public ContractBuilderConfig RegisterContract(string name, string abi, string address)
        {
            RegisteredContracts.Add(new ContractData(name, abi, address));
            return this;
        }
    }

    /// <summary>
    /// Defines the <see cref="ContractData"/> class.
    /// </summary>
    internal class ContractData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractData"/> class.
        /// </summary>
        /// <param name="name">Name of the contract.</param>
        /// <param name="abi">The Application Binary Interface (ABI) for the contract.</param>
        /// <param name="address">The address of the contract.</param>
        public ContractData(string name, string abi, string address)
        {
            Name = name;
            Abi = abi;
            Address = address;
        }

        /// <summary>
        /// Gets the name of the contract.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the Application Binary Interface (ABI) for the contract.
        /// </summary>
        public string Abi { get; }

        /// <summary>
        /// Gets the address of the contract.
        /// </summary>
        public string Address { get; }
    }
}
