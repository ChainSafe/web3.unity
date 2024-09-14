using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.NetCore
{
    public static class ChainConfigExtensions
    {
        public static IChainConfig Clone(this IChainConfig chainConfig)
        {
            return new ChainConfig
            {
                Chain = chainConfig.Chain,
                ChainId = chainConfig.ChainId,
                Network = chainConfig.Network,
                Rpc = chainConfig.Rpc,
                Symbol = chainConfig.Symbol,
                BlockExplorerUrl = chainConfig.BlockExplorerUrl,
                Ws = chainConfig.Ws,
            };
        }
    }
}