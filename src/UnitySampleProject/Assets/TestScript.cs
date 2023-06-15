using System;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet;
using ChainSafe.GamingSDK.EVM.WebGLWallet;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

public class TestScript : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;
    
    private Web3 _web3;

    // Start is called before the first frame update
    async void Start()
    {
        var projectConfig = ProjectConfigUtilities.Load();
        Debug.Log($"projectConfig is null: {projectConfig == null}");
        
        _web3 = await new Web3Builder(projectConfig)
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseJsonRpcProvider();
                
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                    services.UseWebGLWallet();
                else
                    services.UseWebPageWallet();
            })
            .BuildAsync();

        await Task.Delay(TimeSpan.FromSeconds(1));
        
        var response = await _web3.Signer.SignMessage("Hey :)))");
        Debug.Log($"Response: {response}");
    }
}
