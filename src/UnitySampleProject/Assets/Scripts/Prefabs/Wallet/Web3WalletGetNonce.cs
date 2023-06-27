using UnityEngine;

public class Web3WalletGetNonce : MonoBehaviour
{
    // todo rework with new architecture in mind
    // public async void GetNonce()
    // {
    //     var provider = ProviderMigration.NewJsonRpcProvider("YOUR_NODE");
    //     var signer = SignerMigration.NewJsonRpcSigner(provider, 0);
    //     var tx = await signer.SendTransaction(new TransactionRequest
    //     {
    //         To = await signer.GetAddress(),
    //         Value = new HexBigInteger(100000)
    //     });
    //     var nonce = tx.Nonce;
    //     Debug.Log("Nonce: " + nonce);
    // }
}