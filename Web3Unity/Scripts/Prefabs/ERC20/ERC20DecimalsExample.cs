using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "xdai";
        string network = "mainnet";
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

        BigInteger decimals = await ERC20.Decimals(chain, network, contract);
        print(decimals);
    }
}
