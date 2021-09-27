using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLSignMessageExample : MonoBehaviour
{
    async public void OnSignMessage()
    {
        try {
            string message = "hello";
            string response = await Web3GL.Sign(message);
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}