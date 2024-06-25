using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnectUnity;
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

public class WalletConnectUnityLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
{
    [SerializeField] private Button loginButton;

    protected override void Initialize()
    {
        base.Initialize();

        loginButton.onClick.AddListener(LoginClicked);
    }

    private async void LoginClicked()
    {
        await TryLogin();
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.UseWalletConnectUnity()
                .UseWalletConnectSigner()
                .UseWalletConnectTransactionExecutor();
        });
    }
}