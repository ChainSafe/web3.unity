using System.Numerics;
using UnityEngine;

public class Moonbeam20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        BigInteger balance = await Moonbeam20.BalanceOf(network, contract, account);
        
        print (balance);
    }
}
