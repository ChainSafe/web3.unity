using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.InProcessTransactionExecutor
{
    /// <summary>
    /// Concrete Implementation of <see cref="ITransactionExecutor"/> that uses a private key to execute transactions.
    /// </summary>
    public class InProcessTransactionExecutor : ITransactionExecutor
    {
        private readonly IRpcProvider rpcProvider;
        private readonly IAccountProvider accountProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessTransactionExecutor"/> class.
        /// </summary>
        /// <param name="accountProvider">Injected <see cref="IAccountProvider"/>.</param>
        /// <param name="rpcProvider">Injected <see cref="IRpcProvider"/>.</param>
        /// <exception cref="Web3Exception">Throws exception if initializing instance fails.</exception>
        public InProcessTransactionExecutor(IAccountProvider accountProvider, IRpcProvider rpcProvider)
        {
            this.rpcProvider = rpcProvider;
            this.accountProvider = accountProvider;
        }

        private IAccount Account => accountProvider.Account;

        /// <summary>
        /// Implementation of <see cref="ITransactionExecutor.SendTransaction"/>.
        /// Send a transaction using Wallet Connect.
        /// This prompts user to approve a transaction on a connected wallet.
        /// </summary>
        /// <param name="transaction">Transaction to send.</param>
        /// <returns>Hash response of a successfully executed transaction.</returns>
        /// <exception cref="Web3Exception">Throws Exception if executing transaction fails.</exception>
        public virtual async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = Account.Address;
            }

            if (transaction.GasPrice == null && transaction.MaxFeePerGas == null)
            {
                var feeData = await rpcProvider.GetFeeData();

                transaction.MaxFeePerGas = feeData.MaxFeePerGas.ToHexBigInteger();
                if (feeData.MaxFeePerGas.IsZero)
                {
                    transaction.GasPrice = await rpcProvider.GetGasPrice();
                }
                else
                {
                    transaction.MaxPriorityFeePerGas = feeData.MaxPriorityFeePerGas.ToHexBigInteger();
                }
            }

            var txInput = new TransactionInput
            {
                AccessList = transaction.AccessList,
                ChainId = transaction.ChainId,
                Data = transaction.Data,
                From = transaction.From,
                To = transaction.To,
                GasPrice = transaction.GasPrice,
                Gas = transaction.GasLimit,
                MaxFeePerGas = transaction.MaxFeePerGas,
                Nonce = transaction.Nonce,
                Type = transaction.Type,
                Value = transaction.Value,
                MaxPriorityFeePerGas = transaction.MaxPriorityFeePerGas,
            };

            try
            {
                string hash = await Account.TransactionManager.SendTransactionAsync(txInput);

                hash = hash.AssertTransactionValid();

                return await rpcProvider.GetTransaction(hash);
            }
            catch (Exception ex)
            {
                throw new Web3Exception(ex.Message, ex);
            }
        }
    }
}
