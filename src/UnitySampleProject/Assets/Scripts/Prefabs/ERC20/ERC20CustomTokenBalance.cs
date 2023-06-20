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
        var contract = Web3Accessor.Instance.Web3.ContractFactory.Build(contractAbi, contractAddress);
        var calldata = await contract.Call("balanceOf", new object[]
        {
            PlayerPrefs.GetString("Account")
        });
        Debug.Log(calldata[0].ToString());
    }
}