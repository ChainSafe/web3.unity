using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.MetaMask
{
    /// <summary>
    /// Implementation of <see cref="ITransactionExecutor"/> for Metamask.
    /// </summary>
    public class MetaMaskTransactionExecutor : ITransactionExecutor
    {
        private readonly ILogWriter logWriter;

        private readonly IWalletProvider walletProvider;

        private readonly ISigner signer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskTransactionExecutor"/> class.
        /// </summary>
        /// <param name="logWriter">Log Writer used for logging messages and errors.</param>
        /// <param name="walletProvider">Metamask provider that connects to Metamask and makes JsonRPC requests.</param>
        /// <param name="signer">Signer for fetching address.</param>
        public MetaMaskTransactionExecutor(ILogWriter logWriter, IWalletProvider walletProvider, ISigner signer)
        {
            this.logWriter = logWriter;

            this.walletProvider = walletProvider;

            this.signer = signer;
        }

        /// <summary>
        /// Implementation of <see cref="ITransactionExecutor.SendTransaction"/>.
        /// Send a transaction using Metamask.
        /// This prompts user to approve a transaction on Metamask.
        /// </summary>
        /// <param name="transaction">Transaction to send.</param>
        /// <returns>Hash response of a successfully executed transaction.</returns>
        /// <exception cref="Web3Exception">Throws Exception if executing transaction fails.</exception>
        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = signer.PublicAddress;
            }

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

            string hash = await walletProvider.Perform<string>("eth_sendTransaction", transactionInput);

            string hashPattern = @"^0x[a-fA-F0-9]{64}$";
            if (!Regex.IsMatch(hash, hashPattern))
            {
                throw new Web3Exception($"incorrect txn response format {hash}");
            }

            logWriter.Log($"Transaction executed with hash {hash}");

            return await walletProvider.GetTransaction(hash);
        }
    }
}