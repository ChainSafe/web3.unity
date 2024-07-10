using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface IContractBuilder
    {
        Dictionary<string, Contract> BasicContracts { get; }

        Contract Build(string name);

        Contract Build(string abi, string address);
    }
}
