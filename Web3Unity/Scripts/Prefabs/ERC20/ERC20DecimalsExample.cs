using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

        BigInteger decimals = await ERC20.Decimals(contract);
        print(decimals);
    }
}
