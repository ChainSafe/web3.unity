using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileSignMessageExample : MonoBehaviour
{
    async public void OnSignMessage()
    {
        string response = await Web3Mobile.Sign("hello");
        print(response);
    }
}
