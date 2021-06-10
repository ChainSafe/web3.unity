using System.Numerics;
using UnityEngine;

public class XDai20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";
        string account = "0x000000ea89990a17Ec07a35Ac2BBb02214C50152";

        BigInteger balance = await XDai20.BalanceOf(contract, account);
        
        print (balance);
    }
}
