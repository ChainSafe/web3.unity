using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_WEBGL
public class Web3GLLight
{
    [DllImport("__Internal")]
    private static extern void SetSendAsyncResponse(string value);
    
    [DllImport("__Internal")]
    private static extern string SendAsyncResponse();
    
    [DllImport("__Internal")]
    private static extern void SetSendAsyncError(string value);
    
    [DllImport("__Internal")]
    private static extern string SendAsyncError();
    
    [DllImport("__Internal")]
    private static extern void SendAsyncJs(string method, string parameters);

    public static async Task<string> SendAsync(string method, string parameters)
    {
        // Emptied response/error
        SetSendAsyncResponse("");
        SetSendAsyncError("");
        
        SendAsyncJs(method, parameters);
        var response = SendAsyncResponse();
        var error = SendAsyncError();
        
        while (response == "" && error == "")
        {
            await new WaitForSeconds(0.1f);
            response = SendAsyncResponse();
            error = SendAsyncError();
        }
        
        SetSendAsyncResponse("");
        SetSendAsyncError("");
        
        if (error != "")
        {
            throw new Exception(error);
        }
        
        return response;
    }

}
#endif
