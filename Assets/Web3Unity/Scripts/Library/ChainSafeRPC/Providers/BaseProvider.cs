using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Blocks;
using Web3Unity.Scripts.Library.Ethers.Runtime;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class InternalBlockNumber
    {
        public ulong BlockNumber { get; set; }
        public ulong ReqTime { get; set; }
        public ulong RespTime { get; set; }
    }

    public abstract class BaseProvider : IProvider
    {
        public readonly bool AnyNetwork = true;
        internal Network.Network _network;
        internal readonly Formatter _formater;

        private List<Event> _events = new();
        private ulong _nextPollId = 1;
        private InternalBlockNumber _internalBlockNumber;
        private ulong _maxInternalBlockNumber;
        private ulong _lastBlockNumber;

        private ulong? _fastBlockNumber;
        private Task<ulong> _fastBlockNumberTask;
        private ulong _fastQueryDate;

        private long _emittedBlock;

        internal Dispatcher _dispatcher;
        private object _poller;

        public BaseProvider(Network.Network network)
        {
            _dispatcher = Dispatcher.Initialize();
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

            var parameters = new object[] { address, blockTag };
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
                throw new Exception("eth_getBalance: bad result from backend", e);
            }
        }

        public async Task<string> GetCode(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };
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
                throw new Exception("eth_getCode: bad result from backend", e);
            }
        }

        public async Task<string> GetStorageAt(string address, BigInteger position, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, position.ToHex(false), blockTag };
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
                throw new Exception("eth_getStorageAt: bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetTransactionCount(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };
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
                throw new Exception("eth_getTransactionCount: bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), false };
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
                throw new Exception("eth_getBlockByNumber: bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(string blockHash)
        {
            await GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, false };
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
                throw new Exception("eth_getBlockByHash: bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), true };
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
                throw new Exception("eth_getBlockByNumber: bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(string blockHash)
        {
            await GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, true };
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
                throw new Exception("eth_getBlockByHash: bad result from backend", e);
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
                throw new Exception("eth_blockNumber: bad result from backend", e);
            }
        }

        public async Task<Network.Network> GetNetwork()
        {
            var network = await _ready();
            var currentNetwork = await DetectNetwork();

            if (network.ChainId == currentNetwork.ChainId) return network;

            if (!AnyNetwork) throw new Exception("underlying network changed");

            Emit("chainChanged", new object[] { currentNetwork.ChainId });

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
                throw new Exception("eth_gasPrice: bad result from backend", e);
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

            var parameters = new object[] { transaction, blockTag.GetRPCParam() };
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
                throw new Exception("eth_call: bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            await GetNetwork();

            var parameters = new object[] { transaction };
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
                throw new Exception("eth_estimateGas: bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> GetTransaction(string transactionHash)
        {
            await GetNetwork();

            var parameters = new object[] { transactionHash };
            var properties = new Dictionary<string, object>
            {
                {"txHash", transactionHash}
            };
            try
            {
                var result = await Perform<TransactionResponse>("eth_getTransactionByHash", parameters);
                _captureEvent("eth_getTransactionByHash", properties, result);

                if (result == null)
                {
                    throw new Exception("transaction not found");
                }

                if (result.BlockNumber == null) result.Confirmations = 0;
                else if (result.Confirmations == null)
                {
                    var blockNumber = await GetBlockNumber();
                    var confirmations = (blockNumber.ToUlong() - result.BlockNumber.ToUlong()) + 1;
                    if (confirmations <= 0) confirmations = 1;
                    result.Confirmations = confirmations;
                }

                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getTransactionByHash", properties, e);
                throw new Exception("eth_getTransactionByHash: bad result from backend", e);
            }
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
        {
            await GetNetwork();

            var parameters = new object[] { transactionHash };
            var properties = new Dictionary<string, object>
            {
                {"txHash", transactionHash}
            };
            try
            {
                var result = await Perform<TransactionReceipt>("eth_getTransactionReceipt", parameters);
                _captureEvent("eth_getTransactionReceipt", properties, result);

                if (result == null)
                {
                    // throw new Exception("transaction receipt not found");
                    return result;
                }

                if (result.BlockNumber == null) result.Confirmations = 0;
                else if (result.Confirmations == null)
                {
                    var blockNumber = await GetBlockNumber();
                    var confirmations = (blockNumber.ToUlong() - result.BlockNumber.ToUlong()) + 1;
                    if (confirmations <= 0) confirmations = 1;
                    result.Confirmations = confirmations;
                }

                return result;
            }
            catch (Exception e)
            {
                _captureError("eth_getTransactionReceipt", properties, e);
                throw new Exception("eth_getTransactionReceipt: bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> SendTransaction(string signedTx)
        {
            await GetNetwork();

            var tx = _formater.Transaction.Parse(signedTx);

            var parameters = new object[] { signedTx };
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
                throw new Exception("eth_sendRawTransaction: bad result from backend", e);
            }
        }

        public TransactionResponse _wrapTransaction(Transaction tx, string hash)
        {
            if (hash != null && hash.Length != 66)
            {
                throw new Exception("invalid response - SendTransaction");
            }

            var result = (TransactionResponse)tx;
            result.Hash = hash;

            result.Wait = async () =>
            {
                var receipt = await _waitForTransaction(hash, 1, 0);
                return receipt;
            };

            result.WaitParams = async (uint confirms, uint timeout) =>
            {
                var receipt = await _waitForTransaction(hash, confirms, timeout);
                return receipt;
            };

            return result;
        }

        public async Task<TransactionReceipt> WaitForTransactionReceipt(string transactionHash, uint confirmations = 1,
            uint timeout = 0)
        {
            return await _waitForTransaction(transactionHash, confirmations, timeout);
        }

        private async Task<TransactionReceipt> _waitForTransaction(string transactionHash, uint confirmations = 1,
            uint timeout = 0)
        {
            var receipt = await GetTransactionReceipt(transactionHash);
            if ((receipt != null ? receipt.Confirmations : 0) >= confirmations)
            {
                return receipt;
            }

            var noTimeout = timeout == 0;

            while (true)
            {
                await new WaitForSeconds(1.0f);

                receipt = await GetTransactionReceipt(transactionHash);
                if (receipt != null && receipt.Confirmations >= confirmations)
                {
                    return receipt;
                }

                // if timeout disabled
                if (noTimeout) continue;

                timeout--;

                if (timeout == 0)
                {
                    throw new Exception("timeout waiting for transaction");
                }
            }
        }

        public async Task<FilterLog[]> GetLogs(NewFilterInput filter)
        {
            await GetNetwork();

            var parameters = new object[] { filter };
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
                throw new Exception("eth_getLogs: bad result from backend", e);
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

            // PostHog.Client.Capture("[TEST] " + eventName, properties); // TODO: remove [TEST]
        }

        protected virtual void _populateEventProperties(Dictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        private Task<ulong> _getFastBlockNumber()
        {
            var now = _getTime();

            // Stale block number, request a newer value
            if (now - _fastQueryDate > 2 * (ulong)PollingInterval.Milliseconds)
            {
                _fastQueryDate = now;
                _fastBlockNumberTask = Task.Run(async () =>
                {
                    var blockNumber = (await GetBlockNumber()).ToUlong();
                    if (_fastBlockNumber == null || blockNumber > _fastBlockNumber)
                        _fastBlockNumber = blockNumber;

                    return blockNumber;
                });
            }

            return _fastBlockNumberTask;
        }

        private void _setFastBlockNumber(ulong blockNumber)
        {
            // Older block, maybe a stale request
            if (_fastBlockNumber != null && blockNumber < _fastBlockNumber) return;

            // Update the time we updated the blocknumber
            _fastQueryDate = _getTime();

            // Newer block number, use  it
            if (_fastBlockNumber == null || blockNumber > _fastBlockNumber)
            {
                _fastBlockNumber = blockNumber;
            }
        }

        private TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(1);

        private bool _polling { get; set; }

        private bool Polling
        {
            get => _polling;

            set
            {
                switch (value)
                {
                    case true when !_polling:
                        _polling = true;

                        _dispatcher.Enqueue(async () =>
                        {
                            for (; Polling;)
                            {
                                await new WaitForSeconds(PollingInterval.Seconds);
                                Poll();
                            }
                        });
                        break;
                    case false when _polling:
                        if (_dispatcher != null)
                        {
                            _dispatcher.StopAllCoroutines();
                        }

                        _polling = false;
                        break;
                }
            }
        }

        private async Task<ulong> _getInternalBlockNumber(ulong maxAge)
        {
            await _ready();

            // Allowing stale data up to maxAge old
            if (maxAge > 0 && _internalBlockNumber != null)
            {
                var internalBlockNumber = _internalBlockNumber;

                if (_getTime() - internalBlockNumber.RespTime <= maxAge)
                {
                    return internalBlockNumber.BlockNumber;
                }
            }

            var reqTime = _getTime();

            var blockNumber = (await GetBlockNumber()).ToUlong();
            if (blockNumber < _maxInternalBlockNumber)
            {
                blockNumber = _maxInternalBlockNumber;
            }

            _maxInternalBlockNumber = blockNumber;

            var internalBlock = new InternalBlockNumber
            {
                BlockNumber = blockNumber,
                ReqTime = reqTime,
                RespTime = _getTime()
            };

            _internalBlockNumber = internalBlock;

            return internalBlock.BlockNumber;
        }

        internal virtual async void Poll()
        {
            var pollId = _nextPollId++;

            // blockNumber through _getInternalBockNumber
            ulong blockNumber;
            try
            {
                blockNumber = await _getInternalBlockNumber(100 + (ulong)PollingInterval.Milliseconds / 2);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                Emit("error", new object[] { e });
                return;
            }

            _setFastBlockNumber(blockNumber);

            Emit("poll", new object[] { pollId, blockNumber });

            if (blockNumber == _lastBlockNumber)
            {
                Emit("didPoll", new object[] { pollId });
                return;
            }

            if (_emittedBlock == -2)
            {
                _emittedBlock = (long)blockNumber - 1;
            }

            if (Math.Abs(_emittedBlock - (long)blockNumber) > 1000)
            {
                Emit("error", new object[] { new Exception("network block skew detected") });
                Emit("block", new object[] { blockNumber });
            }
            else
            {
                for (var i = _emittedBlock + 1; i <= (long)blockNumber; i++)
                {
                    Emit("block", new object[] { i });
                }
            }

            _lastBlockNumber = blockNumber;
        }

        private bool Emit(string eventName, object[] data)
        {
            var result = false;
            var stopped = new List<Event>();

            _events = _events.FindAll(e =>
            {
                if (e.Tag != eventName) return true;

                _dispatcher.Enqueue(() =>
                {
                    e.Apply(data);
                });

                result = true;

                if (!e.Once) return true;
                stopped.Add(e);
                return false;

            });

            stopped.ForEach(e => _stopEvent(e));
            _stopEvent();

            return result;
        }

        private void _stopEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual void _startEvent()
        {
            Polling = _events.Any(e => e.IsPollable);
        }

        protected virtual void _stopEvent(Event @event)
        {
            Polling = _events.Any(e => e.IsPollable);
        }

        protected virtual BaseProvider _addEventListener<T>(string eventName, Func<T, object> listener, bool once)
        {
            var eEvent = new Event<T>(eventName, listener, once);
            _events.Add(eEvent);
            _startEvent();
            return this;
        }

        public virtual BaseProvider On<T>(string eventName, Func<T, object> listener)
        {
            return _addEventListener(eventName, listener, false);
        }

        public virtual BaseProvider Once<T>(string eventName, Func<T, object> listener)
        {
            return _addEventListener(eventName, listener, true);
        }

        public virtual BaseProvider RemoveAllListeners()
        {
            _events.RemoveAll(_ => true);
            _stopEvent();
            return this;
        }

        private ulong _getTime()
        {
            return (ulong)DateTime.Now.Millisecond;
        }
    }
}