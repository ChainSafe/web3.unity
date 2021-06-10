using System.Numerics;
using UnityEngine;

public class BEP721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string contract = "0x3e855B7941fE8ef5F07DAd68C5140D6a3EC1b286";
        string account = "0xf81035dd3945ee53f5862833844b69df339c7db4";

        BigInteger balance = await BEP721.BalanceOf(network, contract, account);

        print (balance);
    }
}
