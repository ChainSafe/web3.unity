using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Blocks;
using Web3Unity.Scripts.Library.Ethers.InternalEvents;
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
            _network = network;
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
            var properties = new Dictionary<string, object>
            {
                {"address", address},
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_getBalance", parameters));
                _captureEvent("eth_getBalance", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getBalance", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<string> GetCode(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, blockTag};
            var properties = new Dictionary<string, object>
            {
                {"address", address},
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = await Perform<string>("eth_getCode", parameters);
                _captureEvent("eth_getCode", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getCode", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<string> GetStorageAt(string address, BigInteger position, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, position.ToHex(false), blockTag};
            var properties = new Dictionary<string, object>
            {
                {"address", address},
                {"position", position.ToString()},
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = await Perform<string>("eth_getStorageAt", parameters);
                _captureEvent("eth_getStorageAt", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getStorageAt", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetTransactionCount(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {address, blockTag};
            var properties = new Dictionary<string, object>
            {
                {"address", address},
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_getTransactionCount", parameters));
                _captureEvent("eth_getTransactionCount", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getTransactionCount", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {blockTag.GetRPCParam(), false};
            var properties = new Dictionary<string, object>
            {
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = await Perform<Block>("eth_getBlockByNumber", parameters);
                _captureEvent("eth_getBlockByNumber", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getBlockByNumber", properties, e);
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
            var properties = new Dictionary<string, object>
            {
                {"block", blockHash}
            };
            try
            {
                var result = await Perform<Block>("eth_getBlockByHash", parameters);
                _captureEvent("eth_getBlockByHash", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getBlockByHash", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] {blockTag.GetRPCParam(), true};
            var properties = new Dictionary<string, object>
            {
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = await Perform<BlockWithTransactions>("eth_getBlockByNumber", parameters);
                _captureEvent("eth_getBlockByNumber", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getBlockByNumber", properties, e);
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
            var properties = new Dictionary<string, object>
            {
                {"blockHash", blockHash}
            };
            try
            {
                var result = await Perform<BlockWithTransactions>("eth_getBlockByHash", parameters);
                _captureEvent("eth_getBlockByHash", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getBlockByHash", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetBlockNumber()
        {
            await GetNetwork();

            var properties = new Dictionary<string, object>();
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_blockNumber", null));
                _captureEvent("eth_blockNumber", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_blockNumber", properties, e);
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

            var properties = new Dictionary<string, object>();
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_gasPrice", null));
                _captureEvent("eth_gasPrice", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_gasPrice", properties, e);
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
            var properties = new Dictionary<string, object>
            {
                {"transaction", transaction},
                {"block", blockTag.GetRPCParamAsNumber()}
            };
            try
            {
                var result = await Perform<string>("eth_call", parameters);
                _captureEvent("eth_call", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_call", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            await GetNetwork();

            var parameters = new object[] {transaction};
            var properties = new Dictionary<string, object>
            {
                {"transaction", transaction}
            };
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_estimateGas", parameters));
                _captureEvent("eth_estimateGas", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_estimateGas", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> GetTransaction(string transactionHash)
        {
            await GetNetwork();

            // TODO: add polling system to wait for transaction to be mined?

            var parameters = new object[] {transactionHash};
            var properties = new Dictionary<string, object>
            {
                {"txHash", transactionHash}
            };
            try
            {
                var result = await Perform<TransactionResponse>("eth_getTransactionByHash", parameters);
                _captureEvent("eth_getTransactionByHash", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getTransactionByHash", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
        {
            await GetNetwork();

            // TODO: add polling system to wait for transaction to be mined?

            var parameters = new object[] {transactionHash};
            var properties = new Dictionary<string, object>
            {
                {"txHash", transactionHash}
            };
            try
            {
                var result = await Perform<TransactionReceipt>("eth_getTransactionReceipt", parameters);
                _captureEvent("eth_getTransactionReceipt", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getTransactionReceipt", properties, e);
                throw new Exception("bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> SendTransaction(string signedTx)
        {
            await GetNetwork();

            var tx = _formater.Transaction.Parse(signedTx);

            var parameters = new object[] {signedTx};
            var properties = new Dictionary<string, object>
            {
                {"rawTx", signedTx}
            };
            try
            {
                var result = await Perform<string>("eth_sendRawTransaction", parameters);
                _captureEvent("eth_sendRawTransaction", properties, result);
                return _wrapTransaction(tx, result);
            }
            catch (Exception e)
            {
                _captureError("eth_sendRawTransaction", properties, e);
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
            var properties = new Dictionary<string, object>
            {
                {"filter", filter}
            };
            try
            {
                var result = await Perform<FilterLog[]>("eth_getLogs", parameters);
                _captureEvent("eth_getLogs", properties, result);
                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getLogs", properties, e);
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

        private void _captureEvent(string method, Dictionary<string, object> properties, object result)
        {
            properties["result"] = result;
            _capture("Call", method, properties);
        }

        private void _captureError(string method, Dictionary<string, object> properties, Exception error)
        {
            properties["error"] = error;
            _capture("Error", method, properties);
        }

        private void _capture(string eventName, string method, Dictionary<string, object> properties)
        {
            _populateEventProperties(properties);

            var network = _network ?? new Network.Network
            {
                ChainId = 0,
                Name = "unknown",
            };

            properties["chainId"] = network.ChainId;
            properties["network"] = network.Name;
            properties["method"] = method;

            PostHog.Client.Capture("[TEST] " + eventName, properties); // TODO: remove [TEST]
        }

        protected virtual void _populateEventProperties(Dictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }
    }
}