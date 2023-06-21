using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisteredContract : MonoBehaviour
{
    async void Start()
    {
        var account = await Web3Accessor.Instance.Web3.Signer.GetAddress();
        var contract = Web3Accessor.Instance.Web3.ContractFactory.Build("shiba");
        var response = await contract.Call(EthMethod.BalanceOf, new[] { account });
        Debug.Log(response);
    }
}
