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
        private readonly IRpcProvider provider;

        private readonly WalletConnectSigner signer;

        private readonly WalletConnectConfig config;

        public WalletConnectTransactionExecutor(WalletConnectConfig config, ISigner signer, IRpcProvider provider)
        {
            this.signer = signer as WalletConnectSigner ??
                          throw new Web3Exception($"{nameof(WalletConnectTransactionExecutor)} only supports {nameof(WalletConnectSigner)}");

            this.provider = provider;

            this.config = config;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (config.Testing)
            {
                return await provider.GetTransaction(config.TestResponse);
            }

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

            string hash = await signer.Request<EthSendTransaction, string>(requestData);

            // TODO replace validation with regex
            if (!hash.StartsWith("0x") || hash.Length != 66)
            {
                throw new Web3Exception($"incorrect txn response format {hash}");
            }

            WCLogger.Log($"Transaction executed with hash {hash}");

            return await provider.GetTransaction(hash);
        }

        public ValueTask WillStartAsync() => new ValueTask(Task.CompletedTask);

        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);
    }
}