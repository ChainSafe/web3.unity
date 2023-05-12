using System.Numerics;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x2c1867bc3026178a47a677513746dcc6822a137a";
        string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
        string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";

        BigInteger balanceOf = await ERC1155.BalanceOf(contract, account, tokenId);
        print(balanceOf);
    }
}
