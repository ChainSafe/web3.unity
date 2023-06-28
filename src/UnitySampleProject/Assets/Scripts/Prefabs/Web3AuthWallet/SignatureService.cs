using System;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using UnityEngine;

// creates a transaction for the wallet
public class SignatureService : ISignatureService
{
    public string SignTransaction(string privateKey, string transaction, string chainId)
    {
        Debug.Log("PK" + privateKey);
        Debug.Log("TX" + transaction);
        Debug.Log("ChaibId" + chainId);
        int int32 = Convert.ToInt32(chainId);
        var ethEcKey = new EthECKey(privateKey);
        var byteArray = transaction.HexToByteArray();
        var _chainId = BigInteger.Parse(chainId);
        return int32 == 137 || int32 == 80001 || int32 == 1666600000 || int32 == 1666700000 || int32 == 25 || int32 == 338 || int32 == 250 || int32 == 4002 || int32 == 43114 || int32 == 43113 ? EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateYParityV(byteArray)) : EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateV(byteArray, _chainId));
    }

    // used to sign a message with a users private key stored in memory
    public string SignMsgW3A(string _privateKey, string _message) => new EthereumMessageSigner().HashAndSign(_message, _privateKey);
}