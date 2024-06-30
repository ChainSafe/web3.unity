using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the marketplace selection system.
    /// </summary>
    public class MarketplaceGUIManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject marketplaceLoginPrefab;
        [SerializeField] private List<GameObject> primaryBackgroundObjects;
        [SerializeField] private List<GameObject> menuBackgroundObjects;
        [SerializeField] private List<GameObject> primaryTextObjects;
        [SerializeField] private List<GameObject> secondaryTextObjects;
        [SerializeField] private List<GameObject> displayLineObjects;
        [SerializeField] private List<GameObject> borderButtonObjects;
        [SerializeField] private GameObject selectionMenu;
        [SerializeField] private GameObject browseMarketplacesMenu;
        [SerializeField] private GameObject browseCollectionsMenu;
        [SerializeField] private GameObject createMarketplaceMenu;
        [SerializeField] private GameObject createCollectionMenu;
        [SerializeField] private GameObject mintNftToCollectionMenu;
        [SerializeField] private GameObject listNftToMarketplaceMenu;
        [SerializeField] private Button openMarketplacesMenuButton;
        [SerializeField] private Button openCollectionsMenuButton;
        [SerializeField] private Button createMarketplaceButton;
        [SerializeField] private Button createMarketplaceUploadImageButton;
        [SerializeField] private Button closeCreateMarketplaceButton;
        [SerializeField] private Button createCollectionButton;
        [SerializeField] private Button createCollectionUploadImageButton;
        [SerializeField] private Button closeCreateCollectionButton;
        [SerializeField] private Button mintNftToCollectionMenuButton;
        [SerializeField] private Button mintNftToCollectionButton;
        [SerializeField] private Button listNftToMarketplaceMenuButton;
        [SerializeField] private Button listNftToMarketplaceButton;
        [SerializeField] private Button backButtonBrowseSelectedMarketplace;
        [SerializeField] private Button backButtonBrowseSelectedCollection;
        [SerializeField] private Button backButtonBrowseMarketplace;
        [SerializeField] private Button backButtonBrowseCollection;
        [SerializeField] private Button logOutButton;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes objects.
        /// </summary>
        private void Awake()
        {
            logOutButton.onClick.AddListener(MarketplaceLogout);
            openMarketplacesMenuButton.onClick.AddListener(ToggleMarketplacesMenu);
            openCollectionsMenuButton.onClick.AddListener(ToggleCollectionsMenu);
            createMarketplaceButton.onClick.AddListener(ToggleCreateMarketplaceMenu);
            closeCreateMarketplaceButton.onClick.AddListener(ToggleCreateMarketplaceMenu);
            createCollectionButton.onClick.AddListener(ToggleCreateCollectionMenu);
            closeCreateCollectionButton.onClick.AddListener(ToggleCreateCollectionMenu);
            createMarketplaceUploadImageButton.onClick.AddListener(UploadMarketplaceImage);
            createCollectionUploadImageButton.onClick.AddListener(UploadCollectionImage);
            mintNftToCollectionMenuButton.onClick.AddListener(ToggleMintNftToCollectionMenu);
            mintNftToCollectionButton.onClick.AddListener(UploadNftImageToCollection);
            listNftToMarketplaceMenuButton.onClick.AddListener(ToggleListNftToMarketplaceMenu);
            listNftToMarketplaceButton.onClick.AddListener(ListNftToMarketplace);
            backButtonBrowseSelectedMarketplace.onClick.AddListener(ToggleSelectedMarketplace);
            backButtonBrowseSelectedCollection.onClick.AddListener(ToggleSelectedCollection);
            backButtonBrowseMarketplace.onClick.AddListener(BackToSelectionMenu);
            backButtonBrowseCollection.onClick.AddListener(BackToSelectionMenu);
        }
        
        /// <summary>
        /// Sets custom colours.
        /// </summary>
        private void SetCustomColours(object sender, EventManagerMarketplace.MarketplaceGUIConfigEventArgs marketplaceGUIConfigEventArgs)
        {
            CustomizationHelperMarketplace.SetCustomColours(
                EventManagerMarketplace.MarketplaceGUIConfigEventArgs.DisplayFont,
                primaryBackgroundObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.PrimaryBackgroundColour,
                menuBackgroundObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.MenuBackgroundColour,
                primaryTextObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.PrimaryTextColour,
                secondaryTextObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.SecondaryTextColour,
                borderButtonObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.BorderButtonColour,
                displayLineObjects);
        }
        
        /// <summary>
        /// Toggles browse marketplaces menu.
        /// </summary>
        private void ToggleMarketplacesMenu()
        {
            EventManagerMarketplace.RaiseToggleMarketplacesMenu();
            browseMarketplacesMenu.SetActive(!browseMarketplacesMenu.activeSelf);
            selectionMenu.SetActive(!selectionMenu.activeSelf);
        }
        
        /// <summary>
        /// Toggles browse collections menu.
        /// </summary>
        private void ToggleCollectionsMenu()
        {
            EventManagerMarketplace.RaiseToggleCollectionsMenu();
            browseCollectionsMenu.SetActive(!browseCollectionsMenu.activeSelf);
            selectionMenu.SetActive(!selectionMenu.activeSelf);
        }
        
        /// <summary>
        /// Toggles create marketplace menu.
        /// </summary>
        private void ToggleCreateMarketplaceMenu()
        {
            EventManagerMarketplace.RaiseToggleCreateMarketplaceMenu();
            browseMarketplacesMenu.SetActive(!browseMarketplacesMenu.activeSelf);
            createMarketplaceMenu.SetActive(!createMarketplaceMenu.activeSelf);
        }
        
        /// <summary>
        /// Toggles create collection menu.
        /// </summary>
        private void ToggleCreateCollectionMenu()
        {
            EventManagerMarketplace.RaiseToggleCreateCollectionMenu();
            browseCollectionsMenu.SetActive(!browseCollectionsMenu.activeSelf);
            createCollectionMenu.SetActive(!createCollectionMenu.activeSelf);
        }
        
        /// <summary>
        /// Toggles Selected Collection.
        /// </summary>
        private void ToggleSelectedCollection()
        {
            EventManagerMarketplace.RaiseToggleSelectedCollection();
            mintNftToCollectionMenuButton.gameObject.SetActive(!mintNftToCollectionMenuButton.gameObject.activeSelf);
            backButtonBrowseSelectedCollection.gameObject.SetActive(!backButtonBrowseSelectedCollection.gameObject.activeSelf);
            backButtonBrowseCollection.gameObject.SetActive(!backButtonBrowseCollection.gameObject.activeSelf);
        }
        
        /// <summary>
        /// Toggles Selected Marketplace.
        /// </summary>
        private void ToggleSelectedMarketplace()
        {
            EventManagerMarketplace.RaiseToggleSelectedMarketplace();
            listNftToMarketplaceMenuButton.gameObject.SetActive(!listNftToMarketplaceMenuButton.gameObject.activeSelf);
            backButtonBrowseSelectedMarketplace.gameObject.SetActive(!backButtonBrowseSelectedMarketplace.gameObject.activeSelf);
            backButtonBrowseMarketplace.gameObject.SetActive(!backButtonBrowseMarketplace.gameObject);
        }
        
        /// <summary>
        /// Toggles selection menu.
        /// </summary>
        private void BackToSelectionMenu()
        {
            EventManagerMarketplace.RaiseToggleSelectionMenu();
            browseMarketplacesMenu.SetActive(false);
            browseCollectionsMenu.SetActive(false);
            selectionMenu.SetActive(!selectionMenu.activeSelf);
        }
        
        /// <summary>
        /// Uploads collection image.
        /// </summary>
        private void UploadCollectionImage()
        {
            EventManagerMarketplace.RaiseUploadCollectionImage();
        }
        
        /// <summary>
        /// Uploads marketplace image.
        /// </summary>
        private void UploadMarketplaceImage()
        {
            EventManagerMarketplace.RaiseUploadMarketplaceImage();
        }
        
        /// <summary>
        /// Uploads marketplace image.
        /// </summary>
        private void UploadNftImageToCollection()
        {
            EventManagerMarketplace.RaiseUploadNftImageToCollection();
        }
        
        /// <summary>
        /// Toggles mint nft to collection menu.
        /// </summary>
        private void ToggleMintNftToCollectionMenu()
        {
            mintNftToCollectionMenu.SetActive(!mintNftToCollectionMenu.activeSelf);
            EventManagerMarketplace.RaiseToggleMintNftToSelectionMenu();
        }

        /// <summary>
        /// Toggles list nft to marketplace menu.
        /// </summary>
        private void ToggleListNftToMarketplaceMenu()
        {
            listNftToMarketplaceMenu.SetActive(!listNftToMarketplaceMenu.activeSelf);
            EventManagerMarketplace.RaiseToggleListNftToMarketplaceMenu();
        }
        
        /// <summary>
        /// Lists nft to marketplace.
        /// </summary>
        private void ListNftToMarketplace()
        {
            EventManagerMarketplace.RaiseListNftToMarketplace();
        }
        
        /// <summary>
        /// Logs out of the marketplace.
        /// </summary>
        private void MarketplaceLogout()
        {
            EventManagerMarketplace.RaiseLogoutMarketplace();
            Instantiate(marketplaceLoginPrefab);
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.ConfigureMarketplaceGuiManager += SetCustomColours;
            EventManagerMarketplace.ToggleSelectedMarketplace += ToggleSelectedMarketplace;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureMarketplaceGuiManager -= SetCustomColours;
            EventManagerMarketplace.ToggleSelectedMarketplace -= ToggleSelectedMarketplace;
        }

        #endregion

    }
}
