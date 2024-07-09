using ChainSafe.Gaming.UnityPackage.Common;
#if UNITY_WEBGL && !UNITY_EDITOR
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.Gaming.Unity.MetaMask;
#endif
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using MetaMask.
/// Only works for UnityWebGL build (not in editor).
/// </summary>
public class MetaMaskLoginProvider : ConnectionHandler, IWeb3BuilderServiceAdapter
{
    [SerializeField] private Button loginButton;

    protected override void Initialize()
    {
        base.Initialize();

        loginButton.onClick.AddListener(LoginClicked);
    }

    private async void LoginClicked()
    {
        await TryConnect();
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
