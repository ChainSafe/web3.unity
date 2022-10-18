using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Web3PrivateKeySend721Example : MonoBehaviour
{
    async public void OnSend721()
    {
        // private key of account
        string privateKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
        // set chain: ethereum, moonbeam, polygon etc
        string chain = "ethereum";
        // set network mainnet, testnet
        string network = "goerli";
        // smart contract method to call
        string method = "safeTransferFrom";
        // account of player 
        string account = Web3PrivateKey.Address(privateKey);
        // ERC-721 contract address
        string contract = "0xae70a9accf2e0c16b380c0aa3060e9fba6718daf";
        // account to send to
        string toAccount = "0x428066dd8A212104Bc9240dCe3cdeA3D3A0f7979";
        // ERC-721 token id 
        string tokenId = "2543";
        // amount of wei to send
        string value = "0";
        // abi to interact with contract
        string abi = "[{ \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"_data\", \"type\": \"bytes\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }]";
        string rpc = "";

        // array of arguments for contract
        string[] obj = { account, toAccount, tokenId, "0x" };
        string args = JsonConvert.SerializeObject(obj);
        string chainId = await EVM.ChainId(chain, network, rpc);
        string gasPrice = await EVM.GasPrice(chain, network, rpc);
        string data = await EVM.CreateContractData(abi, method, args);
        string gasLimit = "75000";
        string transaction = await EVM.CreateTransaction(chain, network, account, contract, value, data, gasPrice, gasLimit, rpc);
        string signature = Web3PrivateKey.SignTransaction(privateKey, transaction, chainId);
        string response = await EVM.BroadcastTransaction(chain, network, account, contract, value, data, signature, gasPrice, gasLimit, rpc);
        print(response);
        Application.OpenURL("https://goerli.etherscan.io/tx/" + response);
    }
}