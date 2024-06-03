using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public class WalletTransactionExecutor : ITransactionExecutor
    {
        private readonly ILogWriter logWriter;

        private readonly IWalletProvider walletProvider;

        private readonly ISigner signer;

        public WalletTransactionExecutor(ILogWriter logWriter, IWalletProvider walletProvider, ISigner signer)
        {
            this.logWriter = logWriter;

            this.walletProvider = walletProvider;

            this.signer = signer;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = signer.PublicAddress;
            }

            TransactionInput transactionInput = transaction.ToTransactionInput();

            string hash = await walletProvider.Perform<string>("eth_sendTransaction", transactionInput);

            hash = hash.AssertTransactionValid();

            logWriter.Log($"Transaction executed with hash {hash}");

            return await walletProvider.GetTransaction(hash);
        }
    }
}