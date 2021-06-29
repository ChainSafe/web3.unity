using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_WEBGL
public class CallContractExample : MonoBehaviour
{
    public Text buttonTxt;
    async void OnCallContract()
    {
        // smart contract method to call
        string method = "x";
        // abi in json format
        string abi = "[ { \"inputs\": [], \"name\": \"increment\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"x\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        // address of contract
        string contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C";
        // array of arguments for contract
        string args = "[]";
        // connects to user's browser wallet to call a transaction
        string response = await Web3GL.Call(method, abi, contract, args);
        // display response in game
        buttonTxt.text = response;
    }
}
#endif