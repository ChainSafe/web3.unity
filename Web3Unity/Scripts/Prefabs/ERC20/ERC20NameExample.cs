using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC20NameExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        string contract = "0xdAC17F958D2ee523a2206206994597C13D831ec7";

        string name = await ERC20.Name(chain, network, contract);
        print(name); 
    }
}
