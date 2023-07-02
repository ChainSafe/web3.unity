using System;
using System.Collections;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using UnityEngine;

// creates a transaction for the wallet
public class SignatureService : ITransactionSigner, IMessageSigner
{
    /// <summary>
    /// Signs a transaction using the provided private key and transaction data.
    /// </summary>
    /// <param name="privateKey">The private key used for signing the transaction.</param>
    /// <param name="transaction">The transaction data to sign.</param>
    /// <returns>The signature string of the signed transaction.</returns>
    public string SignTransaction(string privateKey, string transaction)
    {
        var projectConfig = LoadProjectConfigData();
        int chainId = Convert.ToInt32(projectConfig.ChainId);
        var ethEcKey = new EthECKey(privateKey);
        var byteArray = transaction.HexToByteArray();
        var chainIdBigInt = BigInteger.Parse(projectConfig.ChainId);

        return ShouldUseYParityV(chainId)
            ? EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateYParityV(byteArray))
            : EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateV(byteArray, chainIdBigInt));
    }

    /// <summary>
    /// Loads the project configuration data.
    /// </summary>
    /// <returns>The loaded project configuration data.</returns>
    private ProjectConfigScriptableObject LoadProjectConfigData()
    {
        return (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ProjectConfigScriptableObject));
    }

    /// <summary>
    /// Determines whether to use the YParityV signature calculation based on the provided chain ID.
    /// </summary>
    /// <param name="chainId">The chain ID to check.</param>
    /// <returns>True if YParityV signature calculation should be used; otherwise, false.</returns>
    private bool ShouldUseYParityV(int chainId)
    {
        int[] validChainIds = { 137, 80001, 1666600000, 1666700000, 25, 338, 250, 4002, 43114, 43113 };
        return ((IList)validChainIds).Contains(chainId);
    }

    /// <summary>
    /// Signs a message using the provided private key.
    /// </summary>
    /// <param name="privateKey">The private key used for signing the message.</param>
    /// <param name="message">The message to sign.</param>
    /// <returns>The signature string of the signed message.</returns>
    public string SignMessage(string privateKey, string message) => new EthereumMessageSigner().HashAndSign(message, privateKey);
}