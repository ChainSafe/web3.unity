using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    public class TransactionExecutor : ITransactionExecutor, ILifecycleParticipant
    {
        private readonly IRpcProvider provider;

        public TransactionExecutor(IRpcProvider provider)
        {
            this.provider = provider;
        }

        public ValueTask WillStartAsync() => new(Task.CompletedTask);

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var hash = await SendUncheckedTransaction(transaction);

            try
            {
                var tx = await provider.GetTransaction(hash);
                return provider.WrapTransaction(tx, hash);
            }
            catch (Exception e)
            {
                throw new Exception($"failed to get transaction {hash}", e);
            }
        }

        private async Task<string> SendUncheckedTransaction(TransactionRequest transaction)
        {
            if (transaction.From == null)
            {
                throw new Web3Exception("Transaction request has no \"from\" address provided");
            }

            if (transaction.GasLimit == null)
            {
                var feeData = await provider.GetFeeData();
                transaction.MaxFeePerGas = new HexBigInteger(feeData.MaxFeePerGas);
            }

            var rpcTxParams = transaction.ToRPCParam();
            return await provider.Perform<string>("eth_sendTransaction", rpcTxParams);
        }
    }
}