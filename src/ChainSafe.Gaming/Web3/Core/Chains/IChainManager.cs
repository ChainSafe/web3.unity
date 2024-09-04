using System;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public interface IChainManager
    {
        event Action<IChainConfig> ChainSwitched;

        IChainConfig Current { get; }

        Task SwitchChain(string newChainId);
    }
}