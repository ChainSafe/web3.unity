using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRPCExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "rootstock";
        string network = "testnet"; 
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        string rpc = "https://public-node.testnet.rsk.co";

        string balance = await EVM.BalanceOf(chain, network, account, rpc);
        print(balance);
    }
}
