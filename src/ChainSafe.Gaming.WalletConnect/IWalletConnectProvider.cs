using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IWalletConnectProvider
    {
        Task<string> Connect();

        Task Disconnect();

        Task<string> Request<T>(T data, long? expiry = null);
    }
}