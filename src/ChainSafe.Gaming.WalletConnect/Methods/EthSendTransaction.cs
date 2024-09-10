using System;
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using Nethereum.RPC.Eth.DTOs;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    /// <summary>
    /// Send Transaction Wallet Connect Json RPC method params.
    /// </summary>
    [RpcMethod("eth_sendTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99999)]
    public class EthSendTransaction : List<TransactionModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthSendTransaction"/> class.
        /// </summary>
        /// <param name="transactions">Transaction to be sent.</param>
        public EthSendTransaction(params TransactionInput[] transactions)
            : base(Array.ConvertAll(transactions, input => new TransactionModel
            {
                From = input.From,
                To = input.To,
                Gas = input.Gas?.HexValue,
                GasPrice = input.GasPrice?.HexValue,
                Value = input.Value?.HexValue,
                Data = input.Data,
                Nonce = input.Nonce?.HexValue,
            }))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthSendTransaction"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public EthSendTransaction()
        {
        }
    }
}