using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public class CreateApproval : MonoBehaviour
{
    public string tokenType = "1155";

    public async void ApproveTransaction()
    {
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        var response = await EVM.CreateApproveTransaction(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network, await Web3Accessor.Web3.Signer.GetAddress(), tokenType);
        Debug.Log("Response: " + response.connection.chain);

        try
        {
            var txRequest = new TransactionRequest
            {
                To = response.tx.to,
                Data = response.tx.data,
                GasPrice = new HexBigInteger(BigInteger.Parse(response.tx.gasPrice)),
                GasLimit = new HexBigInteger(BigInteger.Parse(response.tx.gasLimit)),
                Value = new HexBigInteger(0),
            };
            var responseNft = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(txRequest);
            Debug.Log(JsonConvert.SerializeObject(responseNft));
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}