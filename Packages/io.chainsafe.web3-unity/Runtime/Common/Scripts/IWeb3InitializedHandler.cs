namespace ChainSafe.Gaming.UnityPackage.Common
{
    /// <summary>
    /// Initialized handler used for executing logic when a web3 instance is initialized.
    /// </summary>
    public interface IWeb3InitializedHandler
    {
        /// <summary>
        /// Called when Web3 Instance in <see cref="ILoginProvider"/> is initialized.
        /// </summary>
        public void OnWeb3Initialized();
    }
}
