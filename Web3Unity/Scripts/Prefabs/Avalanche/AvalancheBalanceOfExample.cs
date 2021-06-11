using System.Numerics;
using UnityEngine;

public class AvalancheBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        BigInteger balance = await Avalanche.BalanceOf(network, account);
        
        print(balance);
    }
}
