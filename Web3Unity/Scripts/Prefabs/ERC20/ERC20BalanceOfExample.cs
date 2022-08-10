using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        string contract = "0xdAC17F958D2ee523a2206206994597C13D831ec7";
        string account = "0x000000ea89990a17Ec07a35Ac2BBb02214C50152";

        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        print(balanceOf); 
    }
}
