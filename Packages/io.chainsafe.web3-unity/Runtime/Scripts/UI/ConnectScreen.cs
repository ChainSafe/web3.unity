using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.GUI;
using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    /// <summary>
    /// Connection Modal prompted for connecting to a provider.
    /// </summary>
    public class ConnectScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform providerContainer;
        [SerializeField] private ConnectionProviderButton providerButtonPrefab;

        public void Initialize(ConnectionProvider[] providers)
        {
            foreach (var provider in providers)
            {
                if (provider != null && provider.IsAvailable)
                {
                    var button = Instantiate(providerButtonPrefab, providerContainer);
                    button.Set(provider.ButtonIcon, provider.ButtonText, () => ConnectClicked(provider));
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
                ShowLoading("Establishing connection...");

                await Web3Unity.Instance.Connect(provider);
            }
            catch (Exception e)
            {

                if (!(e is TaskCanceledException))
                {
                    DisplayError(
                        "Connection failed, please try again.");
                }
            }
            finally
            {
                HideLoading();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Display Error.
        /// </summary>
        /// <param name="message">Error Message.</param>
        private void DisplayError(string message)
        {
            DisableButtons();
            GuiManager.Instance.Overlays.Show(GuiOverlayType.Error, message, true, EnableButtons);
        }

        /// <summary>
        /// Show Loading Overlay.
        /// </summary>
        private void ShowLoading(string message)
        {
            DisableButtons();
            GuiManager.Instance.Overlays.Show(GuiOverlayType.Loading, message, false, EnableButtons);
        }
        
        /// <summary>
        /// Hide Loading overlay.
        /// </summary>
        private void HideLoading()
        {
            GuiManager.Instance.Overlays.Hide();
        }

        private void EnableButtons()
        {
            providerContainer.gameObject.SetActive(true);
        }

        private void DisableButtons()
        {
            providerContainer.gameObject.SetActive(false);
        }
    }
}
