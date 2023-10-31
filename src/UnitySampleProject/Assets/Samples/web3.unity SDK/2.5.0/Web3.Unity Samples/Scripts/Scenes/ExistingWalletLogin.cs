using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Unity;
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

/// <summary>
/// Login using an existing wallet using Wallet Connect.
/// </summary>
public class ExistingWalletLogin : Login
{
    [Header("UI")] [SerializeField] private TMP_Dropdown supportedWalletsDropdown;

    [SerializeField] private Toggle redirectToWalletToggle;

    [SerializeField] public Button loginButton;

    [SerializeField] private Toggle rememberMeToggle;

    [Header("Wallet Connect")] [SerializeField]
    private string projectId;

    [SerializeField] public string projectName;

    [SerializeField] public string baseContext;

    [SerializeField] private Metadata metadata;

    [SerializeField] private WalletConnectModal walletConnectModal;

    // user isn't required to select wallet to redirect to wallet
    // this is true for android platform since it natively supports WC protocol
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
            // user doesn't need to select wallet before login for redirection since Android supports the WC protocol
            isRedirectionWalletAgnostic = true;
        }

#endif

        if (!isRedirectionWalletAgnostic)
        {
            yield return InitializeWalletSelection();
        }

        // try auto login first
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
            // Build config to use.
            BuildWalletConnectConfig();
            
            // Use wallet connect providers
            services.UseWalletConnect(walletConnectConfig)
                .UseWalletConnectSigner()
                .UseWalletConnectTransactionExecutor();
        });
    }

    private async Task TryAutoLogin()
    {
        // get saved config from Player Data
        walletConnectConfig = PlayerData.Instance.WalletConnectConfig;

        // should be null if config was never saved/cleared
        if (walletConnectConfig == null)
        {
            return;
        }

        Debug.Log("Attempting to Auto Login...");

        try
        {
            autoLogin = true;

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

        // user is only required to select wallet if redirect to wallet toggle is on
        redirectToWalletToggle.onValueChanged.AddListener(supportedWalletsDropdown.gameObject.SetActive);
    }

    private void BuildWalletConnectConfig()
    {
        // build chain
        var projectConfig = ProjectConfigUtilities.Load();

        ChainModel chain = new ChainModel(ChainModel.EvmNamespace, projectConfig.ChainId, projectConfig.Network);

        WalletConnectWalletModel defaultWallet = null;

        // allow redirection on editor for testing UI flow
        redirectToWallet = autoLogin ? 
            // if it's an auto login get value from saved wallet config
            walletConnectConfig.RedirectToWallet : redirectToWalletToggle.isOn;

        // needs wallet selected to redirect
        if (redirectToWallet && !isRedirectionWalletAgnostic)
        {
            defaultWallet = autoLogin ? 
                // if it's an auto login get value from saved wallet config
                walletConnectConfig.DefaultWallet : supportedWallets.Values.ToArray()[supportedWalletsDropdown.value];
        }

        walletConnectConfig = new WalletConnectConfig
        {
            ProjectId = projectId,
            ProjectName = projectName,
            BaseContext = baseContext,
            Chain = chain,
            Metadata = metadata,
            // try and get saved value
            SavedSessionTopic = walletConnectConfig?.SavedSessionTopic,
            SupportedWallets = supportedWallets,
            StoragePath = Application.persistentDataPath,
            RedirectToWallet = redirectToWallet,
            KeepSessionAlive = autoLogin || rememberMeToggle.isOn,
            DefaultWallet = defaultWallet,
        };
        
        //subscribe to WC events
        walletConnectConfig.OnConnected += WalletConnected;

        walletConnectConfig.OnSessionApproved += SessionApproved;
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

                supportedWallets = JsonConvert.DeserializeObject<Dictionary<string, WalletConnectWalletModel>>(json);

                // make sure supported wallet is also supported on platform
                supportedWallets = supportedWallets
                    .Where(w => w.Value.IsAvailableForPlatform(UnityOperatingSystemMediator.GetCurrentPlatform()))
                    .ToDictionary(p => p.Key, p => p.Value);
                
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

            PlayerData.Instance.WalletConnectConfig = walletConnectConfig;

            PlayerData.Save();
        }

        else
        {
            // reset if any saved config
            PlayerData.Instance.WalletConnectConfig = null;

            PlayerData.Save();
        }

        Debug.Log($"{session.Topic} Approved");
    }
}