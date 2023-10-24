namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// Chain model containing fields used for Wallet Connect.
    /// </summary>
    public class ChainModel
    {
        /// <summary>
        /// Default namespace for EVM.
        /// </summary>
        public const string EvmNamespace = "eip155";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainModel"/> class.
        /// </summary>
        /// <param name="chainNamespace">Chain Namespace - example "eip155:" for EVMs.</param>
        /// <param name="chainId">Chain Id.</param>
        /// <param name="name">Name of the network.</param>
        public ChainModel(string chainNamespace, string chainId, string name)
        {
            ChainNamespace = chainNamespace;

            ChainId = chainId;

            Name = name;
        }

        /// <summary>
        /// Chain Namespace - example "eip155:" for EVMs.
        /// </summary>
        public string ChainNamespace { get; private set; }

        /// <summary>
        /// Chain Id.
        /// </summary>
        public string ChainId { get; private set; }

        /// <summary>
        /// Name of the network.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Full chain Id, together with <see cref="ChainNamespace"/>.
        /// </summary>
        public string FullChainId => $"{ChainNamespace}:{ChainId}";
    }
}