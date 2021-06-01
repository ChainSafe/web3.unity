using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Polygon1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet 
        string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
        string[] accounts =
        {
            "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4",
            "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4"
        };
        string[] tokenIds = { "141", "142" };

        List<BigInteger> batchBalances = await Polygon1155.BalanceOfBatch(network, contract, accounts, tokenIds);

        foreach (var balance in batchBalances)
        {
            print ("batchBalance:" + balance);
        }
    }
}
