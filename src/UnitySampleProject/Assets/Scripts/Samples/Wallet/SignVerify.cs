using System.Text;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;

public class SignVerify : MonoBehaviour
{
    async void Start()
    {
        var message = "Hello from CS gaming!";
        var address = await Web3Accessor.Web3.Signer.GetAddress();
        string signatureString = await Web3Accessor.Web3.Signer.SignMessage(message);

        string msg = "\x19" + "Ethereum Signed Message:\n" + message.Length + message;
        byte[] msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        EthECDSASignature signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        EthECKey key = EthECKey.RecoverFromSignature(signature, msgHash);

        if (key.GetPublicAddress() != address)
        {
            Debug.LogError("Message was signed by a different wallet");
        }
        else
        {
            Debug.Log("Verified successfully");
        }
    }
}
