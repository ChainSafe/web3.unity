using System.Numerics;
using UnityEngine;

public class XDai1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
        string account = "0xa63641e81D223F01d11343C67b77CB4f092acd5a";
        string tokenId = "1344";

        BigInteger balance = await XDai1155.BalanceOf(contract, account, tokenId);

        print (balance);
    }
}
