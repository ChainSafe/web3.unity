using System;
using System.Threading.Tasks;
using UnityEngine;

public class Web3Mobile
{
    #if UNITY_ANDROID && UNITY_IOS
        private static string url = "https://metamask.app.link/dapp/chainsafe.github.io/game-web3mobile/";
    #else
        private static string url = "https://chainsafe.github.io/game-web3mobile/";
    #endif

    public static async Task<string> SendTransaction(string _chainId, string _to, string _value, string _data = "", string _gasLimit = "", string _gasPrice = "")
    {
        // open application
        Application.OpenURL(url + "?action=send" + "&chainId=" + _chainId + "&to=" + _to + "&value=" + _value + "&data=" + _data + "&gasLimit=" + _gasLimit + "&gasPrice=" + _gasPrice);
        // set clipboard to empty
        GUIUtility.systemCopyBuffer = "";
        // wait for clipboard response
        string clipBoard = "";
        while (clipBoard == "")
        {
            clipBoard = GUIUtility.systemCopyBuffer;
            await Task.Delay(100);
        }
        // check if clipboard response is valid
        if (clipBoard.StartsWith("0x") && clipBoard.Length == 66)
        {
            return clipBoard;
        }
        else
        {
            throw new Exception("transaction error");
        }
    }

    public static async Task<string> Sign(string _message)
    {
        // open application
        string message = Uri.EscapeDataString(_message);
        Application.OpenURL(url + "?action=sign" + "&message=" + message);
        // set clipboard to empty
        GUIUtility.systemCopyBuffer = "";
        // wait for clipboard response
        string clipBoard = "";
        while (clipBoard == "")
        {
            clipBoard = GUIUtility.systemCopyBuffer;
            await Task.Delay(100);
        }
        // check if clipboard response is valid
        if (clipBoard.StartsWith("0x") && clipBoard.Length == 132)
        {
            return clipBoard;
        }
        else
        {
            throw new Exception("sign error");
        }   
    }
}
