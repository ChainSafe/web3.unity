using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    /// <summary>
    /// Simple version of connection handler provider.
    /// Lacking Pool functionality and loading from Addressables.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/WalletConnect/Connection Handler Provider", fileName = "ConnectionHandlerProvider", order = 0)]
    public class ConnectionHandlerProviderChainSafe : ConnectionHandlerProviderSO
    {
        [SerializeField] private ConnectionHandlerBehaviour handlerPrefab;

        private ConnectionHandlerBehaviour loadedHandler;

        public override Task<IConnectionHandler> ProvideHandler()
        {
            if (loadedHandler != null)
            {
                return Task.FromResult((IConnectionHandler)loadedHandler);
            }

            loadedHandler = Instantiate(handlerPrefab);
            return Task.FromResult((IConnectionHandler)loadedHandler);
        }
    }
}