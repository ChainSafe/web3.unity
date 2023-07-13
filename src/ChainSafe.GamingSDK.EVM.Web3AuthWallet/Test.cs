using System;
using System.Numerics;
using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Prefabs.Web3AuthWallet.UI;
using Prefabs.Web3AuthWallet.Utils;
using Scripts.Web3AuthWallet;
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
    private Web3AuthWalletConfig _web3AuthWalletConfig;
    private Web3AuthWallet _web3AuthWallet;
    private Web3 _web3;
    private EthereumService _ethereumService;


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
        _ethereumService = new EthereumService(W3AWalletUtils.PrivateKey, projectConfig.Rpc);
    }

    public void OnEnable()
    {
        // resets response text
        responseText.text = string.Empty;
    }

    public async void Transfer()
    {
        // smart contract method to call
        string method = "transfer";
        W3AWalletUtils.OutgoingContract = contractAddress;

        // connects to user's wallet to send a transaction
        try
        {
            // connects to user's wallet to call a transaction
            var contract = new Contract(contractAbi, contractAddress);
            Debug.Log("Contract: " + contract);
            var calldata = contract.Calldata(method, new object[]
            {
                toAccount,
                BigInteger.Parse(amount),
            });
            Debug.Log("Contract Data: " + calldata);
            TransactionInput txInput = new TransactionInput
            {
                To = toAccount,
                From = _ethereumService.GetAddressW3A(W3AWalletUtils.PrivateKey),
                Value = new HexBigInteger(0), // Convert the Ether amount to Wei
                Data = calldata,
                GasPrice = new HexBigInteger(100000),
                Gas = new HexBigInteger(100000),
            };

            var signedTransactionData = await _ethereumService.CreateAndSignTransactionAsync(txInput);
            Debug.Log($"Signed transaction data: {signedTransactionData}");
            var transactionHash = await _ethereumService.SendTransactionAsync(signedTransactionData);
            Debug.Log($"Transaction hash: {transactionHash}");
            responseText.text = transactionHash;
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public async void Mint()
    {
        // smart contract method to call
        string method = "mint";
        W3AWalletUtils.OutgoingContract = contractAddress;

        // connects to user's wallet to send a transaction
        try
        {
            var contract = new Contract(contractAbi, contractAddress, _web3.RpcProvider);
            Debug.Log("Contract: " + contract);
            Debug.Log("Account: " + _ethereumService.GetAddressW3A(W3AWalletUtils.PrivateKey));
            var calldata = contract.Calldata(method, new object[]
            {
                _ethereumService.GetAddressW3A(W3AWalletUtils.PrivateKey),
                BigInteger.Parse(amount),
            });
            
            TransactionInput txInput = new TransactionInput
            {
                To = contractAddress,
                From = _ethereumService.GetAddressW3A(W3AWalletUtils.PrivateKey),
                Value = new HexBigInteger(0), // Convert the Ether amount to Wei
                Data = calldata,
                GasPrice = new HexBigInteger(100000),
                Gas = new HexBigInteger(100000),
            };
            
            var signedTransactionData = await _ethereumService.CreateAndSignTransactionAsync(txInput);
            Debug.Log($"Signed transaction data: {signedTransactionData}");
            var transactionHash = await _ethereumService.SendTransactionAsync(signedTransactionData);
            Debug.Log($"Transaction hash: {transactionHash}");
            responseText.text = transactionHash;
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}