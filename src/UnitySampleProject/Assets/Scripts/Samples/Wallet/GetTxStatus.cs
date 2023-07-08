using Nethereum.Hex.HexTypes;
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
            Value = new HexBigInteger(100000)
        });
        var txReceipt = await tx.Wait();
        Debug.Log("Transaction receipt: " + txReceipt.Confirmations);
    }
}