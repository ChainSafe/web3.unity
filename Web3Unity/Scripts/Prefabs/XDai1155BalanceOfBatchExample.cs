using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class XDai1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
        string[] accounts =
        {
            "0xa63641e81D223F01d11343C67b77CB4f092acd5a",
            "0xa63641e81D223F01d11343C67b77CB4f092acd5a"
        };
        string[] tokenIds = { "1344", "1345" };

        List<BigInteger> batchBalances = await XDai1155.BalanceOfBatch(contract, accounts, tokenIds);

        foreach (var balance in batchBalances)
        {
            print ("batchBalance:" + balance);
        }
    }
}
