using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

public class Web3GL
{

    /*
    ONLY WORKS FOR WEBGL BUILDS
    */

    [DllImport("__Internal")]
    private static extern void SendContract(string method, string abi, string contract, string args, string value);

    [DllImport("__Internal")]
    private static extern void CallContract(string method, string abi, string contract, string args);

    [DllImport("__Internal")]
    private static extern string CallResponse();

    // this function will create a metamask tx for user to confirm.
    public static void Send(string _method, string _abi, string _contract, string _args, string _value)
    {
        SendContract(_method, _abi, _contract, _args, _value);
    }

    public static async Task<string> Call(string _method, string _abi, string _contract, string _args)
    {
        CallContract(_method, _abi, _contract, _args);
        // pause then fetch response
        await new WaitForSeconds(1.5f);
        string response = CallResponse();
        return response;
    }

}
