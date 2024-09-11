using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Model;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public class NethereumTransactionManagerAdapter : TransactionManagerBase
    {
        private readonly ITransactionExecutor transactionExecutor;

        public NethereumTransactionManagerAdapter(IAccount account, IChainConfig chainConfig, ITransactionExecutor transactionExecutor)
        {
            this.transactionExecutor = transactionExecutor;
            Account = account;
            ChainId = BigInteger.Parse(chainConfig.ChainId);
        }

        public override BigInteger DefaultGas { get; set; } = SignedLegacyTransaction.DEFAULT_GAS_LIMIT;

        public override async Task<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            var response = await transactionExecutor.SendTransaction(transactionInput.ToTransactionRequest());
            return response.Hash;
        }

        public override Task<string> SignTransactionAsync(TransactionInput transaction)
        {
            throw new NotImplementedException($"Signing transaction is not implemented for {nameof(NethereumTransactionManagerAdapter)}.");
        }

        public void SetChainConfig(IChainConfig chainConfig)
        {
            ChainId = BigInteger.Parse(chainConfig.ChainId);
        }
    }
}