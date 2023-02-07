using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using UnityEngine;

public class Web3WalletGetTxStatus : MonoBehaviour
{
    public async void GetTransactionStatus()
    {
        var provider = new JsonRpcProvider("YOUR_NODE");
        var signer = new JsonRpcSigner(provider, 0);
        var tx = await signer.SendTransaction(new TransactionRequest
        {
            To = await signer.GetAddress(),
            Value = new HexBigInteger(100000)
        });
        var txReceipt = await tx.Wait();
        Debug.Log("Transaction receipt: " + txReceipt.Confirmations);
    }
}