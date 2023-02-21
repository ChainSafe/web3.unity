using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using UnityEngine;

#if UNITY_WEBGL
public class WebGLGetGasLimit : MonoBehaviour
{
    public async void GetGasLimit()
    {
        var provider = new JsonRpcProvider("YOUR_NODE");
        string contractAbi =
            "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        string contractAddress = "0x741C3F3146304Aaf5200317cbEc0265aB728FE07";
        var contract = new Contract(contractAbi, contractAddress, provider);
        var gasLimit = await contract.EstimateGas("addTotal", new object[] { });
        Debug.Log("Gas Limit: " + gasLimit);
    }
}
#endif