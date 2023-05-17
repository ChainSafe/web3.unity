using System;
using System.Numerics;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class TransferW3A: MonoBehaviour
{
    public Text responseText;
    public string contractAddress = "0xed7f68Ed23bB75841ab1448A95fa19aA46e9EA3E";
    public string toAccount = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
    public string amount = "1000000000000000000";
    public string contractAbi = "[ { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"mint\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [], \"name\": \"_decimal\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
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
        // connects to user's browser wallet (metamask) to send a transaction
        try {
            // connects to user's browser wallet to call a transaction
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
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }

    public void Mint()
    {
        string account = PlayerPrefs.GetString("Account");
        // smart contract method to call
        string method = "mint";
        W3AWalletUtils.outgoingContract = contractAddress;
        // connects to user's browser wallet (metamask) to send a transaction
        try {
            var contract = new Contract(contractAbi, contractAddress);
            Debug.Log("Contract: " + contract);
            var calldata = contract.Calldata(method, new object[]
            {
                account,
                BigInteger.Parse(amount)
            });
            Debug.Log("Contract Data: " + calldata);
            // finds the wallet, sets sign and incoming tx conditions to true and opens
            CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
            W3AWalletUtils.incomingTx = true;
            W3AWalletUtils.incomingAction = "Broadcast";
            W3AWalletUtils.incomingTxData = calldata;
            CSWallet.GetComponent<Web3AuthWallet>().OpenButton();
        } catch (Exception e) {
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