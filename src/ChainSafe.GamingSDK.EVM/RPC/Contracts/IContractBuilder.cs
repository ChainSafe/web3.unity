using System;
using System.Collections.Generic;
using System.Text;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public interface IContractBuilder
    {
        Contract Build(string name);

        Contract Build(string abi, string address);
    }
}
