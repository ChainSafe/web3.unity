using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

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
        [Header("Enable this for testing to bypass auth")]
        [SerializeField] private bool test;

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
            if (test)
            {
                ToggleEmailMenu();
                return;
            }
            EmailAddress = emailAddressInput.text ?? throw new Exception("Email address not set");
            WWWForm form = new WWWForm();
            form.AddField("email", EmailAddress);
            UnityWebRequest request = UnityWebRequest.Post("https://api.chainsafe.io/api/v1/user/email", form);
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
            if (test)
            {
                InstantiateMarketplace();
                return;
            }
            AuthCode = authCodeInput.text ?? throw new Exception("Auth code not set");
            WWWForm form = new WWWForm();
            form.AddField("email", EmailAddress);
            form.AddField("nonce", AuthCode);
            UnityWebRequest request = UnityWebRequest.Post("https://api.chainsafe.io/api/v1/user/email/verify", form);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                InstantiateMarketplace();
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
