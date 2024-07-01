using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the marketplace selection system.
    /// </summary>
    public class MarketplaceSelectionManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject marketplaceLoginPrefab;
        [SerializeField] private GameObject selectMarketplaceMenu;
        [SerializeField] private GameObject createMarketplaceMenu;
        [SerializeField] private GameObject browseMarketplaceMenu;
        [SerializeField] private Button createMarketplaceButton;
        [SerializeField] private Button browseMarketplaceButton;
        [SerializeField] private Button openSelectMarketplaceOptionButton;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes objects.
        /// </summary>
        private void Awake()
        {
            createMarketplaceButton.onClick.AddListener(OpenCreateMarketplaceMenu);
            browseMarketplaceButton.onClick.AddListener(OpenBrowseMarketplaceMenu);
            openSelectMarketplaceOptionButton.onClick.AddListener(OpenSelectMarketplaceOptionMenu);
            StartCoroutine(WaitForTokenExpiration());
        }

        /// <summary>
        /// Opens the create marketplace menu.
        /// </summary>
        private void OpenCreateMarketplaceMenu()
        {
            selectMarketplaceMenu.SetActive(false);
            createMarketplaceMenu.SetActive(true);
        }

        /// <summary>
        /// Opens the browse marketplace menu.
        /// </summary>
        private void OpenBrowseMarketplaceMenu()
        {
            selectMarketplaceMenu.SetActive(false);
            browseMarketplaceMenu.SetActive(true);
        }

        /// <summary>
        /// Opens the select marketplace option menu.
        /// </summary>
        private void OpenSelectMarketplaceOptionMenu()
        {
            createMarketplaceMenu.SetActive(false);
            browseMarketplaceMenu.SetActive(false);
            selectMarketplaceMenu.SetActive(true);
        }
        
        // TODO: Shouldn't be firing instantly, fix later.
        /// <summary>
        /// Waits for token expiry then refreshes it.
        /// </summary>
        private IEnumerator WaitForTokenExpiration()
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan timeToWait = MarketplaceAuth.BearerTokenExpires - currentTime;
            yield return new WaitForSeconds((float)timeToWait.TotalSeconds);
            Debug.Log("Refresh Expired");
            //MarketplaceAuth.RefreshExpiredToken();
        }

        #endregion
        
    }
}
