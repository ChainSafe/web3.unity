using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
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
public class Web3AuthLogin : Login
{
    /// <summary>
    /// Struct used for pairing login buttons to Web3 auth providers.
    /// Used when adding <see cref="Web3AuthLogin.LoginWithWeb3Auth"/> as listeners to the buttons.
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

    protected override IEnumerator Initialize()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Uri uri = new Uri(Application.absoluteURL);

        // make sure this load isn't redirected from Web3Auth a login
        if (!string.IsNullOrEmpty(uri.Fragment))
        {
            useProvider = false;

            Task loginTask = TryLogin();
            
            yield return new WaitUntil(() => loginTask.IsCompleted);
        }
#endif

        // add provider buttons listeners
        providerAndButtonPairs.ForEach(p => p.Button.onClick.AddListener(delegate
        {
            LoginWithWeb3Auth(p.Provider);
        }));

        yield return null;
    }

    private async void LoginWithWeb3Auth(Provider provider)
    {
        if (!useProvider)
        {
            useProvider = true;
        }
        selectedProvider = provider;

        await TryLogin();
        IAnalyticsClient client = (IAnalyticsClient)Web3Accessor.Web3.ServiceProvider.GetService(typeof(IAnalyticsClient));
        client.CaptureEvent(new AnalyticsEvent()
        {
            ChainId = Web3Accessor.Web3.ChainConfig.ChainId,
            Network = Web3Accessor.Web3.ChainConfig.Network,
            ProjectId = Web3Accessor.Web3.ProjectConfig.ProjectId,
            EventName = "Web3AuthLogin",
            Version = client.AnalyticsVersion,
            PackageName = "io.chainsafe.web3-unity.web3auth",
        });
    }

    protected override Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
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
                }
            };

            if (useProvider)
            {
                web3AuthConfig.LoginParams = new LoginParams
                {
                    loginProvider = selectedProvider
                };
            }

            services.UseWeb3AuthWallet(web3AuthConfig);
        });
    }
}
