namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// Configuration object containing chain settings.
    /// </summary>
    public interface IChainConfig
    {
        /// <summary>
        /// The id of the chain to be used. Equals '1' for Ethereum Mainnet.
        /// </summary>
        public string ChainId { get; }

        /// <summary>
        /// Token symbol ID.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// The name of the chain to be used. Equals 'Ethereum' for Ethereum Mainnet.
        /// </summary>
        public string Chain { get; }

        /// <summary>
        /// The name of the network to be used. Equals 'Ethereum Mainnet' for Ethereum Mainnet.
        /// </summary>
        public string Network { get; }

        /// <summary>
        /// The URI for the RPC endpoint to be used by the RPC provider by default.
        /// </summary>
        public string Rpc { get; }

        /// <summary>
        /// The WebSocket URI for the chain, used for real-time blockchain event subscriptions.
        /// </summary>
        public string Ws { get; }

        /// <summary>
        /// The block explorer URL for the active chain.
        /// </summary>
        public string BlockExplorerUrl { get; }
    }
}