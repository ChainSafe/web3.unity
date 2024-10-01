using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// A concrete implementation of <see cref="IConnectionHandler"/>.
    /// </summary>
    public class ConnectionHandler : MonoBehaviour, IConnectionHandler, ILightWeightServiceAdapter
    {
        // Handed in ConnectionHandlerEditor
        [HideInInspector, SerializeField] private ConnectionProvider[] providers;
        
        public HashSet<IServiceAdapter> Web3BuilderServiceAdapters { get; private set; }

        public ConnectionProvider[] Providers => providers;
        
        /// <summary>
        /// Initializes Connection Handler.
        /// </summary>
        public async Task Initialize()
        {
            Web3BuilderServiceAdapters = GetComponentsInChildren<IServiceAdapter>(true)
                .Concat(FindObjectsOfType<ServiceAdapter>(true)).ToHashSet();
            
            foreach (var provider in Providers)
            {
                await provider.Initialize();
            }
        }

        public async Task Restore()
        {
            var data = new StoredConnectionProviderData();

            await data.LoadOneTime();

            var provider = Providers.OfType<RestorableConnectionProvider>()
                .SingleOrDefault(p => p.GetType() == data.Type);
            
            if (provider != null && provider.RememberSession && await provider.SavedSessionAvailable())
            {
                await (this as IConnectionHandler).Connect(provider);
            }
        }

        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                var web3InitializedHandlers = GetComponents<IWeb3InitializedHandler>();

                foreach (var handler in web3InitializedHandlers)
                {
                    services.AddSingleton(handler);
                }
            });
        }

        public bool GetProvider<T>(out T provider) where T : ConnectionProvider
        {
            provider = Providers.FirstOrDefault(p => p.IsAvailable && p is T) as T;

            return provider != null;
        }

        private void Reset()
        {
            providers = Resources.LoadAll<ConnectionProvider>(string.Empty);
        }
    }
}