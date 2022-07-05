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
    private static extern void SendContractJs(string method, string abi, string contract, string args, string value, string gasLimit, string gasPrice);

    [DllImport("__Internal")]
    private static extern string SendContractResponse();

    [DllImport("__Internal")]
    private static extern void SetContractResponse(string value);

    [DllImport("__Internal")]
    private static extern void SendTransactionJs(string to, string value, string gasLimit, string gasPrice);
    
    [DllImport("__Internal")]
    private static extern void SendTransactionJsData(string to, string value, string gasPrice, string gasLimit, string data);

    [DllImport("__Internal")]
    private static extern string SendTransactionResponse();

    [DllImport("__Internal")]
    private static extern void SetTransactionResponse(string value);
    
    [DllImport("__Internal")]
    private static extern void SetTransactionResponseData(string value);

    [DllImport("__Internal")]
    private static extern void SignMessage(string value);
    
    
    [DllImport("__Internal")]
    private static extern void HashMessage(string value);

    [DllImport("__Internal")]
    private static extern string SignMessageResponse();
    
    [DllImport("__Internal")]
    private static extern string HashMessageResponse();

    [DllImport("__Internal")]
    private static extern void SetSignMessageResponse(string value);
    
    [DllImport("__Internal")]
    private static extern void SetHashMessageResponse(string value);

    [DllImport("__Internal")]
    private static extern int GetNetwork();

    // this function will create a metamask tx for user to confirm.
    async public static Task<string> SendContract(string _method, string _abi, string _contract, string _args, string _value, string _gasLimit = "", string _gasPrice = "")
    {
        // Set response to empty
        SetContractResponse("");
        SendContractJs(_method, _abi, _contract, _args, _value, _gasLimit, _gasPrice);
        string response = SendContractResponse();
        while (response == "")
        {
            await new WaitForSeconds(1f);
            response = SendContractResponse();
        }
        SetContractResponse("");
        // check if user submmited or user rejected
        if (response.Length == 66) 
        {
            return response;
        } 
        else 
        {
            throw new Exception(response);
        }
    }

    async public static Task<string> SendTransaction(string _to, string _value, string _gasLimit = "", string _gasPrice = "")
    {
        // Set response to empty
        SetTransactionResponse("");
        SendTransactionJs(_to, _value, _gasLimit, _gasPrice);
        string response = SendTransactionResponse();
        while (response == "")
        {
            await new WaitForSeconds(1f);
            response = SendTransactionResponse();
        }
        SetTransactionResponse("");
        // check if user submmited or user rejected
        if (response.Length == 66) 
        {
            return response;
        } 
        else 
        {
            throw new Exception(response);
        }
    }
    
    async public static Task<string> SendTransactionData(string _to, string _value,  string _gasPrice = "",string _gasLimit = "", string _data = "")
    {
        // Set response to empty
        SetTransactionResponse("");
        SendTransactionJsData(_to, _value, _gasPrice,_gasLimit,_data);
        string response = SendTransactionResponse();
        while (response == "")
        {
            await new WaitForSeconds(1f);
            response = SendTransactionResponse();
        }
        SetTransactionResponse("");
        // check if user submmited or user rejected
        if (response.Length == 66) 
        {
            return response;
        } 
        else 
        {
            throw new Exception(response);
        }
    }

    async public static Task<string> Sign(string _message)
    {
        SignMessage(_message);
        string response = SignMessageResponse();
        while (response == "")
        {
            await new WaitForSeconds(1f);
            response = SignMessageResponse();
        }
        // Set response to empty
        SetSignMessageResponse("");
        // check if user submmited or user rejected
        if (response.Length == 132)
        {
            return response;
        } 
        else 
        {
            throw new Exception(response);
        }
    }
    
    async public static Task<string> Sha3(string _message)
    {
        HashMessage(_message);
        SetHashMessageResponse("");
        try
        {
            await new WaitForSeconds(1f);
            string response = HashMessageResponse();
            return response;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public static int Network()
    {
        return GetNetwork();
    }

}
#endif
