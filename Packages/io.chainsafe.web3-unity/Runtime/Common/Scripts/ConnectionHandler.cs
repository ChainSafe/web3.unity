using System;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage.Common;
using UnityEngine;

namespace Scenes
{
    /// <summary>
    /// A concrete implementation of <see cref="IConnectionHandler"/>.
    /// </summary>
    public class ConnectionHandler : MonoBehaviour, IConnectionHandler
    {
        [SerializeField] private string gelatoApiKey = "";
        [Space]
        [SerializeField] private ConnectModal connectModal;
        [SerializeField] private ConnectionProvider[] providers;
        
        public string GelatoApiKey => gelatoApiKey;
        public IWeb3BuilderServiceAdapter[] Web3BuilderServiceAdapters { get; private set; }
        public IWeb3InitializedHandler[] Web3InitializedHandlers { get; private set; }
        public ConnectionProvider ConnectionProvider { get; private set; }

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes Connection Handler.
        /// </summary>
        protected virtual void Initialize()
        {
            Web3BuilderServiceAdapters = GetComponents<IWeb3BuilderServiceAdapter>();

            Web3InitializedHandlers = GetComponents<IWeb3InitializedHandler>();
            
            foreach (var provider in providers)
            {
                if (provider.IsAvailable)
                {
                    var instantiatedProvider = connectModal.AddProvider(provider);
                    
                    instantiatedProvider.ConnectButton.onClick.AddListener(delegate
                    {
                        ConnectionProvider = provider;
                        
                        ConnectClicked();
                    });
                }
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
                connectModal.DisplayError(
                    $"Connecting failed, please try again\n{e.Message} (see console for more details)");

                throw;
            }
            finally
            {
                connectModal.HideLoading();
            }
        }
    }
}