using System;
using System.Numerics;
using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class TransferW3A : MonoBehaviour
{
    public Text responseText;
    public string contractAddress;
    public string toAccount = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
    public string amount = "1000000000000000000";
    public string contractAbi;
    private GameObject CSWallet = null;
    private Web3AuthWalletConfig _web3AuthWalletConfig;
    private Web3AuthWallet _web3AuthWallet;
    private Web3 _web3;

    async private void Awake()
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
            CSWallet.GetComponent<Web3AuthWalletUI>().OpenButton();
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
            var contract = new Contract(contractAbi, contractAddress, _web3.RpcProvider);
            Debug.Log("Contract: " + contract);
            Debug.Log("Account: " + W3AWalletUtils.account);
            var calldata = contract.Calldata(method, new object[]
            {
                "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0",
                1
            });
            Debug.Log("Contract Data: " + calldata);

            // finds the wallet, sets sign and incoming tx conditions to true and opens
            CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
            W3AWalletUtils.incomingTx = true;
            W3AWalletUtils.incomingAction = "Broadcast";
            W3AWalletUtils.incomingTxData = calldata;
            CSWallet.GetComponent<Web3AuthWalletUI>().OpenButton();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    void Update()
    {
        if (W3AWalletUtils.signedTxResponse != string.Empty)
        {
            // display signed tx response from wallet
            responseText.text = W3AWalletUtils.signedTxResponse;
            W3AWalletUtils.signedTxResponse = string.Empty;
        }
    }
}