using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "xdai";
        string network = "mainnet";
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";
        string account = "0x000000ea89990a17Ec07a35Ac2BBb02214C50152";

        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        print(balanceOf); 
    }
}
