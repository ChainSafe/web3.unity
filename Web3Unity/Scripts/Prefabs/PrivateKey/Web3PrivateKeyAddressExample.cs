using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web3PrivateKeyAddressExample : MonoBehaviour
{
    void Start()
    {
        // private key of account
        string privateKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
        // get account from private key
        string account = Web3PrivateKey.Address(privateKey);
        print("Account: " + account);
    }
}
