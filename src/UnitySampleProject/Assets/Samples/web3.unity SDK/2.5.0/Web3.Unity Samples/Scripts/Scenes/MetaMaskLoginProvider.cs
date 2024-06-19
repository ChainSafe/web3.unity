using System.Collections;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3;
#if UNITY_WEBGL && !UNITY_EDITOR
using ChainSafe.Gaming.MetaMask;
using ChainSafe.Gaming.MetaMask.Unity;
using ChainSafe.Gaming.Web3.Evm.Wallet;
#endif
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using MetaMask.
/// Only works for UnityWebGL build (not in editor).
/// </summary>
public class MetaMaskLoginProvider : LoginProvider, IWeb3BuilderServiceAdapter
{
    [SerializeField] private Button loginButton;
    #if UNITY_WEBGL && !UNITY_EDITOR
    private MetaMaskController metaMaskController;
    #endif

    protected override void Initialize()
    {
        base.Initialize();
        loginButton.onClick.AddListener(LoginClicked);
        #if UNITY_WEBGL && !UNITY_EDITOR
        metaMaskController = Object.FindObjectOfType<MetaMaskController>();
        #endif
    }
    
    public override async Task TryLogin()
    {
        try
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            metaMaskController.ConnectedTsc = new TaskCompletionSource<string>();
            #endif
            await base.TryLogin();
        }
        catch (Web3Exception e)
        {
            errorPopup.ShowError($"Login failed, please try again\n{e.Message}");
            #if UNITY_WEBGL && !UNITY_EDITOR
            if (metaMaskController.ConnectedTsc != null && !metaMaskController.ConnectedTsc.Task.IsCompleted)
            {
                metaMaskController.ConnectedTsc.SetException(e);
            }
            #endif
            throw;
        }
    }

    private async void LoginClicked()
    {
        await TryLogin();
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            // Currently Metamask browser connections can only run in WebGL builds.
            // See point 5 in https://github.com/Nethereum/Unity3dSampleTemplate?tab=readme-ov-file#important-notes.
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseMetaMask().UseWalletSigner().UseWalletTransactionExecutor();
#else
            Debug.LogError("Metamask browser connection, currently, only works on WebGL Builds (not in editor).");
#endif
        });
    }
}
