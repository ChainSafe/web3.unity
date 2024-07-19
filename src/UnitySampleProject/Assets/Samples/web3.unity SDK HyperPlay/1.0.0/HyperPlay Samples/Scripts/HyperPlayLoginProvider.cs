using ChainSafe.Gaming.HyperPlay;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using HyperPlay desktop client.
/// </summary>
public class HyperPlayLoginProvider : ConnectionHandler, IWeb3BuilderServiceAdapter
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Toggle rememberMeToggle;

    private bool _storedSessionAvailable;

    protected override async void Initialize()
    {
        base.Initialize();

        _storedSessionAvailable = false;

        await using (var lightWeb3 = await HyperPlayWeb3.BuildLightweightWeb3(new HyperPlayConfig()))
        {
            var data = lightWeb3.ServiceProvider.GetService<IHyperPlayData>();

            _storedSessionAvailable = data.RememberSession;
        }

        if (_storedSessionAvailable) // auto-login
        {
            Debug.Log("Proceeding with auto-login.");

            await TryConnect();
        }

        loginButton.onClick.AddListener(OnLoginClicked);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            var config = new HyperPlayConfig
            {
                RememberSession = rememberMeToggle.isOn || _storedSessionAvailable,
            };
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseHyperPlay<HyperPlayWebGLProvider>(config);
#else
            services.UseHyperPlay(config);
#endif
            services.UseWalletSigner().UseWalletTransactionExecutor();
        });
    }

    private async void OnLoginClicked()
    {
        await TryConnect();
    }
}
