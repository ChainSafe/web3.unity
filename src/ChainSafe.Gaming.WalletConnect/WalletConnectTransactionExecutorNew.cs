using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectTransactionExecutorNew : ITransactionExecutor
    {
        private readonly IRpcProvider rpcProvider;
        private readonly IWalletConnectProvider provider;
        private readonly ISigner signer;

        public WalletConnectTransactionExecutorNew(IWalletConnectProvider provider, IRpcProvider rpcProvider, ISigner signer)
        {
            this.rpcProvider = rpcProvider;
            this.provider = provider;
            this.signer = signer;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = await signer.GetAddress();
            }

            var requestData = new EthSendTransaction(new TransactionModel
            {
                From = transaction.From,
                To = transaction.To,
                Gas = transaction.GasLimit?.HexValue,
                GasPrice = transaction.GasPrice?.HexValue,
                Value = transaction.Value?.HexValue,
                Data = transaction.Data ?? "0x",
                Nonce = transaction.Nonce?.HexValue,
            });

            var hash = await provider.Request(requestData);
            if (!ValidateResponseHash(hash))
            {
                throw new Web3Exception("Incorrect transaction response format.");
            }

            WCLogger.Log($"Transaction executed successfully. Hash: {hash}.");

            return await rpcProvider.GetTransaction(hash);

            bool ValidateResponseHash(string hash)
            {
                return !hash.StartsWith("0x") || hash.Length != 66;
            }
        }
    }
}