using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IConnectionDialog
    {
        Task ShowAndConnectUserWallet(ConnectionDialogConfig config);

        void CloseDialog();
    }
}