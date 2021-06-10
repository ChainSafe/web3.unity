using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ERC1155BalanceOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
        string[] accounts =
        {
            "0xaCA9B6D9B1636D99156bB12825c75De1E5a58870",
            "0xaCA9B6D9B1636D99156bB12825c75De1E5a58870"
        };
        string[] tokenIds = { "17", "22" };

        List<BigInteger> batchBalances = await ERC1155.BalanceOfBatch(network, contract, accounts, tokenIds);

        foreach (var balance in batchBalances)
        {
            print ("batchBalance:" + balance);
        }
    }
}
