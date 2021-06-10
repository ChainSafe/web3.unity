using System.Numerics;
using UnityEngine;

public class BinanceBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet 
        string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";

        BigInteger balance = await Binance.BalanceOf(network, account);
        
        print(balance);
    }
}
