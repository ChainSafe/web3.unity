using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    /// <summary>
    /// Simple version of connection dialog provider.
    /// Lacking Pool functionality and loading from Addressables.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/WalletConnect/Connection Dialog Provider", fileName = "ConnectionDialogProvider", order = 0)]
    public class ConnectionDialogProviderChainSafe : ConnectionDialogProviderSO
    {
        [SerializeField] private ConnectionDialog DialogPrefab;
    
        private ConnectionDialog loadedDialog;
        
        public override Task<IConnectionHandler> ProvideHandler()
        {
            if (loadedDialog != null)
            {
                return Task.FromResult((IConnectionHandler)loadedDialog);
            }

            loadedDialog = Instantiate(DialogPrefab);
            return Task.FromResult((IConnectionHandler)loadedDialog);
        }
    }
}