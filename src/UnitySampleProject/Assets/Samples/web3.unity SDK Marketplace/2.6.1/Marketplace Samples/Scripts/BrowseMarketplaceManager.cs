using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Ipfs;
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
        private int marketplaceitemObjectNumber = 1;
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
        /// Adds marketplace to the display panel.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract the item belongs to.</param>
        /// <param name="marketplaceName">Marketplace name to add.</param>
        /// <param name="marketplaceBannerUri">Marketplace image uri to add.</param>
        private void AddMarketplaceToDisplay(string marketplaceContract, string marketplaceName, string marketplaceBannerUri)
        {
            if (projectMarketplacesObjectNumber >= projectMarketplacesDisplayCount)
            {
                Destroy(projectMarketplacesPrefabs[0]);
                for (int i = 1; i < projectMarketplacesPrefabs.Length; i++)
                {
                    projectMarketplacesPrefabs[i - 1] = projectMarketplacesPrefabs[i];
                }
                projectMarketplacesPrefabs[projectMarketplacesPrefabs.Length - 1] = Instantiate(projectMarketplacesPrefab, marketplacePanel.transform);
                UpdateProjectMarketplacesDisplay(marketplaceContract, projectMarketplacesObjectNumber, marketplaceName, marketplaceBannerUri);
            }
            else
            {
                projectMarketplacesPrefabs[projectMarketplacesObjectNumber] = Instantiate(projectMarketplacesPrefab, marketplacePanel.transform);
                UpdateProjectMarketplacesDisplay(marketplaceContract, projectMarketplacesObjectNumber, marketplaceName, marketplaceBannerUri);
            }
            projectMarketplacesObjectNumber++;
            marketplaceScrollRect.horizontalNormalizedPosition = 0;
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
            if (marketplaceitemObjectNumber >= marketplaceItemDisplayCount)
            {
                Destroy(marketplaceItemPrefabs[0]);
                for (int i = 1; i < marketplaceItemPrefabs.Length; i++)
                {
                    marketplaceItemPrefabs[i - 1] = marketplaceItemPrefabs[i];
                }
                marketplaceItemPrefabs[marketplaceItemPrefabs.Length - 1] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                UpdateMarketplaceItemDisplay(marketplaceContract, marketplaceitemObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            else
            {
                marketplaceItemPrefabs[marketplaceitemObjectNumber] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                UpdateMarketplaceItemDisplay(marketplaceContract, marketplaceitemObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            marketplaceitemObjectNumber++;
            marketplaceScrollRect.horizontalNormalizedPosition = 0;
        }
        
        /// <summary>
        /// Imports texture (can probably be removed later for helper class)
        /// </summary>
        /// <param name="uri">Nft uri</param>
        private async Task<Texture2D> ImportTexture(string uri)
        {
            var textureUri = IpfsHelper.RollupIpfsUri(uri);
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
        private void UpdateProjectMarketplacesDisplay(string marketplaceContract, int projectMarketplacesObjectIndex, string marketplaceName, string marketplaceBannerUri)
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
                // var image = await ImportTexture(marketplaceBannerUri);
                // var imageObj = projectMarketplacesPrefabs[projectMarketplacesObjectIndex].transform.Find("Image").GetComponent<Image>();
                // imageObj.material.mainTexture = image;
                var buttonObj = projectMarketplacesPrefabs[projectMarketplacesObjectIndex].transform.Find("Image").GetComponent<Button>();
                buttonObj.onClick.AddListener(() => OpenSelectedMarketplace(marketplaceContract, projectMarketplacesObjectIndex));
            }
        }

        /// <summary>
        /// Updates the marketplace item display.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract to call.</param>
        /// <param name="marketplaceObjectIndex">Index of the marketplace.</param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftPrice">Nft price.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private void UpdateMarketplaceItemDisplay(string marketplaceContract, int marketplaceObjectIndex, string nftId, string nftType, string nftPrice, string nftUri)
        {
            string[] textObjectNames = { "IdText", "TypeText", "PriceText" };
            string[] textValues = { nftId, nftType, nftPrice };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = marketplaceItemPrefabs[marketplaceObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                // var image = await ImportTexture(nftUri);
                // var imageObj = marketplaceItemPrefabs[marketplaceObjectIndex].transform.Find("Image").GetComponent<Image>();
                // imageObj.material.mainTexture = image;
                var imageButtonObj = projectMarketplacesPrefabs[marketplaceObjectIndex].transform.Find("Image").GetComponent<Button>();
                imageButtonObj.onClick.AddListener(() => OpenSelectedNft(marketplaceContract, marketplaceObjectIndex.ToString(), nftPrice, nftType));
                var buttonObj = projectMarketplacesPrefabs[marketplaceObjectIndex].transform.Find("Button").GetComponent<Button>();
                buttonObj.onClick.AddListener(() => PurchaseNft(marketplaceContract ,marketplaceObjectIndex.ToString(), nftPrice));
            }
        }
        
        /// <summary>
        /// Sends event to listing manager to populate properties.
        /// </summary>
        /// <param name="collectionContractToList">Collection contract to list from.</param>
        /// <param name="marketplaceContract">Collection contract to list to.</param>
        /// <param name="tokenIdToList">Collection Token Id.</param>
        /// <param name="priceToList">Price to list.</param>
        /// <param name="nftTypeToList">NftType to list.</param>
        private void OpenSelectedNft(string marketplaceContract, string tokenIdToList, string priceToList, string nftTypeToList)
        {
            var listNftToMarketplaceManagerConfigArgs =
                new EventManagerMarketplace.ListNftToMarketplaceConfigEventArgs(null, marketplaceContract, tokenIdToList, priceToList, nftTypeToList);
            EventManagerMarketplace.RaiseListNftToMarketplaceManager(listNftToMarketplaceManagerConfigArgs);
        }

        /// <summary>
        /// Purchases a marketplace Nft.
        /// </summary>
        /// <param name="marketplaceContract">The marketplace contract to purchase from.</param>
        /// <param name="marketplaceObjectIndex">Index of the Nft.</param>
        /// <param name="price">Nft price.</param>
        private async void PurchaseNft(string marketplaceContract, string marketplaceObjectIndex, string price)
        {
            Debug.Log($"Marketplace contract: {marketplaceContract}");
            await EvmMarketplace.PurchaseNft(marketplaceContract, marketplaceObjectIndex, price);
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
            marketplaceitemObjectNumber = 0;
            if (!index.HasValue) return;
            PopulateMarketplaceItems(null, index.Value);
        }
        
        /// <summary>
        /// Toggles selected marketplace.
        /// </summary>
        private void ToggleSelectedMarketplace()
        {
            ResetProjectMarketplacesPrefabsDisplay();
            ResetMarketplaceItemPrefabsDisplay();
        }

        /// <summary>
        /// Opens selected marketplace.
        /// </summary>
        /// <param name="marketplaceContract">Marketpalce contract.</param>
        /// <param name="marketplaceIndex">Index of the marketplace to open.</param>
        private void OpenSelectedMarketplace(string marketplaceContract, int marketplaceIndex)
        {
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
            EventManagerMarketplace.ToggleSelectedMarketplace += ToggleSelectedMarketplace;
            EventManagerMarketplace.ToggleSelectionMenu += CloseMarketplace;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureMarketplaceBrowserManager -= OnConfigureMarketPlaceBrowseManager;
            EventManagerMarketplace.ToggleMarketplacesMenu -= GetProjectMarketplaces;
            EventManagerMarketplace.ToggleSelectedMarketplace -= ToggleSelectedMarketplace;
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