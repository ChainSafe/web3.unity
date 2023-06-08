using Web3Unity.Scripts.Library.Ethers.Contracts;
using UnityEngine.UI;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class ContractCallSignW3A : MonoBehaviour
{
    public string chain = "ethereum";
    // set network mainnet, testnet
    public string network = "goerli";
    // abi in json format
    public string contractAbi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    // address of contract
    public string contractAddress = "0x741C3F3146304Aaf5200317cbEc0265aB728FE07";
    public Text responseText;
    private GameObject CSWallet = null;

    /*
        //Solidity Contract
        // SPDX-License-Identifier: MIT
        pragma solidity ^0.8.0;

        contract AddTotal {
            uint256 public myTotal = 0;

            function addTotal(uint8 _myArg) public {
                myTotal = myTotal + _myArg;
            }
        }
    */

    public void OnEnable()
    {
        // resets response text
        responseText.text = "";
    }

    async public void CheckVariable()
    {
        string method = "myTotal";
        // you can use this to create a provider for hardcoding and parse this instead of rpc get instance
        //var provider = new JsonRpcProvider(PlayerPrefs.GetString("RPC"));
        var contract = new Contract(contractAbi, contractAddress, RPC.GetInstance.Provider());
        Debug.Log("Contract: " + contract);
        var calldata = await contract.Call(method, new object[]
        {
            // if you need to add parameters you can do so, a call with no args is blank
            // arg1,
            // arg2
        });
        Debug.Log("Contract Data: " + calldata[0]);
        // display response in game
        print("Contract Variable Total: " + calldata[0]);
        responseText.text = "Contract Variable Total: " + calldata[0];
    }

    public void AddOneToVariable()
    {
        string method = "addTotal";
        string amount = "1";
        W3AWalletUtils.outgoingContract = contractAddress;
        var contract = new Contract(contractAbi, contractAddress);
        Debug.Log("Contract: " + contract);
        var calldata = contract.Calldata(method, new object[]
        {
            // values need to be converted to their data types in solidity
            int.Parse(amount)
        });
        Debug.Log("Contract Data: " + calldata);
        // finds the wallet, sets sign and incoming tx conditions to true and opens
        CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
        W3AWalletUtils.incomingTx = true;
        W3AWalletUtils.incomingAction = "Broadcast";
        W3AWalletUtils.incomingTxData = calldata;
        CSWallet.GetComponent<Web3AuthWallet>().OpenButton();
        print("Please check the contract variable again in a few seconds once the chain has processed the request!");
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