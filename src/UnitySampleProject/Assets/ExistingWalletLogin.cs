using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Build;
using Newtonsoft.Json;
using Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

public class ExistingWalletLogin : Login
{
    private const string SavedWalletConnectConfigKey = "SavedWalletConnectConfig";
    
    [Header("UI")]
    
    [SerializeField] private TMP_Dropdown supportedWalletsDropdown;
        
    [SerializeField] private Toggle redirectToWalletToggle;
    
    [SerializeField] public Button loginButton;
    
    [SerializeField] private Toggle rememberMeToggle;

    
    [Header("Wallet Connect")]

    [SerializeField] private string projectId;

    [SerializeField] public string projectName;

    [SerializeField] public string baseContext;

    [SerializeField] private Metadata metadata;
    
    [SerializeField] private WalletConnectModal walletConnectModal;
    
    private bool isRedirectionWalletAgnostic = false;
    
    private bool autoLogin;
    
    private bool redirectToWallet;
    
    private Dictionary<string, WalletConnectWalletModel> supportedWallets;
    
    private WalletConnectConfig walletConnectConfig;
    
    private void OnDestroy()
    {
        if (walletConnectConfig != null)
        {
            walletConnectConfig.OnConnected -= WalletConnected;

            walletConnectConfig.OnSessionApproved -= SessionApproved;
        }
    }
    
    protected override IEnumerator Initialize()
    {
        Assert.IsNotNull(loginButton);
        Assert.IsNotNull(rememberMeToggle);

#if UNITY_ANDROID

        if (!Application.isEditor)
        {
            isRedirectionWalletAgnostic = true;
        }
        
#endif

        if (!isRedirectionWalletAgnostic)
        {
            yield return InitializeWalletSelection();
        }
        
        var autoLoginTask = TryAutoLogin();
            
        yield return new WaitUntil(() => autoLoginTask.IsCompleted);

        loginButton.onClick.AddListener(LoginClicked);
    }

    private async void LoginClicked()
    {
        await TryLogin();
    }
    
    protected override Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services
                .UseWalletConnect(BuildWalletConnectConfig())
                .UseWalletConnectSigner()
                .UseWalletConnectTransactionExecutor();
        });
    }

    private async Task TryAutoLogin()
    {
        string savedConfigJson = PlayerPrefs.GetString(SavedWalletConnectConfigKey, null);

        if (string.IsNullOrEmpty(savedConfigJson))
        {
            return;
        }

        Debug.Log("Attempting to Auto Login...");
            
        try
        {
            autoLogin = true;
            
            walletConnectConfig = JsonConvert.DeserializeObject<WalletConnectConfig>(savedConfigJson);
                
            await TryLogin();
        }
        catch (Exception e)
        {
            Debug.LogError($"Auto Login Failed with Exception {e}");

            autoLogin = false;
        }
    }
    
    // add all supported wallets
    private IEnumerator InitializeWalletSelection()
    {
        yield return FetchSupportedWallets();

        List<string> supportedWalletNames = supportedWallets.Values.Select(w => w.Name).ToList();

        supportedWalletsDropdown.AddOptions(supportedWalletNames);
        
        redirectToWalletToggle.onValueChanged.AddListener(supportedWalletsDropdown.gameObject.SetActive);
    }
    
    private WalletConnectConfig BuildWalletConnectConfig()
        {
            // build chain
            var projectConfig = ProjectConfigUtilities.Load();

            ChainModel chain = new ChainModel(ChainModel.EvmNamespace, projectConfig.ChainId, projectConfig.Network);
            
            WalletConnectWalletModel defaultWallet = null;

            // if it's an auto login get these values from saved wallet config
            if (!autoLogin)
            {
                // allow redirection on editor for testing UI flow
                redirectToWallet = redirectToWalletToggle.isOn;

                // needs wallet selected to redirect
                if (redirectToWallet && !isRedirectionWalletAgnostic)
                {
                    defaultWallet = supportedWallets.Values.ToArray()[supportedWalletsDropdown.value];
                }
            }

            var config = new WalletConnectConfig
            {
                ProjectId = projectId,
                ProjectName = projectName,
                BaseContext = baseContext,
                Chain = chain,
                Metadata = metadata,
                SavedSessionTopic = autoLogin ? walletConnectConfig.SavedSessionTopic : null,
                SupportedWallets = supportedWallets,
                StoragePath = Application.persistentDataPath,
                RedirectToWallet = autoLogin ? walletConnectConfig.RedirectToWallet : redirectToWallet,
                KeepSessionAlive = autoLogin || rememberMeToggle.isOn,
                DefaultWallet = autoLogin ? walletConnectConfig.DefaultWallet : defaultWallet,
            };

            walletConnectConfig = config;
            
            walletConnectConfig.OnConnected += WalletConnected;

            walletConnectConfig.OnSessionApproved += SessionApproved;
            
            return config;
        }
    
    private IEnumerator FetchSupportedWallets()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://registry.walletconnect.org/data/wallets.json"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error Getting Supported Wallets: " + webRequest.error);
                
                yield return null;
            }
            
            else
            {
                var json = webRequest.downloadHandler.text;

                supportedWallets = JsonConvert.DeserializeObject<Dictionary<string, WalletConnectWalletModel>>(json)
                    .ToDictionary(w => w.Key, w => (WalletConnectWalletModel) w.Value);

                Debug.Log($"Fetched {supportedWallets.Count} Supported Wallets.");
            }
        }
    }
    
    private void WalletConnected(ConnectedData data)
    {
        // already redirecting to wallet
        if (walletConnectConfig.RedirectToWallet)
        {
            return;
        }

        // might be null in case of auto login
        if (!string.IsNullOrEmpty(data.Uri))
        {
            // display QR and copy to clipboard
            walletConnectModal.WalletConnected(data);
        }
    }
        
    private void SessionApproved(SessionStruct session)
    {
        // save/persist session
        if (walletConnectConfig.KeepSessionAlive)
        {
            walletConnectConfig.SavedSessionTopic = session.Topic;
            
            PlayerPrefs.SetString(SavedWalletConnectConfigKey, JsonConvert.SerializeObject(walletConnectConfig));
        }

        else
        {
            // reset if any saved config
            PlayerPrefs.SetString(SavedWalletConnectConfigKey, null);
        }
            
        Debug.Log($"{session.Topic} Approved");
    }
}
