using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0xdc4aff511e1b94677142a43df90f948f9ae181dd";
        string[] accounts = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a" };
        string[] tokenIds = { "1", "2" };

        List<BigInteger> batchBalances = await ERC1155.BalanceOfBatch(contract, accounts, tokenIds);
        foreach (var balance in batchBalances)
        {
            print("BalanceOfBatch: " + balance);
        }
    }
}
