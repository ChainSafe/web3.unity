using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Moonbeam1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet 
        string contract = "0x6b0bc2e986B0e70DB48296619A96E9ac02c5574b";
        string[] accounts =
        {
            "0xdD4c825203f97984e7867F11eeCc813A036089D1",
            "0xdD4c825203f97984e7867F11eeCc813A036089D1"
        };
        string[] tokenIds = { "1", "2" };

        List<BigInteger> batchBalances = await Moonbeam1155.BalanceOfBatch(network, contract, accounts, tokenIds);

        foreach (var balance in batchBalances)
        {
            print ("batchBalance:" + balance);
        }
    }
}
