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

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the marketplace browse GUI.
    /// </summary>
    public class BrowseMarketplaceManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject marketplaceItemPrefab;
        [SerializeField] private GameObject marketplacePanel;
        [SerializeField] private GameObject selectMarketplaceMenu;
        [SerializeField] private GameObject browseMarketplaceMenu;
        [SerializeField] private Button openSelectMarketplaceOptionButton;
        [SerializeField] private TMP_Dropdown marketplaceDropDown;
        [SerializeField] private ScrollRect marketplaceScrollRect;
        private List<ApiResponse.Project> projects;
        private GameObject[] marketplaceItemPrefabs;
        private int marketplaceObjectNumber = 1;
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
            openSelectMarketplaceOptionButton.onClick.AddListener(OpenSelectMarketplaceOptionMenu);
            marketplaceItemPrefabs = new GameObject[marketplaceItemDisplayCount];
        }

        /// <summary>
        /// Populates the marketplace drop down options.
        /// </summary>
        private async void GetMarketplaceOptions()
        {
            UnityWebRequest request = UnityWebRequest.Get("https://api.gaming.chainsafe.io/project/getByAccountID");
            request.SetRequestHeader("Authorization", $"Bearer {BearerToken}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);
                projects = apiResponse.response.projects;
                List<string> options = new List<string>();
                foreach (var project in projects)
                {
                    options.Add(project.name);
                }

                marketplaceDropDown.ClearOptions();
                marketplaceDropDown.AddOptions(options);
                marketplaceDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
            }

            if (marketplaceDropDown.options != null)
            {
                PopulateMarketplaceItems(0);
            }
        }

        /// <summary>
        /// Populates items to be added to the marketplace display.
        /// </summary>
        /// <param name="index">The index of the project to populate from.</param>
        private async void PopulateMarketplaceItems(int index)
        {
            var projectResponse = await EvmMarketplace.GetProjectItems();
            var response = await EvmMarketplace.GetMarketplaceItems(projectResponse.Items[index].MarketplaceID);
            foreach (var item in response.Items)
            {
                AddMarketplaceItemToDisplay(item.Id, item.Token.TokenType, item.Price, item.Token.Uri);
            }
        }

        /// <summary>
        /// Adds items to the marketplace display.
        /// </summary>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftPrice">Nft price.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private void AddMarketplaceItemToDisplay(string nftId, string nftType, string nftPrice, string nftUri)
        {
            if (marketplaceObjectNumber >= marketplaceItemDisplayCount)
            {
                Destroy(marketplaceItemPrefabs[0]);
                for (int i = 1; i < marketplaceItemPrefabs.Length; i++)
                {
                    marketplaceItemPrefabs[i - 1] = marketplaceItemPrefabs[i];
                }
                marketplaceItemPrefabs[marketplaceItemPrefabs.Length - 1] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                UpdateMarketplaceDisplay(marketplaceObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            else
            {
                marketplaceItemPrefabs[marketplaceObjectNumber] = Instantiate(marketplaceItemPrefab, marketplacePanel.transform);
                UpdateMarketplaceDisplay(marketplaceObjectNumber, nftId, nftType, nftPrice, nftUri);
            }
            marketplaceObjectNumber++;
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
                throw new Web3Exception($"Texture request failure: {metaRequest.error}");
            }

            var texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;

            return texture;
        }

        /// <summary>
        /// Updates the marketplace display.
        /// </summary>
        /// <param name="marketplaceObjectIndex"></param>
        /// <param name="nftId">Nft id.</param>
        /// <param name="nftType">Nft name.</param>
        /// <param name="nftPrice">Nft price.</param>
        /// <param name="nftUri">Nft Uri.</param>
        private async void UpdateMarketplaceDisplay(int marketplaceObjectIndex, string nftId, string nftType, string nftPrice, string nftUri)
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
                var image = await ImportTexture(nftUri);
                var imageObj = marketplaceItemPrefabs[marketplaceObjectIndex].transform.Find("Image").GetComponent<Image>();
                imageObj.material.mainTexture = image;
            }
        }

        /// <summary>
        /// Called when the dropdown value is changed.
        /// </summary>
        /// <param name="index">The index of the selected option.</param>
        private void OnDropdownValueChanged(int index)
        {
            ResetMarketplacePrefabDisplay(index);
        }

        /// <summary>
        /// Resets marketplace display by destroying item prefabs.
        /// </summary>
        /// <param name="index">The index to populate.</param>
        private void ResetMarketplacePrefabDisplay(int? index = null)
        {
            foreach (var prefab in marketplaceItemPrefabs)
            {
                if (prefab != null)
                {
                    Destroy(prefab);
                }
            }
            Array.Clear(marketplaceItemPrefabs, 0, marketplaceItemPrefabs.Length);
            marketplaceObjectNumber = 0;
            if (!index.HasValue) return;
            PopulateMarketplaceItems(index.Value);
        }

        /// <summary>
        /// Opens the select marketplace option menu.
        /// </summary>
        private void OpenSelectMarketplaceOptionMenu()
        {
            browseMarketplaceMenu.SetActive(false);
            selectMarketplaceMenu.SetActive(true);
        }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.ConfigureMarketplaceBrowserManager += OnConfigureMarketPlaceBrowseManager;
            GetMarketplaceOptions();
        }

        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureMarketplaceBrowserManager -= OnConfigureMarketPlaceBrowseManager;
            ResetMarketplacePrefabDisplay();
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