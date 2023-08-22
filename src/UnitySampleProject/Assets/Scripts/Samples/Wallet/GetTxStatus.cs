using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using UnityEngine;

public class GetTxStatus : MonoBehaviour
{
    public async void Start()
    {
        var address = await Web3Accessor.Web3.Signer.GetAddress();
        var tx = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(new TransactionRequest
        {
            To = address,
            Value = new HexBigInteger(10000000)
        });
        var txReceipt = await Web3Accessor.Web3.RpcProvider.WaitForTransactionReceipt(tx.Hash);
        Debug.Log("Transaction receipt: " + txReceipt.Confirmations);
    }
}