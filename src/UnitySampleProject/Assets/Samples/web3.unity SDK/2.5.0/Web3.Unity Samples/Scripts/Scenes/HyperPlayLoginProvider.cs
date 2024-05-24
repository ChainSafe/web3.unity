using System;
using ChainSafe.Gaming.HyperPlay;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Common;
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
    [SerializeField] private Toggle rememberSessionToggle;
    [SerializeField] private bool autoLoginPreviousSession = true;
    private bool storedSessionAvailable;
    private string hyperPlayWallet;
    
    protected override void Initialize()
    {
        base.Initialize();
        if (PlayerPrefs.GetString("RememberMe") == true.ToString())
        {
            rememberSessionToggle.isOn = true;
            LoadData();
        }
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.UseHyperPlay().UseHyperPlaySigner().UseHyperPlayTransactionExecutor();
        });
    }
    
    private async void LoadData()
    {
        hyperPlayWallet = await HyperPlayProvider.GetConnectedWallet(ProjectConfigUtilities.Load().ChainId);
        storedSessionAvailable = string.Equals(hyperPlayWallet, PlayerPrefs.GetString("HyperPlayWallet"), StringComparison.CurrentCultureIgnoreCase);
        if (!autoLoginPreviousSession || !storedSessionAvailable) return;
        Debug.Log("Proceeding with auto-login.");
        OnLoginClicked();
    }
    
    private void SaveData()
    {
        PlayerPrefs.SetString("RememberMe", true.ToString());
        PlayerPrefs.SetString("HyperPlayWallet", hyperPlayWallet);
    }
    
    private async void OnLoginClicked()
    {
        if (rememberSessionToggle.isOn)
        {
            SaveData();
        }
        else
        {
            PlayerPrefs.SetString("HyperPlayWallet", string.Empty);
            PlayerPrefs.SetString("RememberMe", false.ToString());
        }
        await TryLogin();
    }
}
