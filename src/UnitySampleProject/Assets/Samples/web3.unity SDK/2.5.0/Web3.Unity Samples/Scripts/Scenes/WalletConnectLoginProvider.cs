using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using an existing wallet with WalletConnect.
/// </summary>
public class WalletConnectLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
{
    [SerializeField] private WalletConnectConfigSO walletConnectConfig;
    [SerializeField] private bool autoLoginPreviousSession = true;
    [Header("UI")]
    [SerializeField] private Toggle rememberSessionToggle;
    [SerializeField] private Button loginButton;

    private bool storedSessionAvailable;

    protected override async void Initialize()
    {
        base.Initialize();

        await using (var lightWeb3 = await WalletConnectWeb3.BuildLightweightWeb3(walletConnectConfig))
        {
            storedSessionAvailable = lightWeb3.WalletConnect().ConnectionHelper().StoredSessionAvailable;
        }

        if (autoLoginPreviousSession && storedSessionAvailable) // auto-login
        {
            Debug.Log("Proceeding with auto-login.");
            await TryLogin();
        }

        loginButton.onClick.AddListener(OnLoginClicked);
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

    private async void OnLoginClicked()
    {
        await TryLogin();
    }
}