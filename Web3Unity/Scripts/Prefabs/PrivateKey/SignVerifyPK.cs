using System.Collections;
using System.Collections.Generic;
using Nethereum.Hex.HexConvertors.Extensions;
using UnityEngine;

public class SignVerifyPK : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var privateKey = "ADD_PRIVATE_KEY";
        string message = "Message";
        var hashedMessage = Web3Wallet.Sha3(message);
        Debug.Log("Hashed Message: " + hashedMessage);
        string signature = Web3PrivateKey.Sign(privateKey, message);
        print("Signature: " + signature);
        // get account from private key
        string account = Web3PrivateKey.Address(privateKey);
        print("Account from PK: " + account);
        string address = await EVM.Verify(hashedMessage, signature);
        print("Address From Verify: " + address);
        
        ParseSignatureFunction(signature);
    }

    // Update is called once per frame
    public void ParseSignatureFunction(string sig)
    {
        string signature = sig;
        string r = signature.Substring(0, 66).EnsureHexPrefix();
        Debug.Log("R:" + r);
        string s = signature.Substring(66, 64).EnsureHexPrefix();
        Debug.Log("S: " + s);
        int v = int.Parse(signature.Substring(130, 2), System.Globalization.NumberStyles.HexNumber);
        Debug.Log("V: " + v);
    }
}