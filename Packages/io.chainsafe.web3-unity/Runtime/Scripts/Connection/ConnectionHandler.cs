﻿using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// A concrete implementation of <see cref="IConnectionHandler"/>.
    /// </summary>
    public class ConnectionHandler : MonoBehaviour, IConnectionHandler, IWeb3BuilderServiceAdapter
    {
        [SerializeField] private string gelatoApiKey = "";
        [Space]
        // Handed in ConnectionHandlerEditor
        [HideInInspector, SerializeField] private ConnectionProvider[] providers;
        
        public string GelatoApiKey => gelatoApiKey;
        public IWeb3BuilderServiceAdapter[] Web3BuilderServiceAdapters { get; private set; }

        public ConnectionProvider[] Providers => providers;
        
        /// <summary>
        /// Initializes Connection Handler.
        /// </summary>
        public async Task Initialize()
        {
            Web3BuilderServiceAdapters = GetComponents<IWeb3BuilderServiceAdapter>();

            foreach (var provider in Providers)
            {
                await provider.Initialize();
            }
        }

        public async Task<CWeb3> Restore()
        {
            var data = new StoredConnectionProviderData();

            await data.LoadOneTime();

            var provider = Providers.OfType<RestorableConnectionProvider>()
                .SingleOrDefault(p => p.GetType() == data.Type);
            
            if (provider != null && provider.RememberSession && await provider.SavedSessionAvailable())
            {
                return await (this as IConnectionHandler).Connect(provider);
            }

            return null;
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
    }
}