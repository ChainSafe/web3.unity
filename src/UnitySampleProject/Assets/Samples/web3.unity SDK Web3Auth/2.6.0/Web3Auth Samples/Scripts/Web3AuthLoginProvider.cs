using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using AOT;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Web3Auth;
using Nethereum.Hex.HexTypes;
using Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Web3Auth.Network;

/// <summary>
/// Login using Web3Auth.
/// </summary>
public class Web3AuthLoginProvider : ConnectionHandler, IWeb3BuilderServiceAdapter
{
    /// <summary>
    /// Struct used for pairing login buttons to Web3 auth providers.
    /// Used when adding <see cref="Web3AuthLoginProvider.LoginWithWeb3Auth"/> as listeners to the buttons.
    /// </summary>
    [Serializable]
    public struct ProviderAndButtonPair
    {
        public Button Button;
        public Provider Provider;
    }

    [Header("Web3 Auth")]
    [SerializeField] private string clientId;
    [SerializeField] private string redirectUri;
    [SerializeField] private Network network;
    [Header("UI")]
    [SerializeField] private List<ProviderAndButtonPair> providerAndButtonPairs;

    [Header("Wallet GUI Options")]
    [SerializeField] private GameObject web3AuthWalletGUIPrefab;
    [SerializeField] private bool enableWalletGUI;
    [SerializeField] private bool displayWalletIcon;
    [SerializeField] private bool autoConfirmTransactions;
    [SerializeField] private bool autoPopUpWalletOnTx;
    [SerializeField] private Sprite walletIcon;
    [SerializeField] private Sprite walletLogo;
    [SerializeField] public TMP_FontAsset displayFont;
    [SerializeField] private Color primaryBackgroundColour;
    [SerializeField] private Color menuBackgroundColour;
    [SerializeField] private Color primaryTextColour;
    [SerializeField] private Color secondaryTextColour;
    [SerializeField] private Color borderButtonColour;
    private Provider? selectedProvider;
    private bool rememberMe;

// #if UNITY_WEBGL && !UNITY_EDITOR
//     [DllImport("__Internal")]
//     private static extern void InitWeb3Auth(string clientId, string chainId, string rpcTarget, string displayName, string blockExplorerUrl, string ticker, string tickerName, string network);
//     [DllImport("__Internal")]
//     private static extern void Web3AuthLogin(string provider, bool rememberMe);    
//     [DllImport("__Internal")]
//     private static extern void SetLoginCallback(Action<string> callback);
//     
//     public static event Action<string> Web3AuthWebGLConnected;
// #endif


    public void SetRememberMe(bool rememberMe)
    {
        this.rememberMe = rememberMe;
    }

    protected override async void Initialize()
    {
        base.Initialize();
        providerAndButtonPairs.ForEach(p =>
            p.Button.onClick.AddListener(delegate { LoginWithWeb3Auth(p.Provider); }));
// #if !UNITY_EDITOR && UNITY_WEBGL
//         Web3AuthWebGLConnected += Web3AuthSet;
//         var projectSettings = ProjectConfigUtilities.Load();
//         SetLoginCallback(Web3AuthConnected);
//         //1155 is a decimal number, we need to convert it to an integer
//         InitWeb3Auth(clientId, new HexBigInteger(BigInteger.Parse(projectSettings.ChainId)).HexValue, 
//         projectSettings.Rpc, projectSettings.Network, "", projectSettings.Symbol, "", network.ToString().ToLower());
// #else
        if (!string.IsNullOrEmpty(KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID)))
        {
            rememberMe = true;
            await TryConnect();
            Debug.Log("Restoring existing Web3Auth session (Remember Me");
        }
// #endif
    }
// #if !UNITY_EDITOR && UNITY_WEBGL
//     private async void Web3AuthSet(string sessionId)
//     {
//         Web3AuthWebGLConnected -= Web3AuthSet;
//         KeyStoreManagerUtils.savePreferenceData(KeyStoreManagerUtils.SESSION_ID, sessionId);
//         await TryConnect();
//     }
// #endif
//
// #if !UNITY_EDITOR && UNITY_WEBGL
//     [MonoPInvokeCallback(typeof(Action))]
//     private static void Web3AuthConnected(string sessionId)
//     {
//         Web3AuthWebGLConnected?.Invoke(sessionId);
//     }
// #endif

    private async void LoginWithWeb3Auth(Provider provider)
    {
// #if UNITY_WEBGL && !UNITY_EDITOR
//         Web3AuthLogin(provider.ToString().ToLower(), rememberMe);
// #else
        selectedProvider = provider;
        await TryConnect();
        LogAnalytics(provider);
    }

    public override async Task TryConnect()
    {
        await base.TryConnect();
        
        EnableWalletGUI();
    }

    private void EnableWalletGUI()
    {
        if (!enableWalletGUI) return;
        var w3aWalletGuiConfig = new Web3AuthWalletGUI.Web3AuthWalletConfig
        {
            DisplayWalletIcon = displayWalletIcon,
            AutoPopUpWalletOnTx = autoPopUpWalletOnTx,
            AutoConfirmTransactions = autoConfirmTransactions,
            WalletIcon = walletIcon,
            WalletLogo = walletLogo,
            DisplayFont = displayFont,
            PrimaryBackgroundColour = primaryBackgroundColour,
            MenuBackgroundColour = menuBackgroundColour,
            PrimaryTextColour = primaryTextColour,
            SecondaryTextColour = secondaryTextColour,
            BorderButtonColour = borderButtonColour
        };
        var web3AuthWalletInstance = Instantiate(web3AuthWalletGUIPrefab);
        web3AuthWalletInstance.GetComponent<Web3AuthWalletGUI>().Initialize(w3aWalletGuiConfig);
    }

    private void LogAnalytics(Provider provider)
    {
        IAnalyticsClient client = (IAnalyticsClient)Web3Accessor.Web3.ServiceProvider.GetService(typeof(IAnalyticsClient));
        client.CaptureEvent(new AnalyticsEvent()
        {
            EventName = $"Login provider {provider}",
            PackageName = "io.chainsafe.web3-unity.web3auth",
        });
// #endif

    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            var web3AuthConfig = new Web3AuthWalletConfig
            {
                Web3AuthOptions = new()
                {
                    clientId = clientId,
                    redirectUrl = new Uri(redirectUri),
                    network = network,
                    whiteLabel = new()
                    {
                        mode = Web3Auth.ThemeModes.dark,
                        defaultLanguage = Web3Auth.Language.en,
                        appName = "ChainSafe Gaming SDK",
                    }
                },
                RememberMe = rememberMe
            };

            // if (selectedProvider.HasValue)
            // {
            //     web3AuthConfig.LoginParams = new LoginParams()
            //     {
            //         loginProvider = selectedProvider.Value
            //     };
            // }

            services.UseWeb3AuthWallet(web3AuthConfig);
        });
    }
}