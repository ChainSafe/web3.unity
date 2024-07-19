using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// A concrete implementation of <see cref="IConnectionHandler"/>.
    /// </summary>
    public class ConnectionHandler : MonoBehaviour, IConnectionHandler, IWeb3BuilderServiceAdapter
    {
        [SerializeField] private string gelatoApiKey = "";
        [Space]
        [SerializeField] private bool autoConnectToPreviousSession;
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

            if (autoConnectToPreviousSession)
            {
                await TryRestore();
            }
            
            foreach (var provider in providers)
            {
                if (provider != null && provider.IsAvailable)
                {
                    Button button = connectModal.AddProvider(provider.ConnectButtonRow);
                    
                    await provider.Initialize();

                    // Don't allow connection before initialization.
                    button.interactable = true;
                    
                    button.onClick.AddListener(delegate
                    {
                        ConnectionProvider = provider;
                        
                        ConnectClicked();
                    });
                }
            }
        }

        private async Task TryRestore()
        {
            var provider = await providers.OfType<RestorableConnectionProvider>().GetProvider();
            
            if (provider != null && provider.RememberSession && await provider.SavedSessionAvailable())
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