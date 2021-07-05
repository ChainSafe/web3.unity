using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTxConfirmedExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        string transaction = "0x911d4ec9193e0dc14d9d034d88c311453112c5097f29c366ccc9c5e5bc7072e1";

        bool txConfirmed = await EVM.IsTxConfirmed(chain, network, transaction);
        print(txConfirmed);
    }
}
