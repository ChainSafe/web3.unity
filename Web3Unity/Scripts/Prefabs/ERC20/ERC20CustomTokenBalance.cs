using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;

public class ERC20CustomTokenBalance : MonoBehaviour
{
    async void Start()
    {
        // abi in json format
        string contractAbi = "YOUR_TOKEN_ABI";
        // address of contract
        string contractAddress = "YOUR_TOKEN_ADDRESS";
        var provider = new JsonRpcProvider("YOUR_NODE");
        var contract = new Contract(contractAbi, contractAddress, provider);
        var calldata = await contract.Call("balanceOf", new object[]
        {
            PlayerPrefs.GetString("Account")
        });
        Debug.Log(calldata[0].ToString());
    }
}