using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WEBGL
public class WebGLSha3Example : MonoBehaviour
{
    // todo rework with new architecture in mind
    async public void OnHashMessage()
    {
        // try
        // {
        //     string message = "hello";
        //     string hashedMessage = await Web3GL.Sha3(message);
        //     Debug.Log("Hashed Message :" + hashedMessage);
        // }
        // catch (Exception e)
        // {
        //     Debug.LogException(e, this);
        // }
    }
}
#endif