using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Prefabs.Web3AuthWallet.UI;
using Scripts.Web3AuthWallet;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using UnityEngine.UI;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
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
    private Web3AuthWalletConfig _web3AuthWalletConfig;
    private Web3AuthWallet _web3AuthWallet;
    private Web3 _web3;

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
    private async void Awake()
    {
        var projectConfig = ProjectConfigUtilities.Load();
        _web3 = await new Web3Builder(projectConfig)
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseJsonRpcProvider();
                services.UseWeb3AuthWallet();
            })
            .BuildAsync();
        _web3AuthWallet = new Web3AuthWallet(_web3.RpcProvider, _web3AuthWalletConfig);
        _web3AuthWalletConfig = new Web3AuthWalletConfig();
    }

    public void OnEnable()
    {
        // resets response text
        responseText.text = string.Empty;
    }

    public async void CheckVariable()
    {
        string method = "myTotal";

        // you can use this to create a provider for hardcoding and parse this instead of rpc get instance
        var contract = new Contract(contractAbi, contractAddress, _web3.RpcProvider);
        Debug.Log("Contract: " + contract);
        var calldata = await contract.Call(method);
        Debug.Log("Contract Data: " + calldata[0]);

        // display response in game
        print("Contract Variable Total: " + calldata[0]);
        responseText.text = "Contract Variable Total: " + calldata[0];
    }

    public void AddOneToVariable()
    {
        string method = "addTotal";
        string amount = "1";
        W3AWalletUtils.OutgoingContract = contractAddress;
        var contract = new Contract(contractAbi, contractAddress, _web3.RpcProvider);
        Debug.Log("Contract: " + contract);
        var calldata = contract.Calldata(method, new object[]
        {
            // values need to be converted to their data types in solidity
            int.Parse(amount),
        });
        Debug.Log("Contract Data: " + calldata);

        // finds the wallet, sets sign and incoming tx conditions to true and opens
        CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
        W3AWalletUtils.IncomingTx = true;
        W3AWalletUtils.IncomingAction = "Broadcast";
        W3AWalletUtils.IncomingTxData = calldata;
        CSWallet.GetComponent<Web3AuthWalletUI>().OpenButton();
        print("Please check the contract variable again in a few seconds once the chain has processed the request!");
    }

    void Update()
    {
        if (W3AWalletUtils.SignedTxResponse != string.Empty)
        {
            // display signed tx response from wallet
            responseText.text = W3AWalletUtils.SignedTxResponse;
            W3AWalletUtils.SignedTxResponse = string.Empty;
        }
    }
}