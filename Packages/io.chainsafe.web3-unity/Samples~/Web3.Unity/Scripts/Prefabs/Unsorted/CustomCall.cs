using System;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

/// <summary>
/// Call a value from a smart contract
/// </summary>
public class CustomCall : MonoBehaviour
{
    // Variables
    [SerializeField] private string readMethod = "myTotal";
    [SerializeField] private string writeMethod = "addTotal";
    [SerializeField] private string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    [SerializeField] private string address = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";

    /// <summary>
    /// Calls the contract method and logs the result to the console, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTaskRead()
    {
        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, address);
        object[] response = await contract.Call(readMethod, new object[] {});
        Debug.Log($"Total: {response[0]}");
    }
    
    /// <summary>
    /// Writes to the contract method and logs the result to the console, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTaskWrite()
    {
        // Try catch is used here to check for errors during the transaction process
        try
        {
            var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, address);
            object[] response = await contract.Send(writeMethod, new object[] { 1 });
            Debug.Log($"Write Response: {response[0]} Please read the contract again after a moment to see the changes");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
