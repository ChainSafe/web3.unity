using System.Numerics;
using UnityEngine;

public class XDai20TotalSupplyExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

        BigInteger totalSupply = await XDai20.TotalSupply(contract);

        print (totalSupply);
    }
}
