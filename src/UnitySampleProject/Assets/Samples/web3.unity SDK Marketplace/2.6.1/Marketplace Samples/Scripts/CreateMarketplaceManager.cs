using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the marketplace creation GUI.
    /// </summary>
    public class CreateMarketplaceManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject selectMarketplaceMenu;
        [SerializeField] private GameObject createMarketplaceMenu;
        [SerializeField] private Button openSelectMarketplaceOptionButton;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes objects.
        /// </summary>
        private void Awake()
        {
            openSelectMarketplaceOptionButton.onClick.AddListener(OpenSelectMarketplaceOptionMenu);
        }

        /// <summary>
        /// Opens the select marketplace option menu.
        /// </summary>
        private void OpenSelectMarketplaceOptionMenu()
        {
            createMarketplaceMenu.SetActive(false);
            selectMarketplaceMenu.SetActive(true);
        }

        #endregion
    }
}
