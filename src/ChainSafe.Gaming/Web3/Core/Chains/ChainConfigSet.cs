using System.Collections.Generic;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public class ChainConfigSet : List<IChainConfig>, IChainConfigSet
    {
        public ChainConfigSet(params IChainConfig[] chainConfigs)
            : base(chainConfigs)
        {
        }
    }
}