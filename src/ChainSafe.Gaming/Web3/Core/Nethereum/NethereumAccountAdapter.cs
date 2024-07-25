using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.AccountSigning;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public class NethereumAccountAdapter : IAccount
    {
        private readonly ISigner signer;

        public NethereumAccountAdapter(IChainConfig chainConfig, ISigner signer, ITransactionExecutor transactionExecutor)
        {
            this.signer = signer;
            TransactionManager = new NethereumTransactionManagerAdapter(this, chainConfig, transactionExecutor);
        }

        public string Address => signer.PublicAddress;

        public ITransactionManager TransactionManager { get; }

        public INonceService NonceService { get; set; }

        public IAccountSigningService AccountSigningService => null;
    }
}