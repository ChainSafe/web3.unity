using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public static class ConversionExtensions
    {
        public static TransactionRequest ToTransactionRequest(this TransactionInput transactionInput)
        {
            return new TransactionRequest
            {
                From = transactionInput.From,
                To = transactionInput.To,
                GasLimit = transactionInput.Gas,
                GasPrice = transactionInput.GasPrice,
                Value = transactionInput.Value,
                Data = transactionInput.Data,
                Nonce = transactionInput.Nonce,
                AccessList = transactionInput.AccessList,
            };
        }
    }
}