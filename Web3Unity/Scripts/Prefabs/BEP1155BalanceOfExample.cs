using System.Numerics;
using UnityEngine;

public class BEP1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet 
        string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
        string account = "0xe91e3b8b25f41b215645813a33e39edf42ba25cf";
        string tokenId = "770";

        BigInteger balance = await BEP1155.BalanceOf(network, contract, account, tokenId);

        print (balance);
    }
}
