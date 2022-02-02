using System.Numerics;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;

public class Web3PrivateKey
{
    public static string SignTransaction(string _privateKey, string _transaction, string _chainId)
    {
        EthECKey key = new EthECKey(_privateKey);
        // convert transaction
        byte[] hashByteArr = HexByteConvertorExtensions.HexToByteArray(_transaction);
        // parse chain id
        BigInteger chainId = BigInteger.Parse(_chainId);
        // sign transaction
        string signature = EthECDSASignature.CreateStringSignature(key.SignAndCalculateV(hashByteArr, chainId)); 
        return signature;
    }

    public static string Address(string _privateKey)
    {
        EthECKey key = new EthECKey(_privateKey);
        return key.GetPublicAddress();
    }

    public static string Sign(string _privateKey, string _message)
    {
        var signer = new EthereumMessageSigner();
        EthECKey key = new EthECKey(_privateKey);
        string signature = signer.EncodeUTF8AndSign(_message, new EthECKey(_privateKey));
        return signature;
    }
}
