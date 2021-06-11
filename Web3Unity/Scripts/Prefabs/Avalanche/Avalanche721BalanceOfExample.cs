using System.Numerics;
using UnityEngine;

public class Avalanche721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet
        string contract = "0xcB0cbcE06860f6C30C62560f5eFBF918150e056E";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        BigInteger balance = await Avalanche721.BalanceOf(network, contract, account);

        print (balance);
    }
}
