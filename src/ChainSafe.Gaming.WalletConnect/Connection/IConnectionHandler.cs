using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect.Connection
{
    /// <summary>
    /// Connection handler used to choose a wallet when connecting a new session.
    /// This is usually used to show some sort of connection dialog to the user.
    /// </summary>
    public interface IConnectionHandler
    {
        Task ConnectUserWallet(ConnectionHandlerConfig config);

        void Terminate();
    }
}