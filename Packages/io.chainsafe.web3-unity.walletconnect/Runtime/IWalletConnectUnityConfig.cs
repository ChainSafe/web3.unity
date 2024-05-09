namespace ChainSafe.Gaming.WalletConnectUnity
{
    public interface IWalletConnectUnityConfig
    {
        public string[] IncludedWalletIds { get; }

        public string[] ExcludedWalletIds { get; }
    }
}