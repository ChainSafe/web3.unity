using System;
using System.IO;
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
    #region Fields

    [SerializeField] private Button loginButton;
    [SerializeField] private Toggle rememberSessionToggle;
    [SerializeField] private bool autoLoginPreviousSession = true;
    private string hyperPlayWallet;
    private bool storedSessionAvailable;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes class, button & session data if any
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
            
        if (File.Exists(HyperPlayConfig.WALLET_PATH) && File.ReadAllText(HyperPlayConfig.REMEMBER_ME_PATH) == true.ToString())
        {
            rememberSessionToggle.isOn = true;
            LoadData();
        }
        
        loginButton.onClick.AddListener(OnLoginClicked);
    }
    
    /// <summary>
    /// Configures services
    /// </summary>
    /// <param name="web3Builder">Web3 builder object</param>
    /// <returns>Web3 builder object</returns>
    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        HyperPlayConfig hyperPlayConfig = new HyperPlayConfig()
        {
            hyperPlayWallet = this.hyperPlayWallet
        };
        return web3Builder.Configure(services =>
        {
            services.UseHyperPlay(hyperPlayConfig).UseHyperPlaySigner().UseHyperPlayTransactionExecutor();
        });
    }
    
    /// <summary>
    /// Logs the user in and saves session data if requested
    /// </summary>
    private async void OnLoginClicked()
    {
        if (rememberSessionToggle.isOn)
        {
            SaveData();
        }
        else
        {
            ClearData();
        }
        await TryLogin();
    }
    
    /// <summary>
    /// Loads previous session data
    /// </summary>
    private async void LoadData()
    {
        hyperPlayWallet = await HyperPlayProvider.GetConnectedWallet(ProjectConfigUtilities.Load().ChainId);
        storedSessionAvailable = File.Exists(HyperPlayConfig.WALLET_PATH) && string.Equals(hyperPlayWallet, File.ReadAllText(HyperPlayConfig.WALLET_PATH), StringComparison.CurrentCultureIgnoreCase);
        if (!autoLoginPreviousSession || !storedSessionAvailable) return;
        Debug.Log("Proceeding with auto-login.");
        OnLoginClicked();
    }
    
    /// <summary>
    /// Saves session data for usage next time
    /// </summary>
    private void SaveData()
    {
        File.WriteAllText(HyperPlayConfig.REMEMBER_ME_PATH, true.ToString());
        File.WriteAllText(HyperPlayConfig.WALLET_PATH, hyperPlayWallet);
    }
    
    /// <summary>
    /// Clears session data if remember me is unchecked
    /// </summary>
    private void ClearData()
    {
        if (File.Exists(HyperPlayConfig.WALLET_PATH)) File.Delete(HyperPlayConfig.WALLET_PATH);
        File.WriteAllText(HyperPlayConfig.REMEMBER_ME_PATH, false.ToString());
    }

    #endregion
    
}
