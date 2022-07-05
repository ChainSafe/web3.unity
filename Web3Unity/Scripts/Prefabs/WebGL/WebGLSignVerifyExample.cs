using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_WEBGL

public class WebGLSignVerifyExample : MonoBehaviour
{
    public string message = "hello";
    public Text textHashedMessage;
    public Text textSignedHash;
    public Text verifyAddress;

    async public void OnHashMessage()
    {
        try
        {
            string hashedMessage = await Web3GL.Sha3(message);
            textHashedMessage.text = hashedMessage;
            Debug.Log("Hashed Message: " + hashedMessage);
            string signHashed = await Web3GL.Sign(hashedMessage);
            Debug.Log("Signed Hashed: " + signHashed);
            textSignedHash.text = signHashed;
            ParseSignatureFunction(signHashed);
            Task<string> verify = EVM.Verify(hashedMessage, signHashed);
            verifyAddress.text = await verify;
            Debug.Log("Verify Address: " + verifyAddress.text);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
    public void ParseSignatureFunction(string sig)
    {
        string signature = sig;
        string r = signature.Substring(0, 66);
        Debug.Log("R:" + r);
        string s = "0x" + signature.Substring(66, 64);
        Debug.Log("S: " + s);
        int v = int.Parse(signature.Substring(130, 2), System.Globalization.NumberStyles.HexNumber);
        Debug.Log("V: " + v);
    }
}

#endif