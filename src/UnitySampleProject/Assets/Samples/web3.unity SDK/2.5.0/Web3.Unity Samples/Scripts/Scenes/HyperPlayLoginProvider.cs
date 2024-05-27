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
    private string rememberMePath;
    private string walletPath;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes class, button & session data if any
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        
        rememberMePath = Path.Combine(Application.persistentDataPath, HyperPlayConfig.REMEMBER_ME_PATH);
        walletPath = Path.Combine(Application.persistentDataPath, HyperPlayConfig.WALLET_PATH);
        
        if (File.Exists(walletPath) && File.ReadAllText(rememberMePath) == true.ToString())
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
        storedSessionAvailable = File.Exists(walletPath) && string.Equals(hyperPlayWallet, File.ReadAllText(walletPath), StringComparison.CurrentCultureIgnoreCase);
        if (!autoLoginPreviousSession || !storedSessionAvailable) return;
        Debug.Log("Proceeding with auto-login.");
        OnLoginClicked();
    }
    
    /// <summary>
    /// Saves session data for usage next time
    /// </summary>
    private void SaveData()
    {
        File.WriteAllText(rememberMePath, true.ToString());
        File.WriteAllText(walletPath, hyperPlayWallet);
    }
    
    /// <summary>
    /// Clears session data if remember me is unchecked
    /// </summary>
    private void ClearData()
    {
        if (File.Exists(walletPath)) File.Delete(walletPath);
        File.WriteAllText(rememberMePath, false.ToString());
    }

    #endregion
    
}
