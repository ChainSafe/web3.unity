using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Web3WalletTransfer721Example : MonoBehaviour
{
    async public void OnTransfer721()
    {
        // https://chainlist.org/
        string chainId = "5"; // goerli
        // contract to interact with 
        string contract = "0xde458cd3deaa28ce67beefe3f45368c875b3ffd6";
        // value in wei
        string value = "0";
        // abi in json format
        string abi = "[{ \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }]"; 
        // smart contract method to call
        string method = "safeTransferFrom";
        // account to send erc721 to
        string toAccount = PlayerPrefs.GetString("Account");
        // token id to send
        string tokenId = "5";
        // array of arguments for contract
        string[] obj = { PlayerPrefs.GetString("Account"), toAccount, tokenId };
        string args = JsonConvert.SerializeObject(obj);
        // create data to interact with smart contract
        string data = await EVM.CreateContractData(abi, method, args);
        print(data);
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // send transaction
        string response = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        print(response);
    }
}
