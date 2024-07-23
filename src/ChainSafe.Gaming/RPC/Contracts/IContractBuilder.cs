using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface IContractBuilder
    {
        Contract Build(string name);

        Contract Build(string abi, string address);

        Task<T> Build<T>(string address)
            where T : ICustomContract, new();
    }
}
