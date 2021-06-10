using System.Numerics;
using UnityEngine;

public class Polygon1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet 
        string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
        string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";
        string tokenId = "141";

        BigInteger balance = await Polygon1155.BalanceOf(network, contract, account, tokenId);

        print (balance);
    }
}
