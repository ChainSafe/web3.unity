using System;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Web3Auth;
using UnityEngine;
using Network = Web3Auth.Network;

public class Web3AuthConnectionProvider : ConnectionProvider
{
    [SerializeField] private string clientId;
    [SerializeField] private string redirectUri;
    [SerializeField] private Network network;
    
    [Space]
    
    [SerializeField] public GameObject modalPrefab;
    
    private Web3AuthModal _modal;

    public override bool IsAvailable => true;
    
    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        DisplayModal();
        
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
                // RememberMe = rememberMe
            };

            web3AuthConfig.CancellationToken = _modal.CancellationToken;
            
            web3AuthConfig.ProviderTask = _modal.SelectProvider();

            services.UseWeb3AuthWallet(web3AuthConfig);
        });
    }

    private void DisplayModal()
    {
        if (_modal != null)
        {
            _modal.gameObject.SetActive(true);
        }

        else
        {
            var obj = Instantiate(modalPrefab);
            
            _modal = obj.GetComponentInChildren<Web3AuthModal>();
        }
    }
}
