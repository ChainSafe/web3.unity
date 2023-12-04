using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.MetaMask.Unity;
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

public class MetaMaskLogin : Login
{
    [SerializeField] private Button loginButton;
    
    protected override IEnumerator Initialize()
    {
        loginButton.onClick.AddListener(LoginClicked);
        
        yield return null;
    }

    private async void LoginClicked()
    {
        await TryLogin();
    }
    
    protected override Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.UseMetaMask();
        });
    }
}
