using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using ChainSafe.Gaming.Marketplace.Models;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the marketplace login system.
    /// </summary>
    public class LoginManagerMarketplace : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private TMP_InputField emailAddressInput;
        [SerializeField] private TMP_InputField authCodeInput;
        [SerializeField] private GameObject marketplacePrefab;
        [SerializeField] private GameObject emailLoginMenu;
        [SerializeField] private GameObject authCodeMenu;
        [SerializeField] private Button requestEmailButton;
        [SerializeField] private Button verifyAuthCodeButton;
        [SerializeField] private Button verifyAuthCodeBackButton;
        [SerializeField] public TMP_FontAsset displayFont;
        [SerializeField] private Color primaryBackgroundColour;
        [SerializeField] private Color menuBackgroundColour;
        [SerializeField] private Color primaryTextColour;
        [SerializeField] private Color secondaryTextColour;
        [SerializeField] private Color borderButtonColour;
        [SerializeField] private List<GameObject> primaryBackgroundObjects;
        [SerializeField] private List<GameObject> menuBackgroundObjects;
        [SerializeField] private List<GameObject> primaryTextObjects;
        [SerializeField] private List<GameObject> secondaryTextObjects;
        [SerializeField] private List<GameObject> displayLineObjects;
        [SerializeField] private List<GameObject> borderButtonObjects;

        #endregion

        #region Properties

        private string EmailAddress { get; set; }
        private string AuthCode { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes objects.
        /// </summary>
        private void Awake()
        {
            requestEmailButton.onClick.AddListener(RequestEmailAuthCode);
            verifyAuthCodeButton.onClick.AddListener(VerifyEmailAuthCode);
            verifyAuthCodeBackButton.onClick.AddListener(ToggleEmailMenu);
            SetCustomConfig();
        }
        
        /// <summary>
        /// Sets config & object colours.
        /// </summary>
        private void SetCustomConfig()
        {
            var marketplaceGuiConfigArgs = new EventManagerMarketplace.MarketplaceGUIConfigEventArgs(displayFont, primaryBackgroundColour, menuBackgroundColour, primaryTextColour, secondaryTextColour, borderButtonColour);
            EventManagerMarketplace.RaiseConfigureMarketplaceGuiManager(marketplaceGuiConfigArgs);
            CustomizationHelperMarketplace.SetCustomColours(
                displayFont,
                primaryBackgroundObjects, primaryBackgroundColour,
                menuBackgroundObjects, menuBackgroundColour,
                primaryTextObjects, primaryTextColour,
                secondaryTextObjects, secondaryTextColour,
                borderButtonObjects, borderButtonColour,
                displayLineObjects);
        }

        /// <summary>
        /// Logs into ChainSafe's dashboard via email.
        /// </summary>
        private async void RequestEmailAuthCode()
        {
            EmailAddress = emailAddressInput.text ?? throw new Exception("Email address not set");
            var payload = new AuthPayload.EmailRequestPayload
            {
                email = EmailAddress
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var request = new UnityWebRequest("https://api.chainsafe.io/api/v1/user/email", "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                ToggleEmailMenu();
            }
        }
        
        /// <summary>
        /// Authorizes login via email code.
        /// </summary>
        private async void VerifyEmailAuthCode()
        {
            AuthCode = authCodeInput.text ?? throw new Exception("Auth code not set");
            var payload = new AuthPayload.AuthCodePayload()
            {
                email = EmailAddress,
                nonce = AuthCode
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var request = new UnityWebRequest("https://api.chainsafe.io/api/v1/user/email/verify", "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
    
            await request.SendWebRequest();
    
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                AuthSystemResponse.AuthResponse authResponse = JsonConvert.DeserializeObject<AuthSystemResponse.AuthResponse>(jsonResponse);
                TryLogin(authResponse.token);
            }
        }
        
        /// <summary>
        /// Retrieves the user account ID.
        /// </summary>
        private async void TryLogin(string authResponseToken)
        {
            var payload = new AuthPayload.LoginPayload()
            {
                provider = "email",
                service = "gaming",
                token = authResponseToken
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var request = new UnityWebRequest("https://api.chainsafe.io/api/v1/user/login", "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
    
            await request.SendWebRequest();
    
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                AuthSystemResponse.LoginResponse loginResponse = JsonConvert.DeserializeObject<AuthSystemResponse.LoginResponse>(jsonResponse);
                InstantiateMarketplace();
                var authSystemManagerConfigArgs = new EventManagerMarketplace.MarketplaceAuthSystemConfigEventArgs(loginResponse.access_token.token, DateTime.Parse(loginResponse.access_token.expires), loginResponse.refresh_token.token, DateTime.Parse(loginResponse.refresh_token.expires));
                EventManagerMarketplace.RaiseConfigureAuthSystemManager(authSystemManagerConfigArgs);
                var marketplaceBrowserManagerConfigArgs = new EventManagerMarketplace.MarketplaceBrowserConfigEventArgs(displayFont, secondaryTextColour, loginResponse.access_token.token);
                EventManagerMarketplace.RaiseConfigureMarketplaceBrowserManager(marketplaceBrowserManagerConfigArgs);
                var collectionBrowserManagerConfigArgs = new EventManagerMarketplace.CollectionBrowserConfigEventArgs(displayFont, secondaryTextColour, loginResponse.access_token.token);
                EventManagerMarketplace.RaiseConfigureCollectionBrowserManager(collectionBrowserManagerConfigArgs);
                var marketplaceCreateConfigArgs = new EventManagerMarketplace.MarketplaceCreateConfigEventArgs(loginResponse.access_token.token);
                EventManagerMarketplace.RaiseConfigureMarketplaceCreateManager(marketplaceCreateConfigArgs);
                var collectionCreateConfigArgs = new EventManagerMarketplace.CollectionCreateConfigEventArgs(loginResponse.access_token.token);
                EventManagerMarketplace.RaiseConfigureCollectionCreateManager(collectionCreateConfigArgs);
                var listNftToMarketplaceCreateConfigArgs = new EventManagerMarketplace.ListNftToMarketplaceConfigEventArgs(loginResponse.access_token.token);
                EventManagerMarketplace.RaiseConfigureListNftToMarketplaceManager(listNftToMarketplaceCreateConfigArgs);
                var mintCollectionNftConfigArgs = new EventManagerMarketplace.MintCollectionNftConfigEventArgs(loginResponse.access_token.token, null, null);
                EventManagerMarketplace.RaiseMintCollectionNftManager(mintCollectionNftConfigArgs);
            }
        }
        
        /// <summary>
        /// Toggles the email & auth menus.
        /// </summary>
        private void ToggleEmailMenu()
        {
            emailLoginMenu.SetActive(!emailLoginMenu.activeSelf);
            authCodeMenu.SetActive(!authCodeMenu.activeSelf);
        }
        
        /// <summary>
        /// Instantiates the marketplace prefab.
        /// </summary>
        private void InstantiateMarketplace()
        {
            Instantiate(marketplacePrefab);
            Destroy(gameObject);
        }

        #endregion
    }
}