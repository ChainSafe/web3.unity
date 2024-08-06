using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.Debugging;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Mud;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.Gaming.Web3.Unity;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class MudSample : MonoBehaviour
{
    public string WorldContractAddress;

    private Web3 web3;
    
    [Function("app__increment", "uint32")]
    public class IncrementFunction : FunctionMessage
    {
    }
    
    // Try implementing a new Mud System with the GetCounter function. Uncomment then:
    
    // [Function("app__getCounter", "uint32")]
    // public class GetCounterFunction : FunctionMessage
    // {
    // }

    private void Awake()
    {
        Debug.Log("To run this sample successfully you should have the MUD tutorial project running in the background.\n" +
                  "Follow the link https://mud.dev/quickstart");
    }

    public async void IncrementCounter()
    {
        if (web3 != null)
        {
            Debug.Log("Terminating old web3..");
            await web3.TerminateAsync();
        }

        // 1. Initialize web3 client.
        web3 = await new Web3Builder(ProjectConfigUtilities.Load(), ProjectConfigUtilities.BuildLocalhostConfig())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseRpcProvider();

                    // Initializes the Wallet as the first account of the locally running Ethereum Node (Anvil).  
                    services.Debug().UseJsonRpcWallet(new JsonRpcWalletConfig { AccountIndex = 0 });
                    services.UseMud();
                }).LaunchAsync();
        Debug.Log("New Web3 client ready");
        
        // 2. Create MUD World client.
        var world = web3.Mud().BuildWorld(WorldContractAddress);
        Debug.Log("MUD World client ready");
        
        // 3. Send transaction to execute the Increment function of the World contract.
        Debug.Log("Sending transaction to execute IncrementFunction..");
        await world.Send<IncrementFunction>();
        Debug.Log("Increment successful");
        
        
        Debug.Log("You can also call read-only functions for free. Open this script in the editor to continue.");
        
        // 4. Try implementing a new Mud System with the GetCounter function. Uncomment then:
        
        // var counter = await world.Call<GetCounterFunction, int>();
        // Debug.Log($"Counter is \"{counter}\"");
    }
}
