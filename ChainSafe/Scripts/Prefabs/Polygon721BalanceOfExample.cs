using System.Numerics;
using UnityEngine;

public class Polygon721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string contract = "0xbCCaa7ACb552A2c7eb27C7eb77c2CC99580735b9";
        string account = "0x8861399ee37626fcc020c49e5184d9b839ed854a";

        BigInteger balance = await Polygon721.BalanceOf(network, contract, account);

        print (balance);
    }
}
