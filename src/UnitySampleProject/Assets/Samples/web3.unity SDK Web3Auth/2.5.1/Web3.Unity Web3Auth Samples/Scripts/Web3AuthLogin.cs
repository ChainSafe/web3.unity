using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Web3Auth;
using Scenes;
using UnityEngine;
using UnityEngine.UI;
using Network = Web3Auth.Network;

public class Web3AuthLogin : Login
{
    [Serializable]
    public struct ProviderAndButtonPair
    {
        public Button Button;
        public Provider Provider;
    }

    [Serializable]
    public struct Web3AuthSettings
    {
        public string ClientId;
        public string RedirectUri;
        public Network Network;
    }

    [SerializeField] private Web3AuthSettings web3AuthSettings;
    [SerializeField] private List<ProviderAndButtonPair> providerAndButtonPairs;

    private bool useProvider;
    
    private Provider selectedProvider;
    
    protected override IEnumerator Initialize()
    {
#if UNITY_WEBGL
        
        useProvider = false;

        Task loginTask = TryLogin();

        yield return new WaitUntil(() => loginTask.IsCompleted);
#endif

        //add listener
        foreach (var pair in providerAndButtonPairs)
        {
            pair.Button.onClick.AddListener(() =>
            {
                LoginWithWeb3Auth(pair.Provider);
            });
        }

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
    }
    
    protected override Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            var web3AuthConfig = new Web3AuthWalletConfig
            {
                Web3AuthOptions = new()
                {
                    clientId = web3AuthSettings.ClientId,
                    redirectUrl = new Uri(web3AuthSettings.RedirectUri),
                    network = web3AuthSettings.Network,
                    whiteLabel = new()
                    {
                        dark = true,
                        defaultLanguage = "en",
                        name = "ChainSafe Gaming SDK",
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
