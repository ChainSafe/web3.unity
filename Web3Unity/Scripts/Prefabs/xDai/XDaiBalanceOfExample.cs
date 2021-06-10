using System.Numerics;
using UnityEngine;

public class XDaiBalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string account = "0x577b17c9A02B7A360e0cf945D623D6C1ace6074c";

        BigInteger balance = await XDai.BalanceOf(account);
        
        print(balance);
    }
}
