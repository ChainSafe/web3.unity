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
using ChainSafe.Gaming.UnityPackage.Model;

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
            Debug.Log("GetProjectCollections");
            var response = await EvmMarketplace.GetProjectCollections(BearerToken);
            if (response.Collections.Count > 0)
            {
                PopulateCollections(response);
            }
        }

        /// <summary>
        /// Populates Collections and adds them to the display panel.
        /// </summary>
        private void PopulateCollections(NftTokenModel.ProjectCollectionsResponse collectionsResponse)
        {
            Debug.Log("PopulateCollections");
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
            Debug.Log("PopulateCollectionItems");
            var projectResponse = await EvmMarketplace.GetProjectTokens();
            Debug.Log($"Collection Type: {collectionType}");
            switch (collectionType)
            {
                case "erc721":
                {
                    Debug.Log($"Populating 721");
                    var response = await EvmMarketplace.GetCollectionTokens721(projectResponse.Tokens[index].CollectionID);
                    foreach (var item in response.Tokens)
                    {
                        AddCollectionItemToDisplay(item.TokenID, item.TokenType, item.Supply, item.Uri);
                    }

                    break;
                }
                case "erc1155":
                {
                    Debug.Log($"Populating 1155");
                    var response = await EvmMarketplace.GetCollectionTokens1155(projectResponse.Tokens[index].CollectionID);
                    foreach (var item in response.Tokens)
                    {
                        AddCollectionItemToDisplay(item.TokenID, item.TokenType, item.Supply, item.Uri);
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
            Debug.Log("AddCollectionToDisplay");
            if (projectCollectionsObjectNumber >= projectCollectionsDisplayCount)
            {
                Debug.Log("Over Display Count");
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
                Debug.Log("Under Display Count");
                projectCollectionsPrefabs[projectCollectionsObjectNumber] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionType, collectionBannerUri);
            }
            projectCollectionsObjectNumber++;
            CollectionScrollRect.horizontalNormalizedPosition = 0;
        }
        
        /// <summary>
        /// Adds items to the Collection display.
        /// </summary>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="supply">Nft supply.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private void AddCollectionItemToDisplay(string nftId, string nftType, string supply, string nftUri)
        {
            Debug.Log("AddCollectionItemToDisplay");
            if (CollectionItemsObjectNumber >= CollectionItemDisplayCount)
            {
                Debug.Log("Over Display Count");
                Destroy(CollectionItemPrefabs[0]);
                for (int i = 1; i < CollectionItemPrefabs.Length; i++)
                {
                    CollectionItemPrefabs[i - 1] = CollectionItemPrefabs[i];
                }
                CollectionItemPrefabs[CollectionItemPrefabs.Length - 1] = Instantiate(CollectionItemPrefab, CollectionPanel.transform);
                UpdateCollectionItemDisplay(CollectionItemsObjectNumber, nftId, nftType, supply, nftUri);
            }
            else
            {
                Debug.Log("Under Display Count");
                CollectionItemPrefabs[CollectionItemsObjectNumber] = Instantiate(CollectionItemPrefab, CollectionPanel.transform);
                UpdateCollectionItemDisplay(CollectionItemsObjectNumber, nftId, nftType, supply, nftUri);
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
            Debug.Log("UpdateProjectCollectionsDisplay");
            string[] textObjectNames = { "NameText", "TypeText" };
            string[] textValues = { collectionName, $"Type: {collectionType}" };
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
                    throw;
                }
                var buttonObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find("Image").GetComponent<Button>();
                buttonObj.onClick.RemoveAllListeners();
                buttonObj.onClick.AddListener(() => OpenCollection(projectCollectionsObjectIndex, collectionType));
            }
        }

        /// <summary>
        /// Updates the Collection item display.
        /// </summary>
        /// <param name="collectionObjectIndex"></param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="supply">Nft supply.</param>
        /// <param name="listedStatus">Nft listed status.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private async void UpdateCollectionItemDisplay(int collectionObjectIndex, string nftId, string nftType, string supply, string nftUri)
        {
            Debug.Log("UpdateCollectionItemDisplay");
            string[] textObjectNames = { "IdText", "TypeText", "SupplyText" };
            string[] textValues = { $"ID: {nftId}", $"Type: {nftType}", $"Supply: {supply}"};
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
            }
        }
        
        /// <summary>
        /// Resets Collection display by destroying Collection prefabs.
        /// </summary>
        private void ResetProjectCollectionsPrefabsDisplay()
        {
            Debug.Log("ResetProjectCollectionsPrefabsDisplay");
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
            Debug.Log("ResetCollectionItemPrefabsDisplay");
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
        private void ToggleSelectedCollection()
        {
            Debug.Log("ToggleSelectedCollection");
            ResetCollectionItemPrefabsDisplay();
            ResetProjectCollectionsPrefabsDisplay();
        }

        /// <summary>
        /// Opens selected Collection.
        /// </summary>
        /// <param name="collectionIndex">Index of the Collection to open.</param>
        /// <param name="collectionType">Collection type.</param>
        private void OpenCollection(int collectionIndex, string collectionType)
        {
            Debug.Log("OpenCollection");
            ResetProjectCollectionsPrefabsDisplay();
            PopulateCollectionItems(collectionIndex, collectionType);
        }
        
        /// <summary>
        /// Closes Collection menus and resets displays for performance.
        /// </summary>
        private void CloseCollection()
        {
            Debug.Log("CloseCollection");
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
            EventManagerMarketplace.ToggleSelectionMenu += CloseCollection;
            EventManagerMarketplace.ToggleSelectedCollection += ToggleSelectedCollection;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureCollectionBrowserManager -= OnConfigureCollectionBrowserManager;
            EventManagerMarketplace.ToggleCollectionsMenu -= GetProjectCollections;
            EventManagerMarketplace.ToggleSelectionMenu -= CloseCollection;
            EventManagerMarketplace.ToggleSelectedCollection -= ToggleSelectedCollection;
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