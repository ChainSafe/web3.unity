using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLSendTransactionExample : MonoBehaviour
{
    async public void OnSendTransaction()
    {
        // account to send to
        string to = "0x428066dd8A212104Bc9240dCe3cdeA3D3A0f7979";
        // amount in wei to send
        string value = "12300000000000000";
        // gas limit OPTIONAL
        string gas = "21000";
        // connects to user's browser wallet (metamask) to send a transaction
        try {
            string response = await Web3GL.SendTransaction(to, value, gas);
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
