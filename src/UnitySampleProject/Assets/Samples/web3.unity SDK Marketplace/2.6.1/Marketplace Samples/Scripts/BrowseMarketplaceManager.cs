using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Ipfs;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;
using ChainSafe.Gaming.Marketplace.Models;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the marketplace browse GUI.
    /// </summary>
    public class BrowseMarketplaceManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject projectMarketplacesPrefab;
        [SerializeField] private GameObject marketplaceItemPrefab;
        [SerializeField] private GameObject marketplacePanel;
        [SerializeField] private ScrollRect marketplaceScrollRect;
        private List<ApiResponse.Project> projects;
        private GameObject[] projectMarketplacesPrefabs;
        private int projectMarketplacesObjectNumber = 1;
        private int projectMarketplacesDisplayCount = 100;
        private GameObject[] marketplaceItemPrefabs;
        private int marketplaceItemObjectNumber = 1;
        private int marketplaceItemDisplayCount = 100;

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
            projectMarketplacesPrefabs = new GameObject[projectMarketplacesDisplayCount];
            marketplaceItemPrefabs = new GameObject[marketplaceItemDisplayCount];
        }

        /// <summary>
        /// Populates the marketplace drop down options.
        /// </summary>
        private async void GetProjectMarketplaces()
        {
            var response = await EvmMarketplace.GetProjectMarketplaces(BearerToken);
            if (response.Marketplaces.Count > 0)
            {
                PopulateMarketplaces(response);
            }
        }

        /// <summary>
        /// Populates marketplaces and adds them to the display panel.
        /// </summary>
        private void PopulateMarketplaces(MarketplaceModel.ProjectMarketplacesResponse marketplacesResponse)
        {
            foreach (var marketplace in marketplacesResponse.Marketplaces)
            {
                AddMarketplaceToDisplay(marketplace.ContractAddress, marketplace.Name, marketplace.Banner);
            }
        }

        /// <summary>
        /// Populates items to be added to the marketplace display.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract to call.</param>
        /// <param name="index">The index of the project to populate from.</param>
        private async void PopulateMarketplaceItems(string marketplaceContract, int index)
        {
            var projectResponse = await EvmMarketplace.GetProjectItems();
            var response = await EvmMarketplace.GetMarketplaceItems(projectResponse.Items[index].MarketplaceID);
            foreach (var item in response.Items)
            {
                AddMarketplaceItemToDisplay(marketplaceContract, item.Id, item.Token.TokenType, item.Price, item.Token.Uri);
            }
        }

        /// <summary>
        /// Adds items to the marketplace display.
        /// </summary>
        /// /// <param name="marketplaceContract">Marketplace contract.</param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftPrice">Nft price.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private void AddMarketplaceItemToDisplay(string marketplaceContract, string nftId, string nftType, string nftPrice, string nftUri)
        {
            if (marketplaceItemObjectNumber >= marketplaceItemDisplayCount)
            {
                Destroy(marketplaceItemPrefabs[0]);
                for (int i = 1; i < marketplaceItemPrefabs.Length; i++)
                {
                    marketplaceItemPrefabs[i - 1] = marketplaceItemPrefabs[i];
                }
                marketplaceItemPrefabs[marketplaceItemPrefabs.Length - 1] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                UpdateMarketplaceItemDisplay(marketplaceContract, marketplaceItemObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            else
            {
                marketplaceItemPrefabs[marketplaceItemObjectNumber] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                UpdateMarketplaceItemDisplay(marketplaceContract, marketplaceItemObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            marketplaceItemObjectNumber++;
            marketplaceScrollRect.horizontalNormalizedPosition = 0;
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
                throw new Web3Exception($"Texture request failure: {textureRequest.error}");
            }

            var texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;

            return texture;
        }

        /// <summary>
        /// Updates the marketplaces display.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract.</param>
        /// <param name="projectMarketplacesObjectIndex">Index of marketplace.</param>
        /// <param name="marketplaceName">Marketplace name.</param>
        /// <param name="marketplaceBannerUri">Marketplace Uri.</param>
        private async void UpdateProjectMarketplacesDisplay(string marketplaceContract, int projectMarketplacesObjectIndex, string marketplaceName, string marketplaceBannerUri)
        {
            string[] textObjectNames = { "NameText" };
            string[] textValues = { marketplaceName };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = projectMarketplacesPrefabs[projectMarketplacesObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                try
                {
                    var image = await ImportTexture(marketplaceBannerUri);
                    var imageObj = projectMarketplacesPrefabs[projectMarketplacesObjectIndex].transform.Find("Image").GetComponent<Image>();
                    imageObj.material.mainTexture = image;
                }
                catch (Exception e)
                {
                    Debug.Log($"Error getting image: {e}");
                }
                var buttonObj = projectMarketplacesPrefabs[projectMarketplacesObjectIndex].transform.Find("Image").GetComponent<Button>();
                buttonObj.onClick.RemoveAllListeners();
                buttonObj.onClick.AddListener(() => OpenSelectedMarketplace(marketplaceContract, projectMarketplacesObjectIndex));
            }
        }

        /// <summary>
        /// Resets marketplace display by destroying marketplace prefabs.
        /// </summary>
        /// <param name="index">The index to populate.</param>
        private void ResetProjectMarketplacesPrefabsDisplay(int? index = null)
        {
            foreach (var prefab in projectMarketplacesPrefabs)
            {
                if (prefab != null)
                {
                    Destroy(prefab);
                }
            }
            Array.Clear(projectMarketplacesPrefabs, 0, projectMarketplacesPrefabs.Length);
            projectMarketplacesObjectNumber = 0;
            if (!index.HasValue) return;
            GetProjectMarketplaces();
        }

        /// <summary>
        /// Resets marketplace display by destroying item prefabs.
        /// </summary>
        /// <param name="index">The index to populate.</param>
        private void ResetMarketplaceItemPrefabsDisplay(int? index = null)
        {
            foreach (var prefab in marketplaceItemPrefabs)
            {
                if (prefab != null)
                {
                    Destroy(prefab);
                }
            }
            Array.Clear(marketplaceItemPrefabs, 0, marketplaceItemPrefabs.Length);
            marketplaceItemObjectNumber = 0;
            if (!index.HasValue) return;
            PopulateMarketplaceItems(null, index.Value);
        }
        
        /// <summary>
        /// Toggles selected marketplace.
        /// </summary>
        private void CloseSelectedMarketplace()
        {
            ResetProjectMarketplacesPrefabsDisplay(0);
            ResetMarketplaceItemPrefabsDisplay();
        }

        /// <summary>
        /// Opens selected marketplace.
        /// </summary>
        /// <param name="marketplaceContract">Marketpalce contract.</param>
        /// <param name="marketplaceIndex">Index of the marketplace to open.</param>
        private void OpenSelectedMarketplace(string marketplaceContract, int marketplaceIndex)
        {
            EventManagerMarketplace.RaiseOpenSelectedMarketplace();
            ResetProjectMarketplacesPrefabsDisplay();
            PopulateMarketplaceItems(marketplaceContract, marketplaceIndex);
        }
        
        /// <summary>
        /// Resets displays for performance on menu closed.
        /// </summary>
        private void CloseMarketplace()
        {
            ResetProjectMarketplacesPrefabsDisplay();
            ResetMarketplaceItemPrefabsDisplay();
        }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.ConfigureMarketplaceBrowserManager += OnConfigureMarketPlaceBrowseManager;
            EventManagerMarketplace.ToggleMarketplacesMenu += GetProjectMarketplaces;
            EventManagerMarketplace.CloseSelectedMarketplace += CloseSelectedMarketplace;
            EventManagerMarketplace.ToggleSelectionMenu += CloseMarketplace;
        }

        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureMarketplaceBrowserManager -= OnConfigureMarketPlaceBrowseManager;
            EventManagerMarketplace.ToggleMarketplacesMenu -= GetProjectMarketplaces;
            EventManagerMarketplace.CloseSelectedMarketplace -= CloseSelectedMarketplace;
            EventManagerMarketplace.ToggleSelectionMenu -= CloseMarketplace;
        }

        /// <summary>
        /// Configures class properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConfigureMarketPlaceBrowseManager(object sender, EventManagerMarketplace.MarketplaceBrowserConfigEventArgs args)
        {
            DisplayFont = args.DisplayFont;
            SecondaryTextColour = args.SecondaryTextColour;
            BearerToken = args.BearerToken;
        }

        #endregion
    }
}