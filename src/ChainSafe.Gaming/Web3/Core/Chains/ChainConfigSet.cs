using System.Collections.Generic;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public class ChainConfigSet : List<IChainConfig>, IChainConfigSet
    {
        private readonly IChainConfig[] chainConfigs;

        public ChainConfigSet(params IChainConfig[] chainConfigs)
            : base(chainConfigs)
        {
            this.chainConfigs = chainConfigs;
        }

        public IEnumerable<IChainConfig> Configs => chainConfigs;
    }
}