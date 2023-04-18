using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public abstract class BaseSigner : ISigner
    {
        internal IProvider _provider;

        protected BaseSigner(IProvider provider)
        {
            _provider = provider;
        }

        public virtual IProvider Provider => _provider;

        // TODO: specific reason why these functions weren't abstract?
        public abstract Task<string> GetAddress();

        public abstract Task<string> SignMessage(byte[] message);

        public abstract Task<string> SignMessage(string message);

        // TODO: JsonRpcSigner doesn't implement this; Is it never used?
        public virtual Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new Exception("SignTransaction not implemented");
        }

        public virtual ISigner Connect(IProvider provider)
        {
            _provider = provider;
            return this;
        }

        public virtual async Task<HexBigInteger> GetBalance(BlockParameter blockTag = null)
        {
            _checkProvider("GetBalance");
            return await _provider.GetBalance(await GetAddress(), blockTag);
        }

        public virtual async Task<HexBigInteger> GetTransactionCount(BlockParameter blockTag = null)
        {
            _checkProvider("GetTransactionCount");
            return await _provider.GetTransactionCount(await GetAddress(), blockTag);
        }

        public virtual async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            _checkProvider("EstimateGas");
            return await _provider.EstimateGas(transaction);
        }

        public virtual async Task<string> Call(TransactionRequest transaction, BlockParameter blockTag = null)
        {
            _checkProvider("Call");
            return await _provider.Call(transaction, blockTag);
        }

        public abstract Task<TransactionResponse> SendTransaction(TransactionRequest transaction);

        public virtual async Task<ulong> GetChainId()
        {
            _checkProvider("GetChainId");
            return (await _provider.GetNetwork()).ChainId;
        }

        public virtual async Task<HexBigInteger> GetGasPrice()
        {
            _checkProvider("GetGasPrice");
            return await _provider.GetGasPrice();
        }

        public virtual async Task<FeeData> GetFeeData()
        {
            _checkProvider("GetFeeData");
            return await _provider.GetFeeData();
        }

        private void _checkProvider(string operation)
        {
            if (_provider == null)
            {
                _captureError(operation, "missing provider");
                throw new Exception("missing provider");
            }
        }

        private static void _captureError(string operation, string error)
        {
            var properties = new Dictionary<string, object>
            {
                {"error", error}
            };

            //DataDog.Client.Capture("Chains", properties);
        }
    }
}