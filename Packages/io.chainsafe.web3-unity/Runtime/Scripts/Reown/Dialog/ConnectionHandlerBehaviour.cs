using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.Reown.Dialog
{
    public abstract class ConnectionHandlerBehaviour : MonoBehaviour, IConnectionHandler
    {
        public abstract Task ConnectUserWallet(ConnectionHandlerConfig config);
        public abstract void Terminate();
    }
}