using System.Threading.Tasks;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialog : MonoBehaviour, IConnectionDialog
    {
        public abstract Task ShowAndConnectUserWallet(ConnectionDialogConfig config);
        public abstract void CloseDialog();
    }
}