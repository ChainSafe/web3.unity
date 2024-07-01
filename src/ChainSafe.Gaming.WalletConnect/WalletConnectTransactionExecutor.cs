using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// WalletConnect implementation of <see cref="ITransactionExecutor"/>.
    /// </summary>
    public class WalletConnectTransactionExecutor : ITransactionExecutor
    {
        private readonly IWalletProvider provider;
        private readonly ISigner signer;

        public WalletConnectTransactionExecutor(IWalletProvider provider, ISigner signer)
        {
            this.provider = provider;
            this.signer = signer;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = signer.PublicAddress;
            }

            var requestData = new TransactionModel
            {
                From = transaction.From,
                To = transaction.To,
                Gas = transaction.GasLimit?.HexValue,
                GasPrice = transaction.GasPrice?.HexValue,
                Value = transaction.Value?.HexValue,
                Data = transaction.Data ?? "0x",
                Nonce = transaction.Nonce?.HexValue,
            };

            var hash = await provider.Perform<string>("eth_sendTransaction", requestData);
            if (!ValidateResponseHash(hash))
            {
                throw new Web3Exception($"Incorrect transaction response format: \"{hash}\".");
            }

            WCLogger.Log($"Transaction executed successfully. Hash: {hash}.");

            return await provider.GetTransaction(hash);

            bool ValidateResponseHash(string hash)
            {
                string hashPattern = @"^0x[a-fA-F0-9]{64}$";
                return Regex.IsMatch(hash, hashPattern);
            }
        }
    }
}