using System.Numerics;
using UnityEngine;

public class EthereumBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
        string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";

        BigInteger balance = await Ethereum.BalanceOf(network, account);

        print(balance);
    }
}
