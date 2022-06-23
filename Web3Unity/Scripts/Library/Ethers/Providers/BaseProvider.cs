using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Blocks;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public abstract class BaseProvider : IProvider
    {
        public readonly bool AnyNetwork = true;
        internal Network.Network _network;
        internal readonly Formatter _formater;

        public BaseProvider(Network.Network network)
        {
        }

        public Network.Network Network => _network;

        public virtual Task<Network.Network> DetectNetwork()
        {
            throw new Exception("provider does not support network detection");
        }

        public async Task<HexBigInteger> GetBalance(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, blockTag};
            try
            {
                var result = await Perform<string>("eth_getBalance", parameters);
                return new HexBigInteger(result);
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<string> GetCode(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, blockTag};
            try
            {
                var result = await Perform<string>("eth_getCode", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<string> GetStorageAt(string address, BigInteger position, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, position.ToHex(false), blockTag};
            try
            {
                var result = await Perform<string>("eth_getStorageAt", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetTransactionCount(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, blockTag};
            try
            {
                var result = await Perform<string>("eth_getTransactionCount", parameters);
                return new HexBigInteger(result);
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {blockTag.GetRPCParam(), false};
            try
            {
                var result = await Perform<Block>("eth_getBlockByNumber", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(string blockHash)
        {
            await GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Exception("wrong block hash format");
            }

            var parameters = new object[] {blockHash, false};
            try
            {
                var result = await Perform<Block>("eth_getBlockByHash", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {blockTag.GetRPCParam(), true};
            try
            {
                var result = await Perform<BlockWithTransactions>("eth_getBlockByNumber", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(string blockHash)
        {
            await GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Exception("wrong block hash format");
            }

            var parameters = new object[] {blockHash, true};
            try
            {
                var result = await Perform<BlockWithTransactions>("eth_getBlockByHash", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetBlockNumber()
        {
            await GetNetwork();

            try
            {
                var result = await Perform<string>("eth_blockNumber", null);
                return new HexBigInteger(result);
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<Network.Network> GetNetwork()
        {
            var network = await _ready();
            var currentNetwork = await DetectNetwork();

            if (network.ChainId == currentNetwork.ChainId) return network;

            if (!AnyNetwork) throw new Exception("underlying network changed");

            _network = currentNetwork;
            return _network;
        }

        public async Task<HexBigInteger> GetGasPrice()
        {
            await GetNetwork();

            try
            {
                var result = await Perform<string>("eth_gasPrice", null);
                return new HexBigInteger(result);
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<FeeData> GetFeeData()
        {
            var block = await GetBlock();
            var gasPrice = await GetGasPrice();

            var maxPriorityFeePerGas = BigInteger.Zero;
            var maxFeePerGas = BigInteger.Zero;

            if (block.BaseFeePerGas > BigInteger.Zero)
            {
                maxPriorityFeePerGas = new BigInteger(1500000000);
                maxFeePerGas = block.BaseFeePerGas * new BigInteger(2) + maxPriorityFeePerGas;
            }

            return new FeeData
            {
                GasPrice = gasPrice,
                MaxFeePerGas = maxFeePerGas,
                MaxPriorityFeePerGas = maxPriorityFeePerGas
            };
        }

        public async Task<string> Call(TransactionRequest transaction, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {transaction, blockTag.GetRPCParam()};
            try
            {
                var result = await Perform<string>("eth_call", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            await GetNetwork();

            var parameters = new object[] {transaction};
            try
            {
                var result = await Perform<string>("eth_estimateGas", parameters);
                return new HexBigInteger(result);
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> GetTransaction(string transactionHash)
        {
            await GetNetwork();

            // TODO: add polling system to wait for transaction to be mined?

            var parameters = new object[] {transactionHash};
            try
            {
                var result = await Perform<TransactionResponse>("eth_getTransactionByHash", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
        {
            await GetNetwork();

            // TODO: add polling system to wait for transaction to be mined?

            var parameters = new object[] {transactionHash};
            try
            {
                var result = await Perform<TransactionReceipt>("eth_getTransactionReceipt", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> SendTransaction(string signedTx)
        {
            await GetNetwork();

            var tx = _formater.Transaction.Parse(signedTx);

            var parameters = new object[] {signedTx};
            try
            {
                var result = await Perform<string>("eth_sendRawTransaction", parameters);
                return _wrapTransaction(tx, result);
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public TransactionResponse _wrapTransaction(Transaction tx, string hash)
        {
            if (hash != null && hash.Length != 66)
            {
                throw new Exception("invalid response - SendTransaction");
            }

            var result = (TransactionResponse) tx;
            result.Hash = hash;

            result.Wait = async () =>
            {
                var receipt = await GetTransactionReceipt(hash); // TODO: _waitForTransaction(hash, confirms);
                return receipt;
            };

            return result;
        }

        public Task<TransactionReceipt> WaitForTransactionReceipt(string transactionHash, uint confirmations = 1,
            uint timeout = 30)
        {
            throw new NotImplementedException();
        }

        public async Task<FilterLog[]> GetLogs(NewFilterInput filter)
        {
            await GetNetwork();

            var parameters = new object[] {filter};
            try
            {
                var result = await Perform<FilterLog[]>("eth_getLogs", parameters);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("bad result from backend", e);
            }
        }

        public virtual Task<T> Perform<T>(string method, object[] parameters = null)
        {
            throw new Exception(method + " not implemented");
        }

        private async Task<Network.Network> _ready()
        {
            if (_network != null) return _network;

            _network = await DetectNetwork();
            return _network;
        }
    }
}