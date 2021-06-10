using System.Numerics;
using UnityEngine;

public class ERC721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0x60f80121c31a0d46b5279700f9df786054aa5ee5";
        string account = "0x6b2be2106a7e883f282e2ea8e203f516ec5b77f7";

        BigInteger balance = await ERC721.BalanceOf(network, contract, account);

        print (balance);
    }
}
