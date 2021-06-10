using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BEP1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet 
        string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
        string[] accounts =
        {
            "0xe91e3b8b25f41b215645813a33e39edf42ba25cf",
            "0xe91e3b8b25f41b215645813a33e39edf42ba25cf"
        };
        string[] tokenIds = { "770", "771" };

        List<BigInteger> batchBalances = await BEP1155.BalanceOfBatch(network, contract, accounts, tokenIds);

        foreach (var balance in batchBalances)
        {
            print ("batchBalance:" + balance);
        }
    }
}
