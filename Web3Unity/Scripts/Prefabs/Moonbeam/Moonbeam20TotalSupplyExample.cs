using System.Numerics;
using UnityEngine;

public class Moonbeam20TotalSupplyExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet 
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";

        BigInteger totalSupply = await Moonbeam20.TotalSupply(network, contract);

        print (totalSupply);
    }
}
