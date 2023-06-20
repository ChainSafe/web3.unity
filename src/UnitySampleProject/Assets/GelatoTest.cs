using ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using ChainSafe.GamingSdk.Gelato;
using ChainSafe.GamingSdk.Gelato.Types;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

public class GelatoTest : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var web3 = await new Web3Builder(ProjectConfigUtilities.Load()).Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseJsonRpcProvider();
            services.UseWebPageWallet();
            services.UseGelatoModule();
        }).BuildAsync();
        
        
        var gelatoInstance = web3.ServiceProvider.GetRequiredService<IGelatoModule>();
        gelatoInstance.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
