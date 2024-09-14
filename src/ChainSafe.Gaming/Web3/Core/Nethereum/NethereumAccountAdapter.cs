using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.RPC.AccountSigning;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public class NethereumAccountAdapter : INethereumAccountAdapter, IChainSwitchHandler
    {
        private readonly ISigner signer;
        private readonly NethereumTransactionManagerAdapter transactionManager;
        private readonly IChainConfig chainConfig;

        public NethereumAccountAdapter(IChainConfig chainConfig, ISigner signer, ITransactionExecutor transactionExecutor)
        {
            this.chainConfig = chainConfig;
            this.signer = signer;
            transactionManager = new NethereumTransactionManagerAdapter(this, chainConfig, transactionExecutor);
        }

        public string Address => signer.PublicAddress;

        public ITransactionManager TransactionManager => transactionManager;

        public INonceService NonceService { get; set; }

        public IAccountSigningService AccountSigningService => null;

        public Task HandleChainSwitching()
        {
            transactionManager.SetChainConfig(chainConfig);
            return Task.CompletedTask;
        }
    }
}