using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNumberExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet"; // mainnet kovan goerli
        int blockNumber = await EVM.BlockNumber(chain, network);
        print(blockNumber);
    }
}
