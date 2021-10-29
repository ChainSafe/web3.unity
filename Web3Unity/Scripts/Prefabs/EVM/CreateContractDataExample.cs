using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateContractDataExample : MonoBehaviour
{
    async void Start()
    {
        string method = "addTotal";
        // abi in json format
        string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        // array of arguments for contract
        string args = "[\"1\"]";
        
        string contractData = await EVM.CreateContractData(abi, method, args);
        print(contractData);
    }
}
