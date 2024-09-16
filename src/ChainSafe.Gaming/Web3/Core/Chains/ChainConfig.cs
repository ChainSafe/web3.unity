using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// Concrete Implementation of <see cref="IChainConfig"/>.
    /// Holds all config files related to chain and network.
    /// </summary>
    [Serializable]
    public class ChainConfig : IChainConfig
    {
        /// <summary>
        /// Implementation of <see cref="IChainConfig.ChainId"/>
        /// Chain Id, eg. "5" for Goerli.
        /// </summary>
        public string ChainId { get; set; }

        /// <summary>
        /// Implementation of <see cref="IChainConfig.Symbol"/>
        /// Chain currency symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Implementation of <see cref="IChainConfig.Chain"/>
        /// Chain, for eg. "Ethereum".
        /// </summary>
        public string Chain { get; set; }

        /// <summary>
        /// Implementation of <see cref="IChainConfig.Network"/>
        /// Specific Chain Network, eg. "mainnet" or "goerli".
        /// </summary>
        public string Network { get; set; }

        /// <summary>
        /// Implementation of <see cref="IChainConfig.Rpc"/>
        /// RPC node link.
        /// </summary>
        public string Rpc { get; set; }

        /// <summary>
        /// Implementation of <see cref="IChainConfig.Ws"/>
        /// WebSocket link.
        /// </summary>
        public string Ws { get; set; }

        /// <summary>
        /// Implementation of <see cref="IChainConfig.BlockExplorerUrl"/>
        /// Chain block explorer.
        /// </summary>
        public string BlockExplorerUrl { get; set; }
    }
}