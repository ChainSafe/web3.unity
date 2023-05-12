using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Web3Unity.Scripts.Library.Web3Wallet;

public class Web3WalletSignMessageExample : MonoBehaviour
{
    async public void OnSignMessage()
    {
        string response = await Web3Wallet.Sign("hello");
        print(response);
    }
}
