using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxStatusExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string transaction = "0xa70f93b662b2559ceef2fe83640b4cfe694f8b19be802ed0a06bae5fbbc014c2";

        string txConfirmed = await EVM.TxStatus(chain, network, transaction);
        print(txConfirmed);
    }
}
