using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileSignMessageExample : MonoBehaviour
{
    public Text text;
    async public void OnSignMessage()
    {
        // string response = await Web3Mobile.Sign("hello");
        // text.text = response;
        // print(response);

        // https://chainlist.org/
        string chainId = "4"; // rinkeby
        // account to send to
        string to = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        // value in wei
        string value = "12300000000000000";
        // data OPTIONAL
        string data = "";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // send transaction
        string response = await Web3Mobile.SendTransaction(chainId, to, value, data, gasLimit, gasPrice);
        text.text = response;


        // // https://chainlist.org/
        // string chainId = "4"; // rinkeby
        // // contract to interact with 
        // string contract = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";
        // // value in wei
        // string value = "0";
        // // abi in json format
        // string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        // // smart contract method to call
        // string method = "addTotal";
        // // array of arguments for contract
        // string args = "[\"1\"]";
        // // create data to interact with smart contract
        // string data = await EVM.CreateContractData(abi, method, args);
        // // gas limit OPTIONAL
        // string gasLimit = "";
        // // gas price OPTIONAL
        // string gasPrice = "";
        // // send transaction
        // string response = await Web3Mobile.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        // text.text = response;
    }
}
