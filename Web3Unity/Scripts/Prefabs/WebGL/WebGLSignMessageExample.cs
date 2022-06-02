using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WEBGL
public class WebGLSignMessageExample : MonoBehaviour
{
    async public void OnSignMessage()
    {
        try {
            string message = "hello";
            string response = await Web3GL.Sign(message);
            print(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
#endif