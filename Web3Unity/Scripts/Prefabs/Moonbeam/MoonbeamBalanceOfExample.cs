using System.Numerics;
using UnityEngine;

public class MoonbeamBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet 
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        BigInteger balance = await Moonbeam.BalanceOf(network, account);
        
        print(balance);
    }
}
