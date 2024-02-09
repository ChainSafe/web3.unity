namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Delegate used to redirect user to one of the locally installed wallets.
    /// </summary>
    public delegate void OpenLocalWalletDelegate(string walletName = null);
}