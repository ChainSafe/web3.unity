using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.RPC;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public abstract class BaseSigner : ISigner
    {
        protected BaseSigner(IProvider provider)
        {
            Provider = provider;
        }

        public virtual IProvider Provider { get; private set; }

        // TODO: specific reason why these functions weren't abstract?
        public abstract Task<string> GetAddress();

        public abstract Task<string> SignMessage(byte[] message);

        public abstract Task<string> SignMessage(string message);

        private static void CaptureError(string operation, string error)
        {
            var properties = new Dictionary<string, object>
            {
                { "error", error },
            };

            /* RpcEnvironmentStore.Environment.CaptureEvent("Chains", properties); */
        }

        // TODO: JsonRpcSigner doesn't implement this; Is it never used?
        public virtual Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new Exception("SignTransaction not implemented");
        }

        public virtual ISigner Connect(IProvider provider)
        {
            Provider = provider;
            return this;
        }

        public virtual async Task<HexBigInteger> GetBalance(BlockParameter blockTag = null)
        {
            CheckProvider("GetBalance");
            return await Provider.GetBalance(await GetAddress(), blockTag);
        }

        public virtual async Task<HexBigInteger> GetTransactionCount(BlockParameter blockTag = null)
        {
            CheckProvider("GetTransactionCount");
            return await Provider.GetTransactionCount(await GetAddress(), blockTag);
        }

        public virtual async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            CheckProvider("EstimateGas");
            return await Provider.EstimateGas(transaction);
        }

        public virtual async Task<string> Call(TransactionRequest transaction, BlockParameter blockTag = null)
        {
            CheckProvider("Call");
            return await Provider.Call(transaction, blockTag);
        }

        public abstract Task<TransactionResponse> SendTransaction(TransactionRequest transaction);

        public virtual async Task<ulong> GetChainId()
        {
            CheckProvider("GetChainId");
            return (await Provider.GetNetwork()).ChainId;
        }

        public virtual async Task<HexBigInteger> GetGasPrice()
        {
            CheckProvider("GetGasPrice");
            return await Provider.GetGasPrice();
        }

        public virtual async Task<FeeData> GetFeeData()
        {
            CheckProvider("GetFeeData");
            return await Provider.GetFeeData();
        }

        private void CheckProvider(string operation)
        {
            if (Provider == null)
            {
                CaptureError(operation, "missing provider");
                throw new Exception("missing provider");
            }
        }
    }
}