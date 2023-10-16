using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Build;
using Newtonsoft.Json;
using Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.UI;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

public class ExistingWalletLogin : Login
{
    internal const string SavedWalletConnectConfigKey = "SavedWalletConnectConfig";
    
    [SerializeField] private TMP_Dropdown supportedWalletsDropdown;
        
    [SerializeField] private Toggle redirectToWalletToggle;
    
    [Header("Wallet Connect")]

    [SerializeField] private string projectId;

    [SerializeField] public string projectName;

    [SerializeField] public string baseContext;

    [SerializeField] private Metadata metadata;
    
    [SerializeField] private WalletConnectModal walletConnectModal;
    
    private bool isRedirectionWalletAgnostic = false;
    
    private bool autoLogin;
    
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
        Assert.IsNotNull(ExistingWalletButton);
        Assert.IsNotNull(RememberMeToggle);

#if UNITY_ANDROID
        
        isRedirectionWalletAgnostic = true;
        
#endif

        if (!isRedirectionWalletAgnostic)
        {
            yield return InitializeWalletSelection();
        }
        
        var autoLoginTask = TryAutoLogin();
            
        yield return new WaitUntil(() => autoLoginTask.IsCompleted);

        ExistingWalletButton.onClick.AddListener(TryLogin);
    }

    protected override Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services
                .UseWalletConnectProvider(BuildWalletConnectConfig())
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
                
            await LoginWithExistingAccount();
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
