using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Reown.Models;
using Nethereum.RPC.Eth.DTOs;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    /// <summary>
    /// Send Transaction Reown Json RPC method params.
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