using System.Threading.Tasks;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialogProviderSO : ScriptableObject, IConnectionHandlerProvider
    {
        public abstract Task<IConnectionHandler> ProvideHandler();
    }
}