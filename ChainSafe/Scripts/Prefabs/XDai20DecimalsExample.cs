using System.Numerics;
using UnityEngine;

public class XDai20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

        BigInteger decimals = await XDai20.Decimals(contract);

        print (decimals);
    }
}
