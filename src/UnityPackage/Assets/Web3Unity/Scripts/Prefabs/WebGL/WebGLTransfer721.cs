using System;
using UnityEngine;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Prefabs;

#if UNITY_WEBGL
public class WebGLTransfer721 : MonoBehaviour
{
    [SerializeField]
    private string contract = "0xdE458Cd3dEaA28ce67BeEFE3F45368c875b3FfD6";
    [SerializeField]
    private string toAccount = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
    [SerializeField]
    private string tokenId = "1";
    private readonly string abi = ABI.ERC_721;

    async public void SafeTransferFrom()
    {
        // smart contract method to call
        string method = "safeTransferFrom";
        // array of arguments for contract
        string[] obj = { PlayerPrefs.GetString("Account"), toAccount, tokenId };
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