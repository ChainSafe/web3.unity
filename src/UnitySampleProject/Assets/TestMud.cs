using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Mud;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.Gaming.Web3.Unity;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using UnityEngine;

public class TestMud : MonoBehaviour
{
    public WalletConnectConfigSO WalletConnectConfig;
    public string WorldContractAddress;

    private Web3 web3;
    
    [Function("app__increment")]
    public class IncrementFunction : FunctionMessage
    {
    }
    
    [Function("app__getCounter")]
    public class GetCounterFunction : FunctionMessage
    {
    }
    
    public async void RunTest()
    {
        if (web3 != null)
        {
            Debug.Log("Terminating old web3..");
            await web3.TerminateAsync();
        }
        
        web3 = await new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseWalletConnect(WalletConnectConfig.WithRememberSession(true));
                services.UseWalletSigner();
                services.UseWalletTransactionExecutor();
                services.UseMud();
            }).LaunchAsync();
        Debug.Log("New web3 ready for use");
        
        var world = web3.Mud().BuildWorld(WorldContractAddress);
        Debug.Log("World created");
        
        await world.Send<IncrementFunction>();
        Debug.Log("Increment responded");
        
        var counter = await world.Call<GetCounterFunction, int>();
        Debug.Log($"Counter is \"{counter}\"");
    }
}
