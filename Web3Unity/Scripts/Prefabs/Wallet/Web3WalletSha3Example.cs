using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web3WalletSha3Example : MonoBehaviour
{
    void Start()
    {
        string message = "hello";
        string hashedMessage = Web3Wallet.Sha3(message);
        // 0x1c8aff950685c2ed4bc3174f3472287b56d9517b9c948127319a09a7a36deac8
        print(hashedMessage);
    }
}
