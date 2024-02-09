namespace ChainSafe.Gaming.WalletConnect.Storage
{
    /// <summary>
    /// A class representing a local data used by the WalletConnect integration.
    /// </summary>
    public class LocalData
    {
        /// <summary>
        /// Session topic used to restore previously connected session.
        /// </summary>
        public string SessionTopic { get; set; }

        /// <summary>
        /// The name of the locally connected wallet.
        /// </summary>
        public string ConnectedLocalWalletName { get; set; }
    }
}