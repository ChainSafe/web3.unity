using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC20NameExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "xdai";
        string network = "mainnet";
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

        string name = await ERC20.Name(chain, network, contract);
        print(name); 
    }
}
