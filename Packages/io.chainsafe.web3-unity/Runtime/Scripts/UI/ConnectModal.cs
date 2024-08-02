using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    /// <summary>
    /// Connection Modal prompted for connecting to a provider.
    /// </summary>
    public class ConnectModal : MonoBehaviour
    {
        [SerializeField] private ErrorOverlay errorOverlay;
        [SerializeField] private LoadingOverlay loadingOverlay;
        [SerializeField] private Button closeButton;
        // Closes modal when background is clicked
        [SerializeField] private Button closeFromBackgroundButton;
        
        [Space]
        
        [SerializeField] private RectTransform modalContainer;
        [SerializeField] private RectTransform providerContainer;

        public ConnectionProvider[] Providers => _connectionHandler.Providers;
        
        private ConnectionHandler _connectionHandler;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
            closeFromBackgroundButton.onClick.AddListener(Close);
        }

        public void Initialize(ConnectionHandler connectionHandler)
        {
            _connectionHandler = connectionHandler;
            
            foreach (var provider in Providers)
            {
                if (provider != null && provider.IsAvailable)
                {
                    Button button = Instantiate(provider.ConnectButtonRow, providerContainer);

                    // Don't allow connection before initialization.
                    button.interactable = true;
                    
                    button.onClick.AddListener(delegate
                    {
                        ConnectClicked(provider);
                    });
                }
            }

            async void ConnectClicked(ConnectionProvider provider)
            {
                await TryConnect(provider);
            }
        }
        
        /// <summary>
        /// Try to Connect and displays error and throws exception on a failed attempt.
        /// </summary>
        private async Task TryConnect(ConnectionProvider provider)
        {
            try
            {
                ShowLoading();

                await (_connectionHandler as IConnectionHandler).Connect(provider);
            }
            catch (Exception e)
            {
                HideLoading();
                
                if (!(e is TaskCanceledException))
                {
                    DisplayError(
                        "Connection failed, please try again.");
                    
                    provider.HandleException(e);
                }
            }
        }

        public void Show()
        {
            modalContainer.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Display Error.
        /// </summary>
        /// <param name="message">Error Message.</param>
        private void DisplayError(string message)
        {
            errorOverlay.DisplayError(message);
        }
        
        /// <summary>
        /// Show Loading Overlay.
        /// </summary>
        private void ShowLoading()
        {
            loadingOverlay.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Hide Loading overlay.
        /// </summary>
        private void HideLoading()
        {
            loadingOverlay.gameObject.SetActive(false);
        }

        private void Close()
        {
            modalContainer.gameObject.SetActive(false);
        }
    }
}
