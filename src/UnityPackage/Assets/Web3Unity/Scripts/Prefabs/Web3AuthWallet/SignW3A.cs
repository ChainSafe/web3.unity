using UnityEngine.UI;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class SignW3A : MonoBehaviour
{
    public Text responseText;
    public string message = "This is a test message to sign";
    private GameObject CSWallet = null;

    public void OnEnable()
    {
        // resets response text
        responseText.text = "";
    }

    public void OnSignMessage()
    {
        // finds the wallet, sets sign and incoming tx conditions to true and opens
        CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
        W3AWalletUtils.incomingTx = true;
        W3AWalletUtils.incomingAction = "Sign";
        W3AWalletUtils.incomingMessageData = message;
        CSWallet.GetComponent<Web3AuthWallet>().OpenButton();
    }

    void Update()
    {
        if (W3AWalletUtils.signedTxResponse != "")
        {
            // display signed tx response from wallet
            responseText.text = W3AWalletUtils.signedTxResponse;
            W3AWalletUtils.signedTxResponse = "";
        }
    }
}