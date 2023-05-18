using System;
using System.Numerics;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class TransferW3A : MonoBehaviour
{
    public Text responseText;
    public string contractAddress;
    public string toAccount = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
    public string amount = "1000000000000000000";
    public string contractAbi;
    private GameObject CSWallet = null;

    public void OnEnable()
    {
        // resets response text
        responseText.text = "";
    }

    public void Transfer()
    {
        // smart contract method to call
        string method = "transfer";
        W3AWalletUtils.outgoingContract = contractAddress;
        // connects to user's wallet to send a transaction
        try
        {
            // connects to user's wallet to call a transaction
            var contract = new Contract(contractAbi, contractAddress);
            Debug.Log("Contract: " + contract);
            var calldata = contract.Calldata(method, new object[]
            {
                toAccount,
                BigInteger.Parse(amount)
            });
            Debug.Log("Contract Data: " + calldata);
            // finds the wallet, sets sign and incoming tx conditions to true and opens
            CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
            W3AWalletUtils.incomingTx = true;
            W3AWalletUtils.incomingAction = "Broadcast";
            W3AWalletUtils.incomingTxData = calldata;
            CSWallet.GetComponent<Web3AuthWallet>().OpenButton();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public void Mint()
    {
        // smart contract method to call
        string method = "mint";
        W3AWalletUtils.outgoingContract = contractAddress;
        // connects to user's wallet to send a transaction
        try
        {
            var contract = new Contract(contractAbi, contractAddress);
            Debug.Log("Contract: " + contract);
            var calldata = contract.Calldata(method, new object[]
            {
                W3AWalletUtils.account,
                BigInteger.Parse(amount)
            });
            Debug.Log("Contract Data: " + calldata);
            // finds the wallet, sets sign and incoming tx conditions to true and opens
            CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
            W3AWalletUtils.incomingTx = true;
            W3AWalletUtils.incomingAction = "Broadcast";
            W3AWalletUtils.incomingTxData = calldata;
            CSWallet.GetComponent<Web3AuthWallet>().OpenButton();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
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