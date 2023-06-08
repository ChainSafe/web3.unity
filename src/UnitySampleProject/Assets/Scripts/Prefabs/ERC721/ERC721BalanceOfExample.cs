using System.Numerics;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x9123541E259125657F03D7AD2A7D1a8Ec79375BA";
        string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

        BigInteger balance = await ERC721.BalanceOf(contract, account);
        print(balance);
    }
}
