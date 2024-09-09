namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public class ChainManagerChainConfig : IChainConfig
    {
        private readonly IChainManager chainManager;

        public ChainManagerChainConfig(IChainManager chainManager)
        {
            this.chainManager = chainManager;
        }

        private IChainConfig CurrentConfig => chainManager.Current;

        public string ChainId => CurrentConfig.ChainId;

        public string Symbol => CurrentConfig.Symbol;

        public string Chain => CurrentConfig.Chain;

        public string Network => CurrentConfig.Network;

        public string Rpc => CurrentConfig.Rpc;

        public string Ws => CurrentConfig.Ws;

        public string BlockExplorerUrl => CurrentConfig.BlockExplorerUrl;
    }
}