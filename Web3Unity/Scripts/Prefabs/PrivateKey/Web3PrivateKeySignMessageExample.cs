using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web3PrivateKeySignMessageExample : MonoBehaviour
{
    void Start()
    {
        string privateKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
        string message = "hello";
        string response = Web3PrivateKey.Sign(privateKey, message);
        print(response);
    }
}
