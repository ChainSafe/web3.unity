namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Helper class for the WalletConnect login procedure.
    /// </summary>
    public interface IConnectionHelper
    {
        /// <summary>
        /// True if there is a stored session that can be restored.
        /// No user input required in this case.
        /// </summary>
        bool StoredSessionAvailable { get; }
    }
}