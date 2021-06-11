using System.Numerics;
using UnityEngine;

public class Avalanche1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet 
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        string tokenId = "1";

        BigInteger balance = await Avalanche1155.BalanceOf(network, contract, account, tokenId);

        print (balance);
    }
}
