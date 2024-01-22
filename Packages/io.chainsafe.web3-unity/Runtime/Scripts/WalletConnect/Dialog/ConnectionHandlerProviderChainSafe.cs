using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Connection;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    /// <summary>
    /// Simple version of connection dialog provider.
    /// Lacking Pool functionality and loading from Addressables.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/WalletConnect/Connection Dialog Provider", fileName = "ConnectionDialogProvider", order = 0)]
    public class ConnectionHandlerProviderChainSafe : ConnectionDialogProviderSO
    {
        [SerializeField] private ConnectionHandlerBehaviour HandlerPrefab;
    
        private ConnectionHandlerBehaviour loadedHandler;
        
        public override Task<IConnectionHandler> ProvideHandler()
        {
            if (loadedHandler != null)
            {
                return Task.FromResult((IConnectionHandler)loadedHandler);
            }

            loadedHandler = Instantiate(HandlerPrefab);
            return Task.FromResult((IConnectionHandler)loadedHandler);
        }
    }
}