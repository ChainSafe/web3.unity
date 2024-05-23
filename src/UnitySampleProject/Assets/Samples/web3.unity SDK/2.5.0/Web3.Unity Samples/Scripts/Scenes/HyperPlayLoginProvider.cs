using System;
using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.HyperPlay;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Build;
using Newtonsoft.Json;
using Scenes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Login using HyperPlay desktop client.
/// </summary>
public class HyperPlayLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
{
    public Toggle rememberSessionToggle;
    [SerializeField] private Button loginButton;
    [SerializeField] private bool autoLoginPreviousSession = true;
    private HyperPlayConfigScriptableObject hyperPlayConfigData;
    private bool storedSessionAvailable;
    
    protected override async void Initialize()
    {
        base.Initialize();
        var hyperPlayWallet = await GetHyperPlayWallet();
        storedSessionAvailable = string.Equals(hyperPlayWallet, hyperPlayConfigData.StoredWallet, StringComparison.CurrentCultureIgnoreCase);
        if (autoLoginPreviousSession && storedSessionAvailable)
        {
            Debug.Log("Proceeding with auto-login.");
            await TryLogin();
        }
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        var hyperPlayConfig = new HyperPlayConfig
        {
            storedSessionAvailable = hyperPlayConfigData.StoredSessionAvailable,
            storedWallet = hyperPlayConfigData.StoredWallet,
            rememberMe = hyperPlayConfigData.RememberMe
        };
        return web3Builder.Configure(services =>
        {
            services.UseHyperPlay(hyperPlayConfig).UseHyperPlaySigner().UseHyperPlayTransactionExecutor();
        });
    }

    private async Task<string> GetHyperPlayWallet()
    {
        LoadData();
        var projectConfig = ProjectConfigUtilities.Load();
        string jsonString = $"{{\"request\":{{\"method\":\"eth_accounts\"}},\"chain\":{{\"chainId\":\"{projectConfig.ChainId}\"}}}}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
        UnityWebRequest request = new UnityWebRequest("localhost:9680/rpc", "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        await request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }
        var addressResponse = JsonConvert.DeserializeObject<string[]>(request.downloadHandler.text);
        return addressResponse[0];
    }
        
    private void SaveData(bool _sessionData, string _walletData)
    {
        Debug.Log("Saving data for next login");
        hyperPlayConfigData.StoredSessionAvailable = _sessionData;
        hyperPlayConfigData.StoredWallet = _walletData;
        EditorUtility.SetDirty(hyperPlayConfigData);
        AssetDatabase.SaveAssets();
        Debug.Log($"Session: {_sessionData}");
        Debug.Log($"Wallet: {_walletData}");
    }
        
    private void LoadData()
    {
        Debug.Log("Loading Data");
        hyperPlayConfigData = Resources.Load<HyperPlayConfigScriptableObject>("HyperPlayConfigData");
        hyperPlayConfigData.StoredSessionAvailable = Convert.ToBoolean(hyperPlayConfigData.StoredSessionAvailable);
        hyperPlayConfigData.StoredWallet = hyperPlayConfigData.StoredWallet;
        Debug.Log($"Session: {hyperPlayConfigData.StoredSessionAvailable}");
        Debug.Log($"Wallet: {hyperPlayConfigData.StoredWallet}");
    }
    
    private async void OnLoginClicked()
    {
        if (rememberSessionToggle.isOn)
        {
            hyperPlayConfigData.RememberMe = true;
            SaveData(hyperPlayConfigData.StoredSessionAvailable, hyperPlayConfigData.StoredWallet);
        }
        await TryLogin();
    }
}
