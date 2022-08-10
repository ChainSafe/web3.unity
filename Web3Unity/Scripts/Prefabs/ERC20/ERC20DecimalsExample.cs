using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        string contract = "0xdAC17F958D2ee523a2206206994597C13D831ec7";

        BigInteger decimals = await ERC20.Decimals(chain, network, contract);
        print(decimals);
    }
}
