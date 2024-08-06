using System.Numerics;
using ChainSafe.Gaming.Debugging;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Mud;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Mud;
using TMPro;
using UnityEngine;

public class MudSample : MonoBehaviour
{
    public string WorldContractAddress;
    public TextAsset WorldContractAbi;
    public TMP_Text CounterLabel;

    private Web3 web3;
    private MudWorld world;

    public class CounterRecord : TableRecordSingleton<CounterRecord.CounterValue> // singleton table record - no key required
    {
        public class CounterValue
        {
            [Parameter("uint32", "value", 1)] // column name
            public BigInteger Counter { get; set; }
        }

        public CounterRecord() : base("app", "Counter") // table name
        {
        }
    }

    private async void Awake()
    {
        Debug.Log("To run this sample successfully you should have the MUD tutorial project running in the background.\n" +
                  "Follow the link https://mud.dev/quickstart");
        
        // 1. Initialize Web3 client.
        web3 = await new Web3Builder(ProjectConfigUtilities.Load(), ProjectConfigUtilities.BuildLocalhostConfig())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();

                // Initializes Wallet as the first account of the locally running Ethereum Node (Anvil).  
                services.Debug().UseJsonRpcWallet(new JsonRpcWalletConfig { AccountIndex = 0 });
                    
                // Enable MUD
                services.UseMud();
            }).LaunchAsync();
        Debug.Log("Web3 client ready");
        
        // 2. Create MUD World client.
        world = web3.Mud().BuildWorld(WorldContractAddress, WorldContractAbi.text);
        Debug.Log("MUD World client ready");

        // 3. Query current counter value.
        var counterValue = (await world.Query<CounterRecord, CounterRecord.CounterValue>()).Counter;
        Debug.Log($"Counter value on load: {counterValue}");
        CounterLabel.text = counterValue.ToString("d");
    }

    public async void IncrementCounter()
    {
        if (web3 is null)
        {
            Debug.LogError("Can't run sample. Web3 client was not initialized.");
            return;
        }
        
        // 4. Send transaction to execute the Increment function of the World contract.
        Debug.Log("Sending transaction to execute the Increment function..");
        await world.Send("app__increment");
        Debug.Log($"Increment successful");
        
        // 5. Query new counter value.
        var counterValue = (await world.Query<CounterRecord, CounterRecord.CounterValue>()).Counter;
        Debug.Log($"Counter value after increment: {counterValue}");
        CounterLabel.text = counterValue.ToString("d");
    }
}
