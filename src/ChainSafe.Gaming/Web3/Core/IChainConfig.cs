namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// Configuration object containing chain settings.
    /// </summary>
    public interface IChainConfig // TODO: double check these xml docs pls
    {
        /// <summary>
        /// The id of the chain to be used. Equals '1' for Ethereum Mainnet.
        /// </summary>
        public string ChainId { get; }

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
        /// The path to the IPC file to be used by the IPC provider by default.
        /// </summary>
        public string Ipc { get; }

        /// <summary>
        /// TODO.
        /// </summary>
        public string Ws { get; }
    }
}