using ChainSafe.Gaming.HyperPlay;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using HyperPlay desktop client.
/// </summary>
public class HyperPlayLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
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
            
            await TryLogin();
        }

        loginButton.onClick.AddListener(OnLoginClicked);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.UseHyperPlay(new HyperPlayConfig
            {
                RememberSession = rememberMeToggle.isOn || _storedSessionAvailable,
            }).UseHyperPlaySigner().UseHyperPlayTransactionExecutor();
        });
    }
    
    private async void OnLoginClicked()
    {
        await TryLogin();
    }
}
