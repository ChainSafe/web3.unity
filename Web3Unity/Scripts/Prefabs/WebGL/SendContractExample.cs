using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendContractExample : MonoBehaviour
{
    public void OnSendContract()
    {
        // smart contract method to call
        string method = "increment";
        // abi in json format
        string abi = "[ { \"inputs\": [], \"name\": \"increment\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"x\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        // address of contract
        string contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C";
        // array of arguments for contract
        string args = "[]";
        // value in wei
        string value = "0";
        // connects to user's browser wallet (metamask) to send a transaction
        Web3GL.Send(method, abi, contract, args, value);
    }
}
