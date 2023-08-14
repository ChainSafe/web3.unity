using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisteredContract : MonoBehaviour
{
    async void Start()
    {
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        var contract = Web3Accessor.Web3.ContractBuilder.Build("shiba");
        var response = await contract.Call(EthMethod.BalanceOf, new[] { account });
        Debug.Log("Contract Length: " + response.Length);
        Debug.Log("Contract Rank: " + response.Rank);
    }
}
