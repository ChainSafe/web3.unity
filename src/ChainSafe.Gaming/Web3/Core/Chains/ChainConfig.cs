using System;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Chains;

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
        /// Implementation of <see cref="IChainConfig.NativeCurrency"/>.
        /// </summary>
        public INativeCurrency NativeCurrency { get; set; }

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

        /// <summary>
        /// Implementation of <see cref="IChainConfig.BlockExplorerUrl"/>
        /// This field is only relevant if you are using Unity with WebGL and the AppKit package.
        /// </summary>
        public string ViemName { get; set; }
    }
}