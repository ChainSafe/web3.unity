using System.Numerics;
using UnityEngine;

public class BEP20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet 
        string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";

        BigInteger decimals = await BEP20.Decimals(network, contract);

        print (decimals);
    }
}
