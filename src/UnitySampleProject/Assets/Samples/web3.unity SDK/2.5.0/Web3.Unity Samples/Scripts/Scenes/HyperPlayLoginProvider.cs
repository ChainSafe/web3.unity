using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.HyperPlay;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using HyperPlay desktop client.
/// </summary>
public class HyperPlayLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
{
    [SerializeField] private Button loginButton;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.UseHyperPlay().UseHyperPlaySigner().UseHyperPlayTransactionExecutor();
        });
    }
    
    private async void OnLoginClicked()
    {
        await TryLogin();
    }
}
