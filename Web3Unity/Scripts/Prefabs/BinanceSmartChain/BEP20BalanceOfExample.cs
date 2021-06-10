using System.Numerics;
using UnityEngine;

public class BEP20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet
        string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";
        string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";

        BigInteger balance = await BEP20.BalanceOf(network, contract, account);
        
        print (balance);
    }
}
