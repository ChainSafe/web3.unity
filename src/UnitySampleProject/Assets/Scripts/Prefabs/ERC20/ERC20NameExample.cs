using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage.Ethereum.Eip;
using UnityEngine;

public class ERC20NameExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        string name = await ERC20.Name(contract);
        print(name);
    }
}
