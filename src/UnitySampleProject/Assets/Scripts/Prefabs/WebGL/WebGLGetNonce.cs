using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using UnityEngine;

#if UNITY_WEBGL
public class WebGLGetNonce : MonoBehaviour
{
    public async void GetNonce()
    {
        // todo: rework with new architecture in mind
        // var provider = ProviderMigration.NewJsonRpcProvider("YOUR_NODE");
        // var signer = SignerMigration.NewJsonRpcSigner(provider, 0);
        // var tx = await signer.SendTransaction(new TransactionRequest
        // {
        //     To = await signer.GetAddress(),
        //     Value = new HexBigInteger(100000)
        // });
        // var nonce = tx.Nonce;
        // Debug.Log("Nonce: " + nonce);
    }
}
#endif