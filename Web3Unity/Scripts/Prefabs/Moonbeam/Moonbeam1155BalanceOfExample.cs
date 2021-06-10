using System.Numerics;
using UnityEngine;

public class Moonbeam1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet 
        string contract = "0x6b0bc2e986B0e70DB48296619A96E9ac02c5574b";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        string tokenId = "1";

        BigInteger balance = await Moonbeam1155.BalanceOf(network, contract, account, tokenId);

        print (balance);
    }
}
