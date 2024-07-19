using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// A concrete implementation of <see cref="IConnectionHandler"/>.
    /// </summary>
    public class ConnectionHandler : MonoBehaviour, IConnectionHandler, IWeb3BuilderServiceAdapter
    {
        [SerializeField] private string gelatoApiKey = "";
        [Space]
        [SerializeField] private ConnectModal connectModal;
        // Handed in ConnectionHandlerEditor
        [HideInInspector] [SerializeField] private ConnectionProvider[] providers;
        
        public string GelatoApiKey => gelatoApiKey;
        public IWeb3BuilderServiceAdapter[] Web3BuilderServiceAdapters { get; private set; }
        public ConnectionProvider ConnectionProvider { get; private set; }

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes Connection Handler.
        /// </summary>
        protected virtual async void Initialize()
        {
            Web3BuilderServiceAdapters = GetComponents<IWeb3BuilderServiceAdapter>();

            await TryRestore();
            
            foreach (var provider in providers)
            {
                if (provider != null && provider.IsAvailable)
                {
                    var instantiatedProvider = connectModal.AddProvider(provider);

                    await instantiatedProvider.Initialize();
                    
                    instantiatedProvider.ConnectButton.onClick.AddListener(delegate
                    {
                        ConnectionProvider = instantiatedProvider;
                        
                        ConnectClicked();
                    });
                }
            }
        }

        private async Task TryRestore()
        {
            var provider = await providers.OfType<RestorableConnectionProvider>().GetProvider();
            
            if (provider != null && await provider.SavedSessionAvailable())
            {
                ConnectionProvider = provider;
                
                await TryConnect();
            }
        }
        
        private async void ConnectClicked()
        {
            await TryConnect();
        }
        
        /// <summary>
        /// Try to Connect and displays error and throws exception on a failed attempt.
        /// </summary>
        public virtual async Task TryConnect()
        {
            try
            {
                connectModal.ShowLoading();

                await (this as IConnectionHandler).Connect();
            }
            catch (Exception e)
            {
                if (!(e is TaskCanceledException))
                {
                    connectModal.DisplayError(
                        $"Connection failed, please try again.");
                    
                    ConnectionProvider.HandleException(e);
                }
            }
            finally
            {
                if (connectModal != null)
                {
                    connectModal.HideLoading();
                }
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
    }
}