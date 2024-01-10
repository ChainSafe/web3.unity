using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IWalletConnectProviderNew
    {
        bool CanAutoLogin { get; } // todo move to separate interface

        Task<string> Connect();

        ValueTask Disconnect();
        Task<string> Request<T>(T data, long? expiry = null);
    }
}