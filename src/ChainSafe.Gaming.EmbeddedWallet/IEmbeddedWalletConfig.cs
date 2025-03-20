namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Configuration for the Embedded Wallet.
    /// </summary>
    public interface IEmbeddedWalletConfig
    {
        /// <summary>
        /// Automatically execute transactions without user approval.
        /// </summary>
        public bool AutoApproveTransactions { get; }
    }
}