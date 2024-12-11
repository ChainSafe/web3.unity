using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.Reown.AppKit
{
    public class AppKitTransactionExecutor : ITransactionExecutor
    {
        private readonly IWalletProvider walletProvider;

        private readonly ISigner signer;

        public AppKitTransactionExecutor(IWalletProvider walletProvider, ISigner signer)
        {
            this.walletProvider = walletProvider;

            this.signer = signer;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            transaction.From ??= signer.PublicAddress;
            transaction.Data ??= "0x";

            string hash = await walletProvider.Request<string>("eth_sendTransaction", transaction.ToTransactionInput());

            hash = hash.AssertTransactionValid();

            return await walletProvider.GetTransaction(hash);
        }
    }
}