using System.Numerics;
using UnityEngine;

public class PolygonBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string account = "0x99C85bb64564D9eF9A99621301f22C9993Cb89E3";

        BigInteger balance = await Polygon.BalanceOf(network, account);
        
        print(balance);
    }
}
