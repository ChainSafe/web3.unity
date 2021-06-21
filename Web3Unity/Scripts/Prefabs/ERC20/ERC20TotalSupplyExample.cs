using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20TotalSupplyExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        string contract = "0x6b3595068778dd592e39a122f4f5a5cf09c90fe2";

        BigInteger totalSupply = await ERC20.TotalSupply(chain, network, contract);
        print(totalSupply);
    }
}
