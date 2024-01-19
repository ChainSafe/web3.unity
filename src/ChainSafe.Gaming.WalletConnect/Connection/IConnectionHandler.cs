using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect.Connection
{
    public interface IConnectionHandler
    {
        Task ConnectUserWallet(ConnectionHandlerConfig config);

        void Terminate();
    }
}