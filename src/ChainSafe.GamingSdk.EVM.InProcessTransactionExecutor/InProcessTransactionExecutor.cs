﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3.Accounts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

using NWeb3 = Nethereum.Web3.Web3;

namespace ChainSafe.GamingSdk.EVM.InProcessTransactionExecutor
{
    public class InProcessTransactionExecutor : ITransactionExecutor
    {
        private readonly NWeb3 web3;
        private readonly IRpcProvider rpcProvider;

        public InProcessTransactionExecutor(ISigner signer, IChainConfig chainConfig, IRpcProvider rpcProvider)
        {
            // It should be possible to set up other signers to work with this as well.
            // However, does it make sense to let a remote wallet sign a transaction, but
            // broadcast it locally? I think not.
            var privateKey = (signer as InProcessSigner.InProcessSigner)?.GetKey() ??
                throw new Web3Exception($"{nameof(InProcessTransactionExecutor)} only supports {nameof(InProcessSigner.InProcessSigner)}");
            var account = new Account(privateKey);
            web3 = new NWeb3(account, chainConfig.Rpc);
            this.rpcProvider = rpcProvider;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
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
