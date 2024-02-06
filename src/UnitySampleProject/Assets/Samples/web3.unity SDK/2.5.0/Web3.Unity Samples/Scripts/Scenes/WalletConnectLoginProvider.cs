using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using an existing wallet using Wallet Connect.
/// </summary>
public class WalletConnectLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
{
    [SerializeField] private WalletConnectConfigSO walletConnectConfig;
    [SerializeField] private Toggle rememberSessionToggle;
    [SerializeField] private bool AutoLoginPreviousSession = true;

    private bool storedSessionAvailable;

    private async void Awake()
    {
        var connectionHelper = await new Web3Builder(ProjectConfigUtilities.Load()) // build lightweight web3 
            .BuildConnectionHelper(walletConnectConfig);
        
        storedSessionAvailable = connectionHelper.StoredSessionAvailable;
        
        if (AutoLoginPreviousSession && storedSessionAvailable) // auto-login
        {
            Debug.Log("Proceeding with auto-login.");
            await TryLogin();
        }
    }
    
    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            var rememberSession = rememberSessionToggle.isOn || storedSessionAvailable;

            services.UseWalletConnect(walletConnectConfig.WithRememberSession(rememberSession))
                .UseWalletConnectSigner()
                .UseWalletConnectTransactionExecutor();
        });
    }
}