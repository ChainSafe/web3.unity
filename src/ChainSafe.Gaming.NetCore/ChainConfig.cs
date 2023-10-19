using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// Concrete Implementation of <see cref="IChainConfig"/>.
    /// Holds all config files related to chain and network.
    /// </summary>
    public class ChainConfig : IChainConfig
    {
        /// <summary>
        /// Chain Id, eg. "5" for Goerli.
        /// </summary>
        public string ChainId { get; set; }

        /// <summary>
        /// Chain, for eg. "Ethereum".
        /// </summary>
        public string Chain { get; set; }

        /// <summary>
        /// Specific Chain Network, eg. "mainnet" or "goerli".
        /// </summary>
        public string Network { get; set; }

        /// <summary>
        /// RPC node link.
        /// </summary>
        public string Rpc { get; set; }

        /// <summary>
        /// IPC link.
        /// </summary>
        public string Ipc { get; set; }

        /// <summary>
        /// WebSocket link.
        /// </summary>
        public string Ws { get; set; }
    }
}