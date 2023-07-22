using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;

public class ERC20CustomTokenBalance : MonoBehaviour
{
    // abi in json format
    [SerializeField]
    string contractAbi;
    // address of contract
    [SerializeField]
    string contractAddress;

    async void Start()
    {
        var contract = Web3Accessor.Web3.ContractBuilder.Build(contractAbi, contractAddress);
        var calldata = await contract.Call("balanceOf", new object[]
        {
            await Web3Accessor.Web3.Signer.GetAddress()
        });
        Debug.Log(calldata[0].ToString());
    }
}