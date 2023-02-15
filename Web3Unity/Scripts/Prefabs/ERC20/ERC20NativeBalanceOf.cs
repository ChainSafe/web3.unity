using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class ERC20NativeBalanceOf : MonoBehaviour
{
    async void Start()
    {
        string account = "0xEDf117cd77C5323f3f21Fc1698E67688b4B8Af8b";
        var provider = new JsonRpcProvider("YOUR_NODE_HERE");
        var getBalance = await provider.GetBalance(account);
        Debug.Log("Account Balance: " + getBalance);
    }
}