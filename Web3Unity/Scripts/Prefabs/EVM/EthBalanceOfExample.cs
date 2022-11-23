using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "goerli"; // mainnet goerli
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        string balance = await EVM.BalanceOf(chain, network, account);
        print(balance);
    }
}
