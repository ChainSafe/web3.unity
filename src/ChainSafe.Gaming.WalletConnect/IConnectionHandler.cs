using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IConnectionHandler
    {
        Task ConnectUserWallet(ConnectionDialogConfig config);

        void Terminate();
    }
}