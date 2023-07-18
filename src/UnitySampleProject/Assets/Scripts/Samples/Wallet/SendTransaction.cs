using System;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public class SendTransaction : MonoBehaviour
{
    public async void Start()
    {
        var txRequest = new TransactionRequest
        {
            To = "0xdD4c825203f97984e7867F11eeCc813A036089D1",
            Value = new HexBigInteger(12300000000000000),
        };
        var response = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(txRequest);
        print(response);
    }
}
