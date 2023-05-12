using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Prefabs;

#if UNITY_WEBGL
public class WebGLTransfer1155 : MonoBehaviour
{
    [SerializeField]
    private string contract = "0x8207ba6852ee561f0786e2d876b1a20fef916e46";
    [SerializeField]
    private string toAccount = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
    [SerializeField]
    private string tokenId = "0";
    [SerializeField]
    private string amount = "1";
    private readonly string abi = ABI.ERC_1155;

    async public void SafeTransferFrom()
    {
        // smart contract method to call
        string method = "safeTransferFrom";
        // array of arguments for contract
        string[] obj = { PlayerPrefs.GetString("Account"), toAccount, tokenId, amount, "0x" };
        string args = JsonConvert.SerializeObject(obj);
        // value in wei
        string value = "0";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to send a transaction
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        };
    }
}
#endif