using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Marketplace;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;
using ChainSafe.Gaming.Marketplace.Models;

namespace ChainSafe.Gaming.Collection
{
    /// <summary>
    /// Manages the collection browse GUI.
    /// </summary>
    public class BrowseCollectionManager : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private GameObject projectCollectionsPrefab;
        [SerializeField] private GameObject CollectionItemPrefab;
        [SerializeField] private GameObject CollectionPanel;
        [SerializeField] private ScrollRect CollectionScrollRect;
        private List<ApiResponse.Project> projects;
        private GameObject[] projectCollectionsPrefabs;
        private int projectCollectionsObjectNumber = 1;
        private int projectCollectionsDisplayCount = 100;
        private GameObject[] CollectionItemPrefabs;
        private int CollectionItemsObjectNumber = 1;
        private int CollectionItemDisplayCount = 100;
        private string baseUrl = "https://ipfs.chainsafe.io/ipfs/";

        #endregion
        
        #region Properties
        
        private string BearerToken { get; set; }
        private TMP_FontAsset DisplayFont { get; set; }
        private Color SecondaryTextColour { get; set; }
        
    
        #endregion

        #region Methods

        /// <summary>
        /// Initializes objects.
        /// </summary>
        private void Awake()
        {
            projectCollectionsPrefabs = new GameObject[projectCollectionsDisplayCount];
            CollectionItemPrefabs = new GameObject[CollectionItemDisplayCount];
        }
        
        /// <summary>
        /// Populates the Collection drop down options.
        /// </summary>
        private async void GetProjectCollections()
        {
            var response = await EvmMarketplace.GetProjectCollections(BearerToken);
            if (response.collections.Count > 0)
            {
                PopulateCollections(response);
            }
        }

        /// <summary>
        /// Populates Collections and adds them to the display panel.
        /// </summary>
        private async void PopulateCollections(UnityPackage.Model.NftTokenModel.ProjectCollectionsResponse collectionsResponse)
        {
            foreach (var collection in collectionsResponse.collections.Where(collection => collection.type is "ERC721" or "ERC1155"))
            {
                await AddCollectionToDisplay(collection.name, collection.type, baseUrl + collection.banner);
            }

            EventManagerMarketplace.RaiseToggleProcessingMenu();
        }

        /// <summary>
        /// Populates items to be added to the Collection display.
        /// </summary>
        /// <param name="index">The index of the project to populate from.</param>
        /// <param name="collectionType">Collection type.</param>
        private async void PopulateCollectionItems(int index, string collectionType)
        {
            var projectResponse = await EvmMarketplace.GetProjectTokens();
            if (index >= projectResponse.tokens.Count)
            {
                EventManagerMarketplace.RaiseToggleProcessingMenu();
                return;
            }
            var collectionContract = projectResponse.tokens[index].contract_address;
            var mintCollectionNftConfigArgs = new EventManagerMarketplace.MintCollectionNftConfigEventArgs(null, collectionContract, collectionType);
            EventManagerMarketplace.RaiseMintCollectionNftManager(mintCollectionNftConfigArgs);
            switch (collectionType)
            {
                case "ERC721":
                {
                    var response = await EvmMarketplace.GetCollectionTokens721(projectResponse.tokens[index].collection_id);
                    foreach (var item in response.tokens)
                    {
                        await AddCollectionItemToDisplay(collectionContract, item.token_id, item.token_type, item.supply, item.uri);
                    }
                    EventManagerMarketplace.RaiseToggleProcessingMenu();

                    break;
                }
                case "ERC1155":
                {
                    var response = await EvmMarketplace.GetCollectionTokens1155(projectResponse.tokens[index].collection_id);
                    foreach (var item in response.tokens)
                    {
                        await AddCollectionItemToDisplay(collectionContract, item.token_id, item.token_type, item.supply, item.uri);
                    }
                    EventManagerMarketplace.RaiseToggleProcessingMenu();
    
                    break;
                }
                default:
                    Debug.Log("No NFT type given, returning");
                    EventManagerMarketplace.RaiseToggleProcessingMenu();
                    return;
            }
        }

        /// <summary>
        /// Adds Collection to the display panel.
        /// </summary>
        /// <param name="collectionName">Collection name to add.</param>
        /// <param name="collectionType">Collection type.</param>
        /// <param name="collectionBannerUri">Collection image uri to add.</param>
        private async Task AddCollectionToDisplay(string collectionName, string collectionType, string collectionBannerUri)
        {
            if (projectCollectionsObjectNumber >= projectCollectionsDisplayCount)
            {
                Destroy(projectCollectionsPrefabs[0]);
                for (int i = 1; i < projectCollectionsPrefabs.Length; i++)
                {
                    projectCollectionsPrefabs[i - 1] = projectCollectionsPrefabs[i];
                }
                projectCollectionsPrefabs[projectCollectionsPrefabs.Length - 1] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                await UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionType, collectionBannerUri);
            }
            else
            {
                projectCollectionsPrefabs[projectCollectionsObjectNumber] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                await UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionType, collectionBannerUri);
            }
            projectCollectionsObjectNumber++;
            CollectionScrollRect.horizontalNormalizedPosition = 0;
        }

        /// <summary>
        /// Adds items to the Collection display.
        /// </summary>
        /// <param name="collectionContract">Collection contract.</param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft type.</param>
        /// <param name="supply">Nft supply.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private async Task AddCollectionItemToDisplay(string collectionContract, string nftId, string nftType, string supply, string nftUri)
        {
            if (CollectionItemsObjectNumber >= CollectionItemDisplayCount)
            {
                Destroy(CollectionItemPrefabs[0]);
                for (int i = 1; i < CollectionItemPrefabs.Length; i++)
                {
                    CollectionItemPrefabs[i - 1] = CollectionItemPrefabs[i];
                }
                CollectionItemPrefabs[CollectionItemPrefabs.Length - 1] = Instantiate(CollectionItemPrefab, CollectionPanel.transform);
                await UpdateCollectionItemDisplay(collectionContract, CollectionItemsObjectNumber, nftId, nftType, supply, nftUri);
            }
            else
            {
                CollectionItemPrefabs[CollectionItemsObjectNumber] = Instantiate(CollectionItemPrefab, CollectionPanel.transform);
                await UpdateCollectionItemDisplay(collectionContract, CollectionItemsObjectNumber, nftId, nftType, supply, nftUri);
            }
            CollectionItemsObjectNumber++;
            CollectionScrollRect.horizontalNormalizedPosition = 0;
        }
        
        /// <summary>
        /// Imports texture (can probably be removed later for helper class)
        /// </summary>
        /// <param name="uri">Nft uri</param>
        private async Task<Texture2D> ImportTexture(string uri)
        {
            var textureRequest = UnityWebRequestTexture.GetTexture(uri);
            await textureRequest.SendWebRequest();
            if (textureRequest.result != UnityWebRequest.Result.Success)
            {
                throw new Web3Exception($"Texture request failure: {textureRequest.error}");
            }
            var texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            return texture;
        }

        /// <summary>
        /// Updates the Collections display.
        /// </summary>
        /// <param name="projectCollectionsObjectIndex">Index of Collection.</param>
        /// <param name="collectionName">Collection name.</param>
        /// <param name="collectionType">Collection type.</param>
        /// <param name="collectionBannerUri">Collection Uri.</param>
        private async Task UpdateProjectCollectionsDisplay(int projectCollectionsObjectIndex, string collectionName, string collectionType, string collectionBannerUri)
        {
            //Debug.Log($"COLLECTION TYPE: {collectionType}");
            string[] textObjectNames = { "NameText", "TypeText" };
            string[] textValues = { collectionName, collectionType };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                try
                {
                    var image = await ImportTexture(collectionBannerUri);
                    Sprite newSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new UnityEngine.Vector2(0.5f, 0.5f));
                    var imageObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find("Image").GetComponent<Image>();
                    imageObj.sprite = newSprite;
                }
                catch (Exception e)
                {
                    Debug.Log($"Error getting image {e}");
                }
                var buttonObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find("Image").GetComponent<Button>();
                buttonObj.onClick.RemoveAllListeners();
                buttonObj.onClick.AddListener(() => OpenSelectedCollection(projectCollectionsObjectIndex, collectionType));
            }
        }

        /// <summary>
        /// Updates the Collection item display.
        /// </summary>
        /// <param name="collectionContract">Collection contract.</param>
        /// <param name="collectionObjectIndex">Collection object index position.</param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftSupply">Nft supply.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private async Task UpdateCollectionItemDisplay(string collectionContract, int collectionObjectIndex, string nftId, string nftType, string nftSupply, string nftUri)
        {
            //Debug.Log($"NFT TYPE: {nftType}");
            if (nftType == "")
            {
                Debug.Log($"NFTID: {nftId} from collection contract {collectionContract} has no type");
            }
            string[] textObjectNames = { "IdText", "TypeText", "SupplyText" };
            string[] textValues = { $"ID: {nftId}", nftType, $"Supply: {nftSupply}"};
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = CollectionItemPrefabs[collectionObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                try
                {
                    var image = await ImportTexture(nftUri);
                    Sprite newSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new UnityEngine.Vector2(0.5f, 0.5f));
                    var imageObj = CollectionItemPrefabs[collectionObjectIndex].transform.Find("Image").GetComponent<Image>();
                    imageObj.sprite = newSprite;
                }
                catch (Exception e)
                {
                    Debug.Log($"Error getting image: {e}");
                }
                var imageButtonObj = CollectionItemPrefabs[collectionObjectIndex].transform.Find("ListButton").GetComponent<Button>();
                imageButtonObj.onClick.RemoveAllListeners();
                imageButtonObj.onClick.AddListener(() => OpenSelectedNft(collectionContract, nftId, nftType));
            }
        }
        
        /// <summary>
        /// Opens selected nft & transmits data to listing class.
        /// </summary>
        private void OpenSelectedNft(string collectionContract, string tokenIdToList, string nftTypeToList)
        {
            var listNftToMarketplaceManagerTxArgs =
                 new EventManagerMarketplace.ListNftToMarketplaceTxEventArgs(collectionContract, null, tokenIdToList, null, nftTypeToList);
            EventManagerMarketplace.RaiseListNftToMarketplaceTxManager(listNftToMarketplaceManagerTxArgs);
            EventManagerMarketplace.RaiseToggleListNftToMarketplaceMenu();
        }
        
        /// <summary>
        /// Resets Collection display by destroying Collection prefabs.
        /// </summary>
        private void ResetProjectCollectionsPrefabsDisplay()
        {
            foreach (var prefab in projectCollectionsPrefabs)
            {
                if (prefab != null)
                {
                    Destroy(prefab);
                }
            }
            Array.Clear(projectCollectionsPrefabs, 0, projectCollectionsPrefabs.Length);
            projectCollectionsObjectNumber = 0;
        }
        
        /// <summary>
        /// Resets Collection display by destroying item prefabs.
        /// </summary>
        private void ResetCollectionItemPrefabsDisplay()
        {
            foreach (var prefab in CollectionItemPrefabs)
            {
                if (prefab != null)
                {
                    Destroy(prefab);
                }
            }
            Array.Clear(CollectionItemPrefabs, 0, CollectionItemPrefabs.Length);
            CollectionItemsObjectNumber = 0;
        }
        
        /// <summary>
        /// Toggles selected collection.
        /// </summary>
        private void CloseSelectedCollection()
        {
            ResetCollectionItemPrefabsDisplay();
            ResetProjectCollectionsPrefabsDisplay();
            GetProjectCollections();
            EventManagerMarketplace.RaiseToggleProcessingMenu();
        }

        /// <summary>
        /// Opens selected collection.
        /// </summary>
        /// <param name="collectionIndex">Collection index to open.</param>
        /// <param name="collectionType">Collection type to open.</param>
        private void OpenSelectedCollection(int collectionIndex, string collectionType)
        {
            EventManagerMarketplace.RaiseOpenSelectedCollection();
            ResetProjectCollectionsPrefabsDisplay();
            PopulateCollectionItems(collectionIndex, collectionType);
        }
        
        /// <summary>
        /// Closes Collection menus and resets displays for performance.
        /// </summary>
        private void CloseCollection()
        {
            ResetProjectCollectionsPrefabsDisplay();
            ResetCollectionItemPrefabsDisplay();
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.ConfigureCollectionBrowserManager += OnConfigureCollectionBrowserManager;
            EventManagerMarketplace.ToggleCollectionsMenu += GetProjectCollections;
            EventManagerMarketplace.CloseSelectedCollection += CloseSelectedCollection;
            EventManagerMarketplace.ToggleSelectionMenu += CloseCollection;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureCollectionBrowserManager -= OnConfigureCollectionBrowserManager;
            EventManagerMarketplace.ToggleCollectionsMenu -= GetProjectCollections;
            EventManagerMarketplace.CloseSelectedCollection -= CloseSelectedCollection;
            EventManagerMarketplace.ToggleSelectionMenu -= CloseCollection;
        }
        
        /// <summary>
        /// Configures class properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConfigureCollectionBrowserManager(object sender, EventManagerMarketplace.CollectionBrowserConfigEventArgs args)
        {
            DisplayFont = args.DisplayFont;
            SecondaryTextColour = args.SecondaryTextColour;
            BearerToken = args.BearerToken;
        }

        #endregion
    }
}