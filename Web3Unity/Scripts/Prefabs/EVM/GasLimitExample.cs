using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLimitExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "polygon";
        string network = "mainnet";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1"; // account to send to
        string amount = "123"; // amount of wei to send
        string transaction = "0x"; // from EVM.CreateTransaction()

        string gasPrice = await EVM.GasLimit(chain, network, account, amount, transaction);
        print(gasPrice);
    }
}
