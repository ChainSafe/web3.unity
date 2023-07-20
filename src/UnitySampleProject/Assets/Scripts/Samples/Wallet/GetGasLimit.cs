using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using UnityEngine;

public class GetGasLimit : MonoBehaviour
{
    public async void Start()
    {
        string contractAbi =
            "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        string contractAddress = "0x741C3F3146304Aaf5200317cbEc0265aB728FE07";
        var contract = Web3Accessor.Web3.ContractBuilder.Build(contractAbi, contractAddress);
        var gasLimit = await contract.EstimateGas("addTotal", new object[] { });
        Debug.Log("Gas Limit: " + gasLimit);
    }
}