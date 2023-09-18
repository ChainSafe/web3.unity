using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public class ContractBuilderConfig
    {
        internal ContractBuilderConfig()
        {
        }

        internal List<ContractData> RegisteredContracts { get; } = new List<ContractData>();

        public ContractBuilderConfig RegisterContract(string name, string abi, string address)
        {
            RegisteredContracts.Add(new ContractData(name, abi, address));
            return this;
        }
    }

    internal class ContractData
    {
        public ContractData(string name, string abi, string address)
        {
            Name = name;
            Abi = abi;
            Address = address;
        }

        public string Name { get; }

        public string Abi { get; }

        public string Address { get; }
    }
}
