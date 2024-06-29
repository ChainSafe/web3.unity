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
using UnityEngine.Events;

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
        private void PopulateCollections(NftTokenModel.ProjectCollectionsResponse collectionsResponse)
        {
            foreach (var collection in collectionsResponse.Collections)
            {
                AddCollectionToDisplay(collection.Name, collection.Banner);
            }
        }
        
        /// <summary>
        /// Populates items to be added to the Collection display.
        /// </summary>
        /// <param name="index">The index of the project to populate from.</param>
        private async void PopulateCollectionItems(int index)
        {
            var projectResponse = await EvmMarketplace.GetProjectTokens();
            var response = await EvmMarketplace.GetCollectionTokens721(projectResponse.Tokens[index].CollectionID);
            foreach (var item in response.Tokens)
            {
                AddCollectionItemToDisplay(item.TokenID, item.TokenType, item.Supply, item.Uri);
            }
        }
        
        /// <summary>
        /// Adds Collection to the display panel.
        /// </summary>
        /// <param name="collectionName">Collection name to add.</param>
        /// <param name="collectionBannerUri">Collection image uri to add.</param>
        private void AddCollectionToDisplay(string collectionName, string collectionBannerUri)
        {
            if (projectCollectionsObjectNumber >= projectCollectionsDisplayCount)
            {
                Destroy(projectCollectionsPrefabs[0]);
                for (int i = 1; i < projectCollectionsPrefabs.Length; i++)
                {
                    projectCollectionsPrefabs[i - 1] = projectCollectionsPrefabs[i];
                }
                projectCollectionsPrefabs[projectCollectionsPrefabs.Length - 1] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionBannerUri);
            }
            else
            {
                projectCollectionsPrefabs[projectCollectionsObjectNumber] = Instantiate(projectCollectionsPrefab, CollectionPanel.transform);
                UpdateProjectCollectionsDisplay(projectCollectionsObjectNumber, collectionName, collectionBannerUri);
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
            if (CollectionItemsObjectNumber >= CollectionItemDisplayCount)
            {
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
        /// <param name="CollectionName">Collection name.</param>
        /// <param name="CollectionBannerUri">Collection Uri.</param>
        private async void UpdateProjectCollectionsDisplay(int projectCollectionsObjectIndex, string CollectionName, string CollectionBannerUri)
        {
            string[] textObjectNames = { "NameText"};
            string[] textValues = { CollectionName };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                var image = await ImportTexture(CollectionBannerUri);
                var imageObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find("Image").GetComponent<Image>();
                imageObj.material.mainTexture = image;
                var buttonObj = projectCollectionsPrefabs[projectCollectionsObjectIndex].transform.Find("Image").GetComponent<Button>();
                buttonObj.onClick.AddListener(() => OpenCollection(projectCollectionsObjectIndex));
            }
        }
        
        /// <summary>
        /// Updates the Collection item display.
        /// </summary>
        /// <param name="collectionObjectIndex"></param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftPrice">Nft price.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private async void UpdateCollectionItemDisplay(int collectionObjectIndex, string nftId, string nftType, string nftPrice, string nftUri)
        {
            string[] textObjectNames = { "IdText", "TypeText", "PriceText" };
            string[] textValues = { nftId, nftType, nftPrice };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = CollectionItemPrefabs[collectionObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                var image = await ImportTexture(nftUri);
                var imageObj = CollectionItemPrefabs[collectionObjectIndex].transform.Find("Image").GetComponent<Image>();
                imageObj.material.mainTexture = image;
            }
        }
        
        /// <summary>
        /// Resets Collection display by destroying Collection prefabs.
        /// </summary>
        /// <param name="index">The index to populate.</param>
        private void ResetProjectCollectionsPrefabsDisplay(int? index = null)
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
            if (!index.HasValue) return;
            GetProjectCollections();
        }
        
        /// <summary>
        /// Resets Collection display by destroying item prefabs.
        /// </summary>
        /// <param name="index">The index to populate.</param>
        private void ResetCollectionItemPrefabsDisplay(int? index = null)
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
            if (!index.HasValue) return;
            PopulateCollectionItems(index.Value);
        }
        
        /// <summary>
        /// Opens selected Collection.
        /// </summary>
        /// <param name="collectionIndex">Index of the Collection to open.</param>
        private void OpenCollection(int collectionIndex)
        {
            ResetProjectCollectionsPrefabsDisplay();
            PopulateCollectionItems(collectionIndex);
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
            EventManagerMarketplace.ToggleSelectionMenu += CloseCollection;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureCollectionBrowserManager -= OnConfigureCollectionBrowserManager;
            EventManagerMarketplace.ToggleCollectionsMenu -= GetProjectCollections;
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