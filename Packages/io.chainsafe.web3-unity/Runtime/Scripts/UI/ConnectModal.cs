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
        
        [SerializeField] private RectTransform providerContainer;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
            closeFromBackgroundButton.onClick.AddListener(Close);
        }

        /// <summary>
        /// Display Error.
        /// </summary>
        /// <param name="message">Error Message.</param>
        public void DisplayError(string message)
        {
            errorOverlay.DisplayError(message);
        }
        
        /// <summary>
        /// Show Loading Overlay.
        /// </summary>
        public void ShowLoading()
        {
            loadingOverlay.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Hide Loading overlay.
        /// </summary>
        public void HideLoading()
        {
            loadingOverlay.gameObject.SetActive(false);
        }

        /// <summary>
        /// Add connection provider to the modal.
        /// </summary>
        /// <param name="provider">ConnectionProvider prefab to be added.</param>
        /// <returns>Added connection provider.</returns>
        public ConnectionProvider AddProvider(ConnectionProvider provider)
        {
            return Instantiate(provider, providerContainer);
        }
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
