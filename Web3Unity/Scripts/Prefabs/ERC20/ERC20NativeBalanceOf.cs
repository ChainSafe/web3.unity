using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class ERC20NativeBalanceOf : MonoBehaviour
{
    async void Start()
    {
        string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
        var provider = new JsonRpcProvider("YOUR_NODE_HERE");
        var getBalance = await provider.GetBalance(account);
        Debug.Log("Account Balance: " + getBalance);
    }
}