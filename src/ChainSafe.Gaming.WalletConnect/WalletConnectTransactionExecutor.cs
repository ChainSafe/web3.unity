using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Implementation of <see cref="ITransactionExecutor"/> for Wallet Connect.
    /// </summary>
    public class WalletConnectTransactionExecutor : ITransactionExecutor, ILifecycleParticipant
    {
        private readonly IWalletConnectCustomProvider walletConnectCustomProvider;

        private readonly IRpcProvider rpcProvider;

        private readonly ISigner signer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletConnectTransactionExecutor"/> class.
        /// </summary>
        /// <param name="walletConnectCustomProvider">Wallet Connect Provider that connects wallet and makes jsom RPC requests via Wallet Connect.</param>
        /// <param name="rpcProvider">Provider for getting transaction receipt.</param>
        public WalletConnectTransactionExecutor(IWalletConnectCustomProvider walletConnectCustomProvider, IRpcProvider rpcProvider, ISigner signer)
        {
            this.walletConnectCustomProvider = walletConnectCustomProvider;

            this.rpcProvider = rpcProvider;

            this.signer = signer;
        }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStartAsync"/>.
        /// Lifetime event method, called during initialization.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public ValueTask WillStartAsync() => new ValueTask(Task.CompletedTask);

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStopAsync"/>.
        /// Lifetime event method, called during <see cref="Web3.TerminateAsync"/>.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);

        /// <summary>
        /// Implementation of <see cref="ITransactionExecutor.SendTransaction"/>.
        /// Send a transaction using Wallet Connect.
        /// This prompts user to approve a transaction on a connected wallet.
        /// </summary>
        /// <param name="transaction">Transaction to send.</param>
        /// <returns>Hash response of a successfully executed transaction.</returns>
        /// <exception cref="Web3Exception">Throws Exception if executing transaction fails.</exception>
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

            string hash = await walletConnectCustomProvider.Request(requestData);

            // TODO replace validation with regex
            if (!hash.StartsWith("0x") || hash.Length != 66)
            {
                throw new Web3Exception($"incorrect txn response format {hash}");
            }

            WCLogger.Log($"Transaction executed with hash {hash}");

            // TODO use wallet connect to get receipt if possible.
            return await rpcProvider.GetTransaction(hash);
        }
    }
}