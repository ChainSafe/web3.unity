using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPriceExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "polygon";
        string network = "mainnet";

        string gasPrice = await EVM.GasPrice(chain, network);
        print(gasPrice);
    }
}
