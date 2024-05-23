using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Concrete implementation of <see cref="ITransactionExecutor"/> via HyperPlay desktop client.
    /// </summary>
    public class HyperPlayTransactionExecutor : ITransactionExecutor
    {
        private readonly IWalletProvider walletProvider;
        private readonly ISigner signer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperPlayTransactionExecutor"/> class.
        /// </summary>
        /// <param name="walletProvider">HyperPlay provider for making RPC requests.</param>
        /// <param name="signer">Signer for fetching public address.</param>
        public HyperPlayTransactionExecutor(IWalletProvider walletProvider, ISigner signer)
        {
            this.walletProvider = walletProvider;
            this.signer = signer;
        }

        /// <summary>
        /// Send a Transaction via HyperPlay desktop Client.
        /// This prompts user to approve a transaction on HyperPlay.
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

            return await walletProvider.GetTransaction(hash);
        }
    }
}