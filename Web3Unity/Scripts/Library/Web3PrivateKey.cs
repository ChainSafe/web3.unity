using System;
using System.Numerics;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;

public class Web3PrivateKey
{
    const int MATIC_MAIN = 137;
    const int MATIC_MUMBAI = 80001;
    const int HARMONY_MAINNET = 1666600000;
    const int HARMONY_TESTNET = 1666700000;
    const int CRONOS_MAINNET = 25;
    const int CRONOS_TESTNET = 338;
    const int FTM_MAINNET = 250;
    const int FTM_TESTNET = 0xfa2;

    public static string SignTransaction(string _privateKey, string _transaction, string _chainId)
    {
        int CHAIN_ID = Convert.ToInt32(_chainId);
        string signature;
        EthECKey key = new EthECKey(_privateKey);
        // convert transaction
        byte[] hashByteArr = HexByteConvertorExtensions.HexToByteArray(_transaction);
        // parse chain id
        BigInteger chainId = BigInteger.Parse(_chainId);
        // sign transaction
        if ((CHAIN_ID == MATIC_MAIN) || (CHAIN_ID == MATIC_MUMBAI) || (CHAIN_ID == HARMONY_MAINNET) ||
            (CHAIN_ID == HARMONY_TESTNET) || (CHAIN_ID == CRONOS_MAINNET) || (CHAIN_ID == CRONOS_TESTNET) || (CHAIN_ID == FTM_MAINNET) || (CHAIN_ID == FTM_TESTNET))
        {
            signature = EthECDSASignature.CreateStringSignature(key.SignAndCalculateYParityV(hashByteArr));
            return signature;

        }

        signature = EthECDSASignature.CreateStringSignature(key.SignAndCalculateV(hashByteArr, chainId));
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
        string signature = signer.HashAndSign(_message, _privateKey);
        return signature;
    }
}