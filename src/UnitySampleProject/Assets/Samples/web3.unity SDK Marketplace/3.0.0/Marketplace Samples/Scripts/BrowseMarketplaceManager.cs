using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;
using ChainSafe.Gaming.Marketplace.Models;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json.Linq;

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
        private string baseUrl = "https://ipfs.chainsafe.io/ipfs/";
        private Dictionary<string, Sprite> _cachedSprites = new ();


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
        private async void PopulateMarketplaces(UnityPackage.Model.MarketplaceModel.ProjectMarketplacesResponse marketplacesResponse)
        {
            foreach (var marketplace in marketplacesResponse.Marketplaces)
            {
                await AddMarketplaceToDisplay(marketplace.contract_address, marketplace.name, baseUrl + marketplace.banner);
            }
            EventManagerMarketplace.RaiseToggleProcessingMenu();
        }

        /// <summary>
        /// Populates items to be added to the marketplace display.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract to call.</param>
        /// <param name="index">The index of the project to populate from.</param>
        private async void PopulateMarketplaceItems(string marketplaceContract, int index)
        {
            var projectResponse = await EvmMarketplace.GetProjectItems();
            if (index >= projectResponse.items.Count)
            {
                EventManagerMarketplace.RaiseToggleProcessingMenu();
                return;
            }
            var response = await EvmMarketplace.GetMarketplaceItems(projectResponse.items[index].marketplace_id);
            Dictionary<string, JObject> _cache = new();
            foreach (var item in response.items)
            {
                if (item.status == "listed")
                {
                    if (!_cache.TryGetValue(item.token.uri, out var json))
                    {
                        var uwr = UnityWebRequest.Get(item.token.uri);
                        await uwr.SendWebRequest();
                        json = JObject.Parse(uwr.downloadHandler.text);
                        _cache[item.token.uri] = json;
                    }

                    string imageUrl = json.TryGetValue("image", out var jToken) ? jToken.ToString() :  string.Empty;
                        

                    await AddMarketplaceItemToDisplay(marketplaceContract, item.id, item.token.token_type, item.price, imageUrl.ToString());
                }
            }
            EventManagerMarketplace.RaiseToggleProcessingMenu();
        }

        /// <summary>
        /// Adds marketplace to the display panel.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract the item belongs to.</param>
        /// <param name="marketplaceName">Marketplace name to add.</param>
        /// <param name="marketplaceBannerUri">Marketplace image uri to add.</param>
        private async Task AddMarketplaceToDisplay(string marketplaceContract, string marketplaceName, string marketplaceBannerUri)
        {
            if (projectMarketplacesObjectNumber >= projectMarketplacesDisplayCount)
            {
                Destroy(projectMarketplacesPrefabs[0]);
                for (int i = 1; i < projectMarketplacesPrefabs.Length; i++)
                {
                    projectMarketplacesPrefabs[i - 1] = projectMarketplacesPrefabs[i];
                }
                projectMarketplacesPrefabs[projectMarketplacesPrefabs.Length - 1] = Instantiate(projectMarketplacesPrefab, marketplacePanel.transform);
                await UpdateProjectMarketplacesDisplay(marketplaceContract, projectMarketplacesObjectNumber, marketplaceName, marketplaceBannerUri);
            }
            else
            {
                projectMarketplacesPrefabs[projectMarketplacesObjectNumber] = Instantiate(projectMarketplacesPrefab, marketplacePanel.transform);
                await UpdateProjectMarketplacesDisplay(marketplaceContract, projectMarketplacesObjectNumber, marketplaceName, marketplaceBannerUri);
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
        private async Task AddMarketplaceItemToDisplay(string marketplaceContract, string nftId, string nftType, string nftPrice, string nftUri)
        {
            if (marketplaceItemObjectNumber >= marketplaceItemDisplayCount)
            {
                Destroy(marketplaceItemPrefabs[0]);
                for (int i = 1; i < marketplaceItemPrefabs.Length; i++)
                {
                    marketplaceItemPrefabs[i - 1] = marketplaceItemPrefabs[i];
                }
                marketplaceItemPrefabs[marketplaceItemPrefabs.Length - 1] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                await UpdateMarketplaceItemDisplay(marketplaceContract, marketplaceItemObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            else
            {
                marketplaceItemPrefabs[marketplaceItemObjectNumber] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                await UpdateMarketplaceItemDisplay(marketplaceContract, marketplaceItemObjectNumber, nftId, nftType, nftPrice, nftUri);
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
        /// Updates the marketplaces display.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract.</param>
        /// <param name="projectMarketplacesObjectIndex">Index of marketplace.</param>
        /// <param name="marketplaceName">Marketplace name.</param>
        /// <param name="marketplaceBannerUri">Marketplace Uri.</param>
        private async Task UpdateProjectMarketplacesDisplay(string marketplaceContract, int projectMarketplacesObjectIndex, string marketplaceName, string marketplaceBannerUri)
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
                    Sprite newSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new UnityEngine.Vector2(0.5f, 0.5f));
                    var imageObj = projectMarketplacesPrefabs[projectMarketplacesObjectIndex].transform.Find("Image").GetComponent<Image>();
                    imageObj.sprite = newSprite;
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
        /// Updates the marketplace item display.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract to call.</param>
        /// <param name="marketplaceItemObjectIndex">Index of the marketplace.</param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftPrice">Nft price.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private async Task UpdateMarketplaceItemDisplay(string marketplaceContract, int marketplaceItemObjectIndex, string nftId, string nftType, string nftPrice, string nftUri)
        {
            var ethValue = (decimal)BigInteger.Parse(nftPrice) / (decimal)BigInteger.Pow(10, 18);
            string formattedEthValue = ethValue.ToString("0.##################");
            string[] textObjectNames = { "IdText", "TypeText", "PriceText" };
            string[] textValues = { $"ID: {nftId}", $"{nftType}", $"{formattedEthValue} {Web3Unity.Web3.ChainConfig.Symbol.ToUpper()}" };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = marketplaceItemPrefabs[marketplaceItemObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                try
                {
                    Sprite sprite = null;
                    if (!string.IsNullOrEmpty(nftUri) && !_cachedSprites.TryGetValue(nftUri, out sprite))
                    {
                        var image = await ImportTexture(nftUri);
                        sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height),
                            new UnityEngine.Vector2(0.5f, 0.5f));
                        _cachedSprites.TryAdd(nftUri, sprite);
                        Debug.LogError(nftUri);
                    }
                    
                    var imageObj = marketplaceItemPrefabs[marketplaceItemObjectIndex].transform.Find("Image").GetComponent<Image>();
                    if(sprite != null)
                        imageObj.sprite = sprite;
                }
                catch (Exception e)
                {
                    Debug.Log($"Error getting image: {e}");
                    _cachedSprites.TryAdd(nftUri, null);
                }
                var buttonObj = marketplaceItemPrefabs[marketplaceItemObjectIndex].transform.Find("PurchaseButton").GetComponent<Button>();
                buttonObj.onClick.RemoveAllListeners();
                buttonObj.onClick.AddListener(() => PurchaseNft(marketplaceContract ,nftId, nftPrice));
            }
        }

        /// <summary>
        /// Purchases a marketplace Nft.
        /// </summary>
        /// <param name="marketplaceContract">The marketplace contract to purchase from.</param>
        /// <param name="marketplaceObjectIndex">Index of the Nft.</param>
        /// <param name="price">Nft price.</param>
        private async void PurchaseNft(string marketplaceContract, string marketplaceObjectIndex, string price)
        {
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
            EventManagerMarketplace.RaiseToggleProcessingMenu();
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