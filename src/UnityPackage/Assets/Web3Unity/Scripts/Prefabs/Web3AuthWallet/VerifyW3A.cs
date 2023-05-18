using UnityEngine.UI;
using System.Text;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class VerifyW3A : MonoBehaviour
{
    public Text responseText;
    public string message = "hello";
    private GameObject CSWallet = null;

    public void OnEnable()
    {
        // resets response text
        responseText.text = "";
    }

    public void UserSign()
    {
        // finds the wallet, sets sign and incoming tx conditions to true and opens
        CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
        W3AWalletUtils.incomingTx = true;
        W3AWalletUtils.incomingAction = "Sign";
        W3AWalletUtils.incomingMessageData = message;
        CSWallet.GetComponent<Web3AuthWallet>().OpenButton();
    }

    public void SignVerifySignature(string signatureString, string originalMessage)
    {
        string msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
        byte[] msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        EthECDSASignature signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        EthECKey key = EthECKey.RecoverFromSignature(signature, msgHash);

        bool isValid = key.Verify(msgHash, signature);
        Debug.Log("Address Returned: " + key.GetPublicAddress());
        Debug.Log("Is Valid: " + isValid);
        // display signed tx response from wallet
        responseText.text = "Verify Address: " + key.GetPublicAddress();
    }

    void Update()
    {
        if (W3AWalletUtils.signedTxResponse != "")
        {
            //verification
            SignVerifySignature(W3AWalletUtils.signedTxResponse, message);
            W3AWalletUtils.signedTxResponse = "";
        }
    }
}