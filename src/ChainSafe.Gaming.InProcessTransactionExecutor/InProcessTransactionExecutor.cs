using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3.Accounts;
using NIpcClient = Nethereum.JsonRpc.IpcClient.IpcClient;
using NWeb3 = Nethereum.Web3.Web3;

namespace ChainSafe.Gaming.InProcessTransactionExecutor
{
    /// <summary>
    /// Concrete Implementation of <see cref="ITransactionExecutor"/> that uses a private key to execute transactions.
    /// </summary>
    public class InProcessTransactionExecutor : ITransactionExecutor
    {
        private readonly NWeb3 web3;
        private readonly IRpcProvider rpcProvider;
        private readonly string accountAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessTransactionExecutor"/> class.
        /// </summary>
        /// <param name="signer">Injected <see cref="ISigner"/>.</param>
        /// <param name="chainConfig">Injected <see cref="IChainConfig"/>.</param>
        /// <param name="rpcProvider">Injected <see cref="IRpcProvider"/>.</param>
        /// <exception cref="Web3Exception">Throws exception if initializing instance fails.</exception>
        public InProcessTransactionExecutor(ISigner signer, IChainConfig chainConfig, IRpcProvider rpcProvider)
        {
            // It should be possible to set up other signers to work with this as well.
            // However, does it make sense to let a remote wallet sign a transaction, but
            // broadcast it locally? I think not.
            var privateKey = (signer as InProcessSigner.InProcessSigner)?.GetKey() ??
                throw new Web3Exception($"{nameof(InProcessTransactionExecutor)} only supports {nameof(InProcessSigner.InProcessSigner)}");
            accountAddress = privateKey.GetPublicAddress();
            var account = new Account(privateKey);
            if (chainConfig.Rpc is not null && !string.Empty.Equals(chainConfig.Rpc))
            {
                web3 = new NWeb3(account, chainConfig.Rpc);
            }
            else if (chainConfig.Ipc is not null && !string.Empty.Equals(chainConfig.Ipc))
            {
                var client = new NIpcClient(chainConfig.Rpc);
                web3 = new NWeb3(client);
            }
            else
            {
                throw new Web3Exception($"{nameof(IChainConfig)} should have at least one communication method set.");
            }

            this.rpcProvider = rpcProvider;
        }

        /// <summary>
        /// Implementation of <see cref="ITransactionExecutor.SendTransaction"/>.
        /// Send a transaction using Wallet Connect.
        /// This prompts user to approve a transaction on a connected wallet.
        /// </summary>
        /// <param name="transaction">Transaction to send.</param>
        /// <returns>Hash response of a successfully executed transaction.</returns>
        /// <exception cref="Web3Exception">Throws Exception if executing transaction fails.</exception>
        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = accountAddress;
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
                var signedTransaction = await web3.TransactionManager.SignTransactionAsync(txInput);
                var txHash = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransaction);
                return await rpcProvider.GetTransaction(txHash);
            }
            catch (Exception ex)
            {
                throw new Web3Exception(ex.Message, ex);
            }
        }
    }
}
