using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Prefabs;

#if UNITY_WEBGL
public class WebGLTransfer20 : MonoBehaviour
{
    [SerializeField]
    private string contract = "0xc47cB02956F0c735f1136386B0B684e2CE5635dD";
    [SerializeField]
    private string toAccount = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
    [SerializeField]
    private string amount = "1000000000000000000";

    private readonly string abi = ABI.ERC_20;

    async public void Transfer()
    {
        // smart contract method to call
        string method = "transfer";
        // array of arguments for contract
        string[] obj = { toAccount, amount };
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
        }
    }
}
#endif