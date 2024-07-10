using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Ipfs;
using ChainSafe.Gaming.Marketplace;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Scripts.EVM.Token;
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
            if (response.Collections.Count > 0)
            {
                PopulateCollections(response);
            }
        }

        /// <summary>
        /// Populates Collections and adds them to the display panel.
        /// </summary>
        private void PopulateCollections(UnityPackage.Model.NftTokenModel.ProjectCollectionsResponse collectionsResponse)
        {
            foreach (var collection in collectionsResponse.Collections)
            {
                AddCollectionToDisplay(collection.Name, collection.Type, collection.Banner);
            }
        }

        /// <summary>
        /// Populates items to be added to the Collection display.
        /// </summary>
        /// <param name="index">The index of the project to populate from.</param>
        /// <param name="collectionType">Collection type.</param>
        private async void PopulateCollectionItems(int index, string collectionType)
        {
            var projectResponse = await EvmMarketplace.GetProjectTokens();
            var collectionContract = projectResponse.Tokens[index].CollectionID;
            switch (collectionType)
            {
                case "erc721":
                {
                    var response = await EvmMarketplace.GetCollectionTokens721(projectResponse.Tokens[index].CollectionID);
                    foreach (var item in response.Tokens)
                    {
                        AddCollectionItemToDisplay(collectionContract, item.TokenID, item.TokenType, item.Supply, item.Uri);
                    }

                    break;
                }
                case "erc1155":
                {
                    var response = await EvmMarketplace.GetCollectionTokens1155(projectResponse.Tokens[index].CollectionID);
                    foreach (var item in response.Tokens)
                    {
                        AddCollectionItemToDisplay(collectionContract, item.TokenID, item.TokenType, item.Supply, item.Uri);
                    }
    
                    break;
                }
                default:
                    Debug.Log("No NFT type given, returning");
                    return;
            }
        }

        /// <summary>
        /// Adds Collection to the display panel.
        /// </summary>
        /// <param name="collectionName">Collection name to add.</param>
        /// <param name="collectionType">Collection type.</param>
        /// <param name="collectionBannerUri">Collection image uri to add.</param>
        private void AddCollectionToDisplay(string collectionName, string collectionType, string collectionBannerUri)
        {
            if (projectCollectionsObjectNumber >= projectCollectionsDisplayCount)
            {
                Destroy(projectCollectionsPrefabs[0]);
                for (int i = 1; i < projectCollectionsPrefabs.Length; i++)
                {
                    projectCollectionsPrefabs[i - 1] = projectCollectionsPrefabs[i];
                }
                projectCollectionsPrefabs[projectCollectionsPrefabs.Length - 1] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionType, collectionBannerUri);
            }
            else
            {
                projectCollectionsPrefabs[projectCollectionsObjectNumber] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionType, collectionBannerUri);
            }
            projectCollectionsObjectNumber++;
            CollectionScrollRect.horizontalNormalizedPosition = 0;
        }

        /// <summary>
        /// Adds items to the Collection display.
        /// </summary>
        /// <param name="collectionContract">Collection contract.</param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="supply">Nft supply.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private void AddCollectionItemToDisplay(string collectionContract, string nftId, string nftType, string supply, string nftUri)
        {
            if (CollectionItemsObjectNumber >= CollectionItemDisplayCount)
            {
                Destroy(CollectionItemPrefabs[0]);
                for (int i = 1; i < CollectionItemPrefabs.Length; i++)
                {
                    CollectionItemPrefabs[i - 1] = CollectionItemPrefabs[i];
                }
                CollectionItemPrefabs[CollectionItemPrefabs.Length - 1] = Instantiate(CollectionItemPrefab, CollectionPanel.transform);
                UpdateCollectionItemDisplay(collectionContract, CollectionItemsObjectNumber, nftId, nftType, supply, nftUri);
            }
            else
            {
                CollectionItemPrefabs[CollectionItemsObjectNumber] = Instantiate(CollectionItemPrefab, CollectionPanel.transform);
                UpdateCollectionItemDisplay(collectionContract, CollectionItemsObjectNumber, nftId, nftType, supply, nftUri);
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
            var metaRequest = UnityWebRequest.Get(uri);
            await metaRequest.SendWebRequest();
            
            if (metaRequest.result != UnityWebRequest.Result.Success)
            {
                throw new Web3Exception($"Metadata request failure: {metaRequest.error}");
            }
            var metadata = JsonConvert.DeserializeObject<Erc1155Metadata>(Encoding.UTF8.GetString(metaRequest.downloadHandler.data));
            var textureUri = IpfsHelper.RollupIpfsUri(metadata.image);
            var textureRequest = UnityWebRequestTexture.GetTexture(textureUri);
            await textureRequest.SendWebRequest();
            
            if (textureRequest.result != UnityWebRequest.Result.Success)
            {
                throw new Web3Exception($"Texture request failure: {metaRequest.error}");
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
        private async void UpdateProjectCollectionsDisplay(int projectCollectionsObjectIndex, string collectionName, string collectionType, string collectionBannerUri)
        {
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
                    var imageObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find("Image").GetComponent<Image>();
                    imageObj.material.mainTexture = image;
                }
                catch (Exception e)
                {
                    Debug.Log($"Error getting image: {e}");
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
        private async void UpdateCollectionItemDisplay(string collectionContract, int collectionObjectIndex, string nftId, string nftType, string nftSupply, string nftUri)
        {
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
                    var imageObj = CollectionItemPrefabs[collectionObjectIndex].transform.Find("Image").GetComponent<Image>();
                    imageObj.material.mainTexture = image;
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
            ResetProjectCollectionsPrefabsDisplay();
            ResetCollectionItemPrefabsDisplay();
            GetProjectCollections();
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