using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLSendContractExample : MonoBehaviour
{
    async public void OnSendContract()
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
        // gas limit OPTIONAL
        string gas = "21000";
        // connects to user's browser wallet (metamask) to update contract state
        try {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gas);
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}