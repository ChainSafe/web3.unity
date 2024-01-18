using System.Threading.Tasks;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialog : MonoBehaviour, IConnectionHandler
    {
        public abstract Task ConnectUserWallet(ConnectionDialogConfig config);
        public abstract void Terminate();
    }
}