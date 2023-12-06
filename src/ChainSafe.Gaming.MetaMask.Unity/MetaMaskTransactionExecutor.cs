using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskTransactionExecutor : ITransactionExecutor, ILifecycleParticipant
    {
        private readonly ILogWriter logWriter;

        private readonly IMetaMaskProvider metaMaskProvider;

        private readonly ISigner signer;

        public MetaMaskTransactionExecutor(ILogWriter logWriter, IMetaMaskProvider metaMaskProvider, ISigner signer)
        {
            this.logWriter = logWriter;

            this.metaMaskProvider = metaMaskProvider;

            this.signer = signer;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = await signer.GetAddress();
            }

            logWriter.Log($"fee stuff {transaction.MaxFeePerGas} {transaction.MaxPriorityFeePerGas}");

            TransactionInput transactionInput = new TransactionInput
            {
                From = transaction.From,
                To = transaction.To,
                Gas = transaction.GasLimit,
                GasPrice = transaction.GasPrice,
                Value = transaction.Value,
                Data = transaction.Data ?? "0x",
                Nonce = transaction.Nonce,
                AccessList = transaction.AccessList,
            };

            string hash = await metaMaskProvider.Request<string>("eth_sendTransaction", transactionInput);

            // TODO replace validation with regex
            if (!hash.StartsWith("0x") || hash.Length != 66)
            {
                throw new Web3Exception($"incorrect txn response format {hash}");
            }

            logWriter.Log($"Transaction executed with hash {hash}");

            return await metaMaskProvider.Request<TransactionResponse>("eth_getTransactionByHash", hash);
        }

        public ValueTask WillStartAsync() => new ValueTask(Task.CompletedTask);

        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);
    }
}