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
        private readonly IWalletProvider walletProvider;

        private readonly ISigner signer;

        public WalletTransactionExecutor(IWalletProvider walletProvider, ISigner signer)
        {
            this.walletProvider = walletProvider;

            this.signer = signer;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            transaction.From ??= signer.PublicAddress;
            transaction.Data ??= "0x";

            string hash = await walletProvider.Perform<string>("eth_sendTransaction", transaction);

            hash = hash.AssertTransactionValid();

            return await walletProvider.GetTransaction(hash);
        }
    }
}