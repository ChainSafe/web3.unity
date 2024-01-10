using System.Threading.Tasks;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialogProviderSO : ScriptableObject, IConnectionDialogProvider
    {
        public abstract Task<IConnectionDialog> ProvideDialog();
    }
}