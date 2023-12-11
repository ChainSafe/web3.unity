using System.Collections;
#if UNITY_WEBGL
using ChainSafe.Gaming.MetaMask;
using ChainSafe.Gaming.MetaMask.Unity;
#endif
using ChainSafe.Gaming.Web3.Build;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login using MetaMask.
/// Only works for UnityWebGL build (not in editor).
/// </summary>
public class MetaMaskLogin : Login
{
    [SerializeField] private Button loginButton;
    
    protected override IEnumerator Initialize()
    {
        loginButton.onClick.AddListener(LoginClicked);
        
        yield return null;
    }

    private async void LoginClicked()
    {
        await TryLogin();
    }
    
    protected override Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            // Currently Metamask browser connections can only run in WebGL builds.
            // See point 5 in https://github.com/Nethereum/Unity3dSampleTemplate?tab=readme-ov-file#important-notes.
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseMetaMask().UseMetaMaskSigner().UseMetaMaskTransactionExecutor();
#else
            Debug.LogError("Metamask browser connection, currently, only works on WebGL Builds (not in editor).");
#endif
        });
    }
}
