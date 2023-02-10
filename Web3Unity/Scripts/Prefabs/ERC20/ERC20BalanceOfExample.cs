using System.Numerics;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

        BigInteger balanceOf = await ERC20.BalanceOf(contract, account);
        Debug.Log("Balance Of: " + balanceOf);
    }
}
