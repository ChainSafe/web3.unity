using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectTransactionExecutor : ITransactionExecutor, ILifecycleParticipant
    {
        private readonly IWalletConnectProvider walletConnectProvider;

        private readonly IRpcProvider rpcProvider;

        private readonly ISigner signer;

        public WalletConnectTransactionExecutor(IWalletConnectProvider walletConnectProvider, IRpcProvider rpcProvider, ISigner signer)
        {
            this.walletConnectProvider = walletConnectProvider;

            this.rpcProvider = rpcProvider;

            this.signer = signer;
        }

        public ValueTask WillStartAsync() => new ValueTask(Task.CompletedTask);

        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = await signer.GetAddress();
            }

            EthSendTransaction requestData = new EthSendTransaction(new TransactionModel
            {
                From = transaction.From,
                To = transaction.To,
                Gas = transaction.GasLimit?.HexValue,
                GasPrice = transaction.GasPrice?.HexValue,
                Value = transaction.Value?.HexValue,
                Data = transaction.Data ?? "0x",
                Nonce = transaction.Nonce?.HexValue,
            });

            string hash = await walletConnectProvider.Request(requestData);

            // TODO replace validation with regex
            if (!hash.StartsWith("0x") || hash.Length != 66)
            {
                throw new Web3Exception($"incorrect txn response format {hash}");
            }

            WCLogger.Log($"Transaction executed with hash {hash}");

            return await rpcProvider.GetTransaction(hash);
        }
    }
}