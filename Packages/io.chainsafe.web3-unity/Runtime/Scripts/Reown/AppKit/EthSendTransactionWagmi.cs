using ChainSafe.Gaming.Reown.Models;
using Nethereum.RPC.Eth.DTOs;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.AppKit
{
    /// <summary>
    /// Wagmi doesn't support array when sending transactions like Nethereum does, so I've created this class to handle the sending
    /// of a single transaction.
    /// </summary>
    [RpcMethod("sendTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99999)]
    [Preserve]
    public class EthSendTransactionWagmi : TransactionModel
    {
     
        public EthSendTransactionWagmi(params TransactionInput[] inputs) : base()
        {
            var input = inputs[0];
            From = input.From;
            To = input.To;
            Data = input.Data;
            Gas = input.Gas?.ToString();
            GasPrice = input.GasPrice?.ToString();
            Value = input.Value?.ToString();
            Nonce = input.Nonce?.ToString();
        }
        
        public EthSendTransactionWagmi()
        {
            
        }
    }
}