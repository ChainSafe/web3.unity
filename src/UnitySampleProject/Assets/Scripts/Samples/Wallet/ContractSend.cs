using UnityEngine;

public class ContractSend : MonoBehaviour
{
    async public void Start()
    {
        string method = "addTotal";
        string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        string contractAddress = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";
        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, contractAddress);
        await contract.Send(method, new object[] { 1 });
        // Method `addTotal` returns nothing, see ABI.
        Debug.Log("Script is Done!");
    }
}