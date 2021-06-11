using System.Numerics;
using UnityEngine;

public class Avalanche20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet
        string contract = "0x6b0bc2e986B0e70DB48296619A96E9ac02c5574b";

        BigInteger decimals = await Avalanche20.Decimals(network, contract);

        print (decimals);
    }
}
