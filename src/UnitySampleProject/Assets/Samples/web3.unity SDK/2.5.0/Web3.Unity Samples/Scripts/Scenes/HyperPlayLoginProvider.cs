using System;
using System.IO;
using System.Threading.Tasks;
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
    private string rememberMePath;
    private string walletPath;
    
    protected override void Initialize()
    {
        base.Initialize();
        rememberMePath = Path.Combine(Application.persistentDataPath, "RememberMe.txt");
        walletPath = Path.Combine(Application.persistentDataPath, "HyperPlayWallet.txt");
        
        if (File.Exists(rememberMePath) && File.ReadAllText(rememberMePath) == true.ToString())
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
        storedSessionAvailable = File.Exists(walletPath) && string.Equals(hyperPlayWallet, File.ReadAllText(walletPath), StringComparison.CurrentCultureIgnoreCase);
        if (!autoLoginPreviousSession || !storedSessionAvailable) return;
        Debug.Log("Proceeding with auto-login.");
        OnLoginClicked();
    }
    
    private void SaveData()
    {
        File.WriteAllText(rememberMePath, true.ToString());
        File.WriteAllText(walletPath, hyperPlayWallet);
    }
    
    private void ClearData()
    {
        if (File.Exists(walletPath)) File.Delete(walletPath);
        File.WriteAllText(rememberMePath, false.ToString());
    }
    
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
}