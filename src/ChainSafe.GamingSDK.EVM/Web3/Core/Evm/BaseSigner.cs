using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public abstract class BaseSigner : IEvmSigner
    {
        public IEvmProvider Provider { get; }

        public abstract bool Connected { get; }
        public abstract ValueTask Connect();

        protected BaseSigner(IEvmProvider provider)
        {
            Provider = provider;
        }

        // TODO: specific reason why these functions weren't abstract?
        public abstract Task<string> GetAddress();

        public abstract Task<string> SignMessage(byte[] message);

        public abstract Task<string> SignMessage(string message);

        // TODO: JsonRpcSigner doesn't implement this; Is it never used?
        public virtual Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new Exception("SignTransaction not implemented");
        }

        public virtual async Task<HexBigInteger> GetBalance(BlockParameter blockTag = null)
        {
            return await Provider.GetBalance(await GetAddress(), blockTag);
        }

        public virtual async Task<HexBigInteger> GetTransactionCount(BlockParameter blockTag = null)
        {
            return await Provider.GetTransactionCount(await GetAddress(), blockTag);
        }

        public virtual async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            return await Provider.EstimateGas(transaction);
        }

        public virtual async Task<string> Call(TransactionRequest transaction, BlockParameter blockTag = null)
        {
            return await Provider.Call(transaction, blockTag);
        }

        public abstract Task<TransactionResponse> SendTransaction(TransactionRequest transaction);

        public virtual async Task<ulong> GetChainId()
        {
            return (await Provider.GetNetwork()).ChainId;
        }

        public virtual async Task<HexBigInteger> GetGasPrice()
        {
            return await Provider.GetGasPrice();
        }

        public virtual async Task<FeeData> GetFeeData()
        {
            return await Provider.GetFeeData();
        }
    }
}