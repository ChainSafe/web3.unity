using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Web3Auth;
using Scenes;
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
        if (!string.IsNullOrEmpty(KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID)))
        {
            useProvider = false;
            rememberMe = true;
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
