using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC20TotalSupplyExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        BigInteger totalSupply = await ERC20.TotalSupply(contract);
        print(totalSupply);
    }
}
