using System.Threading.Tasks;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    [CreateAssetMenu(menuName = "ChainSafe/WalletConnect/Connection Dialog Provider", fileName = "ConnectionDialogProvider", order = 0)]
    public class ConnectionDialogProviderChainSafe : ConnectionDialogProviderSO
    {
        [SerializeField] private ConnectionDialog DialogPrefab;
    
        private ConnectionDialog loadedDialog;
        
        public override Task<IConnectionDialog> ProvideDialog()
        {
            if (loadedDialog != null)
            {
                return Task.FromResult((IConnectionDialog)loadedDialog);
            }

            loadedDialog = Instantiate(DialogPrefab);
            return Task.FromResult((IConnectionDialog)loadedDialog);
        }
    }
}