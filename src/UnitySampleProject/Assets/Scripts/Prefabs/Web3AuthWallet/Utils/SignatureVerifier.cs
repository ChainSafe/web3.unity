using System.Text;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;

namespace Prefabs.Web3AuthWallet.Utils
{
    public class SignatureVerifier
    {
        public string VerifySignature(string signatureString, string originalMessage)
        {
            string msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
            byte[] msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
            EthECDSASignature signature = MessageSigner.ExtractEcdsaSignature(signatureString);
            EthECKey key = EthECKey.RecoverFromSignature(signature, msgHash);

            bool isValid = key.Verify(msgHash, signature);
            Debug.Log("Address Returned: " + key.GetPublicAddress());
            Debug.Log("Is Valid: " + isValid);

            // return signed tx response from wallet
            return key.GetPublicAddress();
        }
    }
}