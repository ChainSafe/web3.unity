using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Connection;
using UnityEngine;

namespace ChainSafe.Gaming.Reown.Dialog
{
    /// <summary>
    /// Simple version of connection handler provider.
    /// Lacking Pool functionality and loading from Addressables.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Reown/Connection Handler Provider", fileName = "ConnectionHandlerProvider", order = 0)]
    public class ConnectionHandlerProviderChainSafe : ConnectionHandlerProviderAsset
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