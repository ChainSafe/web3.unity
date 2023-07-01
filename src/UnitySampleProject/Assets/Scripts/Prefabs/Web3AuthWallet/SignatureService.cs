using System;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using UnityEngine;

// creates a transaction for the wallet
public class SignatureService : ITransactionSigner , IMessageSigner
{
    public string SignTransaction(string privateKey, string transaction)
    {
        var projectConfigSo = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        Debug.Log("PK" + privateKey);
        Debug.Log("TX" + transaction);
        int int32 = Convert.ToInt32(projectConfigSo.ChainId);
        var ethEcKey = new EthECKey(privateKey);
        var byteArray = transaction.HexToByteArray();
        var _chainId = BigInteger.Parse(projectConfigSo.ChainId);
        return int32 == 137 || int32 == 80001 || int32 == 1666600000 || int32 == 1666700000 || int32 == 25 || int32 == 338 || int32 == 250 || int32 == 4002 || int32 == 43114 || int32 == 43113 ? EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateYParityV(byteArray)) : EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateV(byteArray, _chainId));
    }

    // used to sign a message with a users private key stored in memory
    public string SignMessage(string _privateKey, string _message) => new EthereumMessageSigner().HashAndSign(_message, _privateKey);
}