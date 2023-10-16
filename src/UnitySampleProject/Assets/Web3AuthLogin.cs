using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using ChainSafe.GamingSdk.Web3Auth;
using Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Network = Web3Auth.Network;

public class Web3AuthLogin : Login
{
    [Serializable]
    public class ProviderAndButtonPair
    {
        public Button Button;
        public Provider Provider;
    }

    [Serializable]
    public class Web3AuthSettings
    {
        public string ClientId;
        public string RedirectUri;
        public Network Network;
    }

    [SerializeField] private Web3AuthSettings web3AuthSettings;
    [SerializeField] private List<ProviderAndButtonPair> providerAndButtonPairs;

    protected override IEnumerator Initialize()
    {
#if UNITY_WEBGL
            ProcessWeb3Auth();
#endif
        
        //add listener
        foreach (var pair in providerAndButtonPairs)
        {
            pair.Button.onClick.AddListener(() => LoginWithWeb3Auth(pair.Provider));
        }

        yield return null;
    }
    
    private async void LoginWithWeb3Auth(Provider provider)
    {
        Web3Builder web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(ConfigureCommonServices)
            .Configure(services =>
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
                    },
                    LoginParams = new() { loginProvider = provider }
                };
                services.UseWeb3AuthWallet(web3AuthConfig);
            });

        await ProcessLogin(web3Builder);
    }

    private async Task ProcessLogin(Web3Builder web3Builder)
    {
        try
        {
            Web3 web3 = await web3Builder.BuildAsync();
        }
        
        catch (Exception e)
        {
            throw new Exception($"Login With Web3Auth Failed : Failed with Exception {e}");
        }
    }

    private async void ProcessWeb3Auth()
    {
        var web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(ConfigureCommonServices)
            .Configure(services =>
            {
                var web3AuthConfig = new Web3AuthWalletConfig
                {
                    Web3AuthOptions = new()
                    {
                        whiteLabel = new()
                        {
                            dark = true,
                            defaultLanguage = "en",
                            name = "ChainSafe Gaming SDK",
                        },
                        clientId = web3AuthSettings.ClientId,
                        redirectUrl = new Uri(web3AuthSettings.RedirectUri),
                        network = web3AuthSettings.Network,

                    },
                };
                services.UseWeb3AuthWallet(web3AuthConfig);
            });
        
        await ProcessLogin(web3Builder);
    }
}
