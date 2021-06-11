using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Avalanche1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet 
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";
        string[] accounts =
        {
            "0xdD4c825203f97984e7867F11eeCc813A036089D1",
            "0xdD4c825203f97984e7867F11eeCc813A036089D1"
        };
        string[] tokenIds = { "1", "2" };

        List<BigInteger> batchBalances = await Avalanche1155.BalanceOfBatch(network, contract, accounts, tokenIds);

        foreach (var balance in batchBalances)
        {
            print ("batchBalance:" + balance);
        }
    }
}
