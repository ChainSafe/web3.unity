using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Web3Auth;
using Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Web3Auth.Network;

/// <summary>
/// Login using Web3Auth.
/// </summary>
public class Web3AuthLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
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
    private bool useProvider;
    private Provider selectedProvider;
    private bool rememberMe;

    public void SetRememberMe(bool rememberMe)
    {
        this.rememberMe = rememberMe;
    }

    protected override async void Initialize()
    {
        base.Initialize();

        //Always first add listeners.
        providerAndButtonPairs.ForEach(p =>
            p.Button.onClick.AddListener(delegate { LoginWithWeb3Auth(p.Provider); }));

#if UNITY_WEBGL && !UNITY_EDITOR
        Uri uri = new Uri(Application.absoluteURL);

        // make sure this load isn't redirected from Web3Auth a login
        if (!string.IsNullOrEmpty(uri.Fragment))
        {
            useProvider = false;
            await TryLogin();
        }
#else
        if (!string.IsNullOrEmpty(KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID)) && rememberMe)
        {
            useProvider = false;
            await TryLogin();
            Debug.Log("Restoring existing Web3Auth session (Remember Me");
        }
#endif
    }

    private async void LoginWithWeb3Auth(Provider provider)
    {
        if (!useProvider)
        {
            useProvider = true;
        }
        selectedProvider = provider;
        await TryLogin();
        LogAnalytics(provider);
    }
    
    public override async Task TryLogin()
    {
        try
        {
            await (this as ILoginProvider).Login();
            EnableWalletGUI();
        }
        catch (Exception e)
        {
            Debug.LogError($"Login failed, please try again\n{e.Message} (see console for more details)");
            throw;
        }
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

            if (useProvider)
            {
                web3AuthConfig.LoginParams = new LoginParams()
                {
                    loginProvider = selectedProvider
                };
            }

            services.UseWeb3AuthWallet(web3AuthConfig);
        });
    }
}