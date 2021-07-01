using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_WEBGL
public class Web3GL
{
    [DllImport("__Internal")]
    private static extern void SendContract(string method, string abi, string contract, string args, string value);

    [DllImport("__Internal")]
    private static extern string SendContractResponse();

    [DllImport("__Internal")]
    private static extern void SetContractResponse(string value);

    // this function will create a metamask tx for user to confirm.
    async public static Task<string> Send(string _method, string _abi, string _contract, string _args, string _value)
    {
        SendContract(_method, _abi, _contract, _args, _value);
        string response = SendContractResponse();
        while (response == "") {
            await new WaitForSeconds(1f);
            response = SendContractResponse();
        }
        // Set response to empty
        SetContractResponse("");
        // check if user submmited or user rejected
        if (response.Length == 66) {
            return response;
        } else {
            throw new Exception(response);
        }
    }

}
#endif