using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public abstract class BaseSigner : ISigner
    {
        internal IProvider _provider;

        protected BaseSigner(IProvider provider)
        {
            _provider = provider;
        }

        public virtual Task<string> GetAddress()
        {
            throw new Exception("GetAddress not implemented");
        }

        public virtual Task<string> SignMessage(byte[] message)
        {
            throw new Exception("SignMessage not implemented");
        }

        public virtual Task<string> SignMessage(string message)
        {
            throw new Exception("SignMessage not implemented");
        }

        public virtual Task<string> SignTransaction(Transaction transaction)
        {
            throw new Exception("SignTransaction not implemented");
        }

        public virtual ISigner Connect(IProvider provider)
        {
            _provider = provider;
            return this;
        }

        public async Task<HexBigInteger> GetBalance(BlockParameter blockTag = null)
        {
            _checkProvider("GetBalance");
            return await _provider.GetBalance(await GetAddress(), blockTag);
        }

        public async Task<HexBigInteger> GetTransactionCount(BlockParameter blockTag = null)
        {
            _checkProvider("GetTransactionCount");
            return await _provider.GetTransactionCount(await GetAddress(), blockTag);
        }

        public async Task<HexBigInteger> EstimateGas(CallInput input)
        {
            _checkProvider("EstimateGas");
            return await _provider.EstimateGas(input);
        }

        public async Task<string> Call(CallInput input, BlockParameter blockTag = null)
        {
            _checkProvider("Call");
            return await _provider.Call(input, blockTag);
        }

        public async Task<Transaction> SendTransaction(Transaction transaction)
        {
            _checkProvider("SendTransaction");
            var signedTx = await SignTransaction(transaction);
            return await _provider.SendTransaction(signedTx);
        }

        public async Task<ulong> GetChainId()
        {
            _checkProvider("GetChainId");
            return (await _provider.GetNetwork()).ChainId;
        }

        public async Task<HexBigInteger> GetGasPrice()
        {
            _checkProvider("GetGasPrice");
            return await _provider.GetGasPrice();
        }

        public async Task<FeeData> GetFeeData()
        {
            _checkProvider("GetFeeData");
            return await _provider.GetFeeData();
        }

        private void _checkProvider(string operation)
        {
            if (_provider == null)
            {
                // TODO: event log
                throw new Exception("missing provider");
            }
        }
    }
}