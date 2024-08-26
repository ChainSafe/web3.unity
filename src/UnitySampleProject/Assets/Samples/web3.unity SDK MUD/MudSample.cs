using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.Debugging;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Mud;
using ChainSafe.Gaming.Mud.Storages.InMemory;
using ChainSafe.Gaming.Mud.Tables;
using ChainSafe.Gaming.Mud.Unity;
using ChainSafe.Gaming.Mud.Worlds;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MudSample : MonoBehaviour
{
    public MudConfigAsset mudConfig;
    [FormerlySerializedAs("WorldContractAddress")] public string worldContractAddress;
    [FormerlySerializedAs("WorldContractAbi")] public TextAsset worldContractAbi;
    [FormerlySerializedAs("CounterLabel")] public TMP_Text counterLabel;

    private Web3 web3;
    private MudWorld world;

    private async void Awake()
    {
        Debug.Log("To run this sample successfully you should have the MUD tutorial project running in the background.\n" +
                  "Follow the link to get started: https://mud.dev/quickstart");
        
        // 1. Initialize Web3 client.
        web3 = await new Web3Builder(ProjectConfigUtilities.Load(), ProjectConfigUtilities.BuildLocalhostConfig())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();

                // Initializes Wallet as the first account of the locally running Ethereum Node (Anvil).  
                services.Debug().UseJsonRpcWallet(new JsonRpcWalletConfig { AccountIndex = 0 });
                    
                // Enable MUD
                services.UseMud(mudConfig);
            })
            .LaunchAsync();
        Debug.Log($"Web3 client ready. Player address: {web3.Signer.PublicAddress}");
        
        // 2. Create MUD World client.
        world = await web3.Mud().BuildWorld(new MudWorldConfig
        {
            ContractAddress = worldContractAddress,
            ContractAbi = worldContractAbi.text,
            DefaultNamespace = "app",
            TableSchemas = new List<MudTableSchema>
            {
                new()
                {
                    Namespace = "app",
                    TableName = "Counter",
                    Columns = new List<KeyValuePair<string, string>>
                    {
                        new("value", "uint32"),
                    },
                    KeyColumns = new string[0], // empty - singleton table 
                },
            },
        });
        Debug.Log("MUD World client ready");
        
        // 3. Get Table client.
        var table = world.GetTable("Counter");

        // 4. Query counter value 
        var allRecords = await table.Query(MudQuery.All); // Query all records of the Counter table
        var singleRecord = allRecords.Single(); // Get single record
        var counterValue = (BigInteger)singleRecord[0]; // Get value of the first column
        Debug.Log($"Counter value on load: {counterValue}");
        UpdateGui(counterValue);
        
        // 5. Subscribe to table updates.
        table.RecordUpdated += OnCounterRecordUpdated;
    }

    public async void IncrementCounter()
    {
        if (web3 is null)
        {
            Debug.LogError("Can't run sample. Web3 client was not initialized.");
            return;
        }
        
        // 5. Send transaction to execute the Increment function of the World contract.
        Debug.Log("Sending transaction to execute the Increment function..");
        await world.GetSystems().Send("increment");
        Debug.Log($"Increment successful");
    }

    private void OnCounterRecordUpdated(object[] key, object[] record)
    {
        var counterValue = (BigInteger)record[0];
        Debug.Log($"Counter value updated: {counterValue}");
        UpdateGui(counterValue);
    }

    private void UpdateGui(BigInteger counterValue)
    {
        counterLabel.text = counterValue.ToString("d");
    }
}
