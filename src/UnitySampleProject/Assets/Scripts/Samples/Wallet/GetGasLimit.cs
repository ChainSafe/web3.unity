using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using UnityEngine;

public class GetGasLimit : MonoBehaviour
{
    public async void Start()
    {
        string contractAbi =
            "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        string contractAddress = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";
        var contract = Web3Accessor.Web3.ContractBuilder.Build(contractAbi, contractAddress);
        var gasLimit = await contract.EstimateGas("addTotal", new object[] { });
        Debug.Log("Gas Limit: " + gasLimit);
    }
}