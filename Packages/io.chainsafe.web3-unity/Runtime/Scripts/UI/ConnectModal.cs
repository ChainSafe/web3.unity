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
        [SerializeField] private Button closeButton;
        // Closes modal when background is clicked
        [SerializeField] private Button closeFromBackgroundButton;

        [Space]

        [SerializeField] private RectTransform modalContainer;
        [SerializeField] private RectTransform providerContainer;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
            closeFromBackgroundButton.onClick.AddListener(Close);
        }

        public void Initialize(ConnectionProvider[] providers)
        {
            foreach (var provider in providers)
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

                await Web3Unity.Instance.Connect(provider);
            }
            catch (Exception e)
            {
                if (!(e is TaskCanceledException))
                {
                    DisplayError("Connection failed, please try again.");
                }
            }
            finally
            {
                HideLoading();
            }
        }

        public void Open()
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
        private void ShowLoading(string text = "")
        {
            LoadingOverlay.ShowLoadingOverlay(text);
        }

        /// <summary>
        /// Hide Loading overlay.
        /// </summary>
        private void HideLoading()
        {
            LoadingOverlay.HideLoadingOverlay();
        }

        public void Close()
        {
            modalContainer.gameObject.SetActive(false);
        }
    }
}
