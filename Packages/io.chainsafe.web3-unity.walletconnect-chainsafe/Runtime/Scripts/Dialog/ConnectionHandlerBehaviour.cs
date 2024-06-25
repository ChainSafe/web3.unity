using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionHandlerBehaviour : MonoBehaviour, IConnectionHandler
    {
        public abstract Task ConnectUserWallet(ConnectionHandlerConfig config);
        public abstract void Terminate();
    }
}