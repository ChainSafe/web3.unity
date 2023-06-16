using System;
using System.Text;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;
// using Web3Unity.Scripts.Library.Web3Wallet;

public class Web3WalletSignVerify : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        // string message = "hello";
        // string signature = await Web3Wallet.Sign(message);
        // //verification
        // SignVerifySignature(signature, message);
    }

    public void SignVerifySignature(string signatureString, string originalMessage)
    {
        throw new NotImplementedException(
            "Example scripts are in the process of migration to the new API. This function has not yet been migrated.");

        // string msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
        // byte[] msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        // EthECDSASignature signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        // EthECKey key = EthECKey.RecoverFromSignature(signature, msgHash);
        //
        // bool isValid = key.Verify(msgHash, signature);
        // Debug.Log("Address Returned: " + key.GetPublicAddress());
        // Debug.Log("Is Valid: " + isValid);
    }
}
