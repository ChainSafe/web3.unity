using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    public class JsonRpcTransactionExecutor : ITransactionExecutor, ILifecycleParticipant
    {
        private readonly IRpcProvider provider;
        private readonly ISigner signer;

        public JsonRpcTransactionExecutor(ISigner signer, IRpcProvider provider)
        {
            this.provider = provider;
            this.signer = signer;
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
            transaction.From ??= (await signer.GetAddress()).ToLower();

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