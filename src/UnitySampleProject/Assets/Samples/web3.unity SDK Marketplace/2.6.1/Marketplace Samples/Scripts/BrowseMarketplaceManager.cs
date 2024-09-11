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
        private readonly Dictionary<string, Sprite> _cachedSprites = new ();
        private readonly Dictionary<string, JObject> _nftUriCache = new ();

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
            
            ProcessingMenu.ToggleMenu();
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
                ProcessingMenu.ToggleMenu();
                return;
            }
            var response = await EvmMarketplace.GetMarketplaceItems(projectResponse.items[index].marketplace_id);
            foreach (var item in response.items)
            {
                if (item.status == "listed")
                {
                    if (!_nftUriCache.TryGetValue(item.token.uri, out var json))
                    {
                        var uwr = UnityWebRequest.Get(item.token.uri);
                        await uwr.SendWebRequest();
                        json = JObject.Parse(uwr.downloadHandler.text);
                        _nftUriCache[item.token.uri] = json;
                    }

                    string imageUrl = json.TryGetValue("image", out var jToken) ? jToken.ToString() :  string.Empty;
                        

                    await AddMarketplaceItemToDisplay(marketplaceContract, item.id, item.token.token_type, item.price, imageUrl);
                }
            }
            ProcessingMenu.ToggleMenu();
            
        }

        private Queue<GameObject> marketplacePrefabQueue = new Queue<GameObject>();

        /// <summary>
        /// Adds marketplace to the display panel.
        /// </summary>
        /// <param name="marketplaceContract">Marketplace contract the item belongs to.</param>
        /// <param name="marketplaceName">Marketplace name to add.</param>
        /// <param name="marketplaceBannerUri">Marketplace image URI to add.</param>
        private async Task AddMarketplaceToDisplay(string marketplaceContract, string marketplaceName, string marketplaceBannerUri)
        {
            // If the current number of displayed marketplaces reaches the maximum limit
            if (marketplacePrefabQueue.Count >= projectMarketplacesDisplayCount)
            {
                // Remove the oldest marketplace prefab from the queue and destroy it
                var oldestPrefab = marketplacePrefabQueue.Dequeue();
                Destroy(oldestPrefab);
            }

            // Create a new marketplace prefab and add it to the panel
            var newPrefab = Instantiate(projectMarketplacesPrefab, marketplacePanel.transform);

            // Add the new prefab to the queue
            marketplacePrefabQueue.Enqueue(newPrefab);

            // Update the new marketplace's display
            await UpdateProjectMarketplacesDisplay(marketplaceContract, newPrefab, marketplaceName, marketplaceBannerUri);

            // Ensure the scroll position is reset to the start
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
        /// <param name="marketplacePrefab">Prefab instance for this marketplace.</param>
        /// <param name="marketplaceName">Marketplace name.</param>
        /// <param name="marketplaceBannerUri">Marketplace URI.</param>
        private async Task UpdateProjectMarketplacesDisplay(string marketplaceContract, GameObject marketplacePrefab, string marketplaceName, string marketplaceBannerUri)
        {
            string[] textObjectNames = { "NameText" };
            string[] textValues = { marketplaceName };

            // Update the text for the marketplace prefab
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = marketplacePrefab.transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
            }

            // Set the marketplace banner image
            try
            {
                await SetSpriteFromUrl(marketplaceBannerUri, marketplacePrefab.transform.Find("Image").GetComponent<Image>());
            }
            catch (Exception e)
            {
                Debug.Log($"Error getting image: {e}");
            }

            // Configure the button to open the selected marketplace
            var buttonObj = marketplacePrefab.transform.Find("Image").GetComponent<Button>();
            buttonObj.onClick.RemoveAllListeners();
            buttonObj.onClick.AddListener(() => OpenSelectedMarketplace(marketplaceContract, marketplacePrefabQueue.Count - 1));
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
            string[] textValues = { $"ID: {nftId}", $"{nftType}", $"{formattedEthValue} {Web3Accessor.Web3.ChainConfig.Symbol.ToUpper()}" };
            for (int i = 0; i < textObjectNames.Length; i++)
            {
                var textObj = marketplaceItemPrefabs[marketplaceItemObjectIndex].transform.Find(textObjectNames[i]);
                var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = textValues[i];
                textMeshPro.font = DisplayFont;
                textMeshPro.color = SecondaryTextColour;
                try
                {
                    SetSpriteFromUrl(nftUri, marketplaceItemPrefabs[marketplaceItemObjectIndex].transform.Find("Image").GetComponent<Image>());
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

        private async Task SetSpriteFromUrl(string nftUri, Image image)
        {
            Sprite sprite = null;
            if (!string.IsNullOrEmpty(nftUri) && !_cachedSprites.TryGetValue(nftUri, out sprite))
            {
                var texture = await ImportTexture(nftUri);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new UnityEngine.Vector2(0.5f, 0.5f));
                _cachedSprites.TryAdd(nftUri, sprite);
            }
                    
            if(sprite != null)
                image.sprite = sprite;
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
            ProcessingMenu.ToggleMenu();
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