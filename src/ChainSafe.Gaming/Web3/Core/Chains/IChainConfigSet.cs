using System.Collections.Generic;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public interface IChainConfigSet
    {
        IEnumerable<IChainConfig> Configs { get; }
    }
}