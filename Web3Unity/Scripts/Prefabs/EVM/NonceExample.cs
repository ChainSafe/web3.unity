using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonceExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        string nonce = await EVM.Nonce(chain, network, account);
        print(nonce);
    }
}
