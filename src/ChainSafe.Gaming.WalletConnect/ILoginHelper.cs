namespace ChainSafe.Gaming.WalletConnect
{
    public interface ILoginHelper
    {
        bool StoredSessionAvailable { get; }
    }
}