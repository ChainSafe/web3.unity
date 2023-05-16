using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Block = Web3Unity.Scripts.Library.Ethers.Blocks.Block;
using BlockWithTransactions = Web3Unity.Scripts.Library.Ethers.Blocks.BlockWithTransactions;
using Transaction = Web3Unity.Scripts.Library.Ethers.Transactions.Transaction;
using TransactionReceipt = Web3Unity.Scripts.Library.Ethers.Transactions.TransactionReceipt;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public abstract class BaseProvider : IEvmProvider
    {
        private readonly bool anyNetwork = true;

        // TODO: this isn't actually used, see comment in SendTransaction
        private readonly Formatter formater = new();

        private readonly Web3Environment environment;

        private Network.Network network;
        private List<Event> events = new();
        private ulong nextPollId = 1;
        private InternalBlockNumber internalBlockNumber;
        private ulong maxInternalBlockNumber;
        private ulong lastBlockNumber;

        private ulong? fastBlockNumber;

        /*
        private Task<ulong> fastBlockNumberTask;
        */
        private ulong fastQueryDate;

        private long emittedBlock;

        private CancellationTokenSource pollLoopCts;

        public BaseProvider(Network.Network network, Web3Environment environment)
        {
            this.network = network;
            this.environment = environment;
        }

        public Network.Network Network
        {
            get => network;
            protected set => network = value;
        }

        private TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(1);

        private static ulong GetTime()
        {
            // TODO: Millisecond returns only the millisecond component
            // of the time (between 0 and 999). This is likely wrong.
            return (ulong)DateTime.Now.Millisecond;
        }

        public virtual async ValueTask Initialize()
        {
            if (network != null)
            {
                return;
            }

            network = await GetNetwork();
        }

        public virtual Task<Network.Network> DetectNetwork()
        {
            throw new Web3Exception("provider does not support network detection");
        }

        public async Task<HexBigInteger> GetBalance(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };
            var properties = new Dictionary<string, object>
            {
                { "address", address },
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_getBalance", parameters));
                CaptureEvent("eth_getBalance", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getBalance", properties, e);
                throw new Web3Exception("eth_getBalance: bad result from backend", e);
            }
        }

        public async Task<string> GetCode(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };
            var properties = new Dictionary<string, object>
            {
                { "address", address },
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = await Perform<string>("eth_getCode", parameters);
                CaptureEvent("eth_getCode", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getCode", properties, e);
                throw new Web3Exception("eth_getCode: bad result from backend", e);
            }
        }

        public async Task<string> GetStorageAt(string address, BigInteger position, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, position.ToHex(BitConverter.IsLittleEndian), blockTag };
            var properties = new Dictionary<string, object>
            {
                { "address", address },
                { "position", position.ToString() },
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = await Perform<string>("eth_getStorageAt", parameters);
                CaptureEvent("eth_getStorageAt", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getStorageAt", properties, e);
                throw new Web3Exception("eth_getStorageAt: bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetTransactionCount(string address, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { address, blockTag };
            var properties = new Dictionary<string, object>
            {
                { "address", address },
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_getTransactionCount", parameters));
                CaptureEvent("eth_getTransactionCount", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getTransactionCount", properties, e);
                throw new Web3Exception("eth_getTransactionCount: bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), false };
            var properties = new Dictionary<string, object>
            {
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = await Perform<Block>("eth_getBlockByNumber", parameters);
                CaptureEvent("eth_getBlockByNumber", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getBlockByNumber", properties, e);
                throw new Web3Exception("eth_getBlockByNumber: bad result from backend", e);
            }
        }

        public async Task<Block> GetBlock(string blockHash)
        {
            await GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, false };
            var properties = new Dictionary<string, object>
            {
                { "block", blockHash },
            };
            try
            {
                var result = await Perform<Block>("eth_getBlockByHash", parameters);
                CaptureEvent("eth_getBlockByHash", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getBlockByHash", properties, e);
                throw new Web3Exception("eth_getBlockByHash: bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { blockTag.GetRPCParam(), true };
            var properties = new Dictionary<string, object>
            {
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = await Perform<BlockWithTransactions>("eth_getBlockByNumber", parameters);
                CaptureEvent("eth_getBlockByNumber", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getBlockByNumber", properties, e);
                throw new Web3Exception("eth_getBlockByNumber: bad result from backend", e);
            }
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactions(string blockHash)
        {
            await GetNetwork();

            if (!blockHash.HasHexPrefix() || blockHash.Length != 66)
            {
                throw new Web3Exception("wrong block hash format");
            }

            var parameters = new object[] { blockHash, true };
            var properties = new Dictionary<string, object>
            {
                { "blockHash", blockHash },
            };
            try
            {
                var result = await Perform<BlockWithTransactions>("eth_getBlockByHash", parameters);
                CaptureEvent("eth_getBlockByHash", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getBlockByHash", properties, e);
                throw new Web3Exception("eth_getBlockByHash: bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> GetBlockNumber()
        {
            await GetNetwork();

            var properties = new Dictionary<string, object>();
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_blockNumber", null));
                CaptureEvent("eth_blockNumber", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_blockNumber", properties, e);
                throw new Web3Exception("eth_blockNumber: bad result from backend", e);
            }
        }

        public async Task<Network.Network> GetNetwork()
        {
            var network = await Ready();
            var currentNetwork = await DetectNetwork();

            if (network.ChainId == currentNetwork.ChainId)
            {
                return network;
            }

            if (!anyNetwork)
            {
                throw new Web3Exception("underlying network changed");
            }

            Emit("chainChanged", new object[] { currentNetwork.ChainId });

            this.network = currentNetwork;
            return this.network;
        }

        public async Task<HexBigInteger> GetGasPrice()
        {
            await GetNetwork();

            var properties = new Dictionary<string, object>();
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_gasPrice", null));
                CaptureEvent("eth_gasPrice", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_gasPrice", properties, e);
                throw new Web3Exception("eth_gasPrice: bad result from backend", e);
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
                maxFeePerGas = (block.BaseFeePerGas * new BigInteger(2)) + maxPriorityFeePerGas;
            }

            return new FeeData
            {
                GasPrice = gasPrice,
                MaxFeePerGas = maxFeePerGas,
                MaxPriorityFeePerGas = maxPriorityFeePerGas,
            };
        }

        public async Task<string> Call(TransactionRequest transaction, BlockParameter blockTag = null)
        {
            await GetNetwork();

            blockTag ??= new BlockParameter();

            var parameters = new object[] { transaction, blockTag.GetRPCParam() };
            var properties = new Dictionary<string, object>
            {
                { "transaction", transaction },
                { "block", blockTag.GetRPCParamAsNumber() },
            };
            try
            {
                var result = await Perform<string>("eth_call", parameters);
                CaptureEvent("eth_call", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_call", properties, e);
                throw new Web3Exception("eth_call: bad result from backend", e);
            }
        }

        public async Task<HexBigInteger> EstimateGas(TransactionRequest transaction)
        {
            await GetNetwork();

            var parameters = new object[] { transaction };
            var properties = new Dictionary<string, object>
            {
                { "transaction", transaction },
            };
            try
            {
                var result = new HexBigInteger(await Perform<string>("eth_estimateGas", parameters));
                CaptureEvent("eth_estimateGas", properties, result.ToString());
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_estimateGas", properties, e);
                throw new Web3Exception("eth_estimateGas: bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> GetTransaction(string transactionHash)
        {
            await GetNetwork();

            var parameters = new object[] { transactionHash };
            var properties = new Dictionary<string, object>
            {
                { "txHash", transactionHash },
            };
            try
            {
                var result = await Perform<TransactionResponse>("eth_getTransactionByHash", parameters);
                CaptureEvent("eth_getTransactionByHash", properties, result);

                if (result == null)
                {
                    throw new Web3Exception("transaction not found");
                }

                if (result.BlockNumber == null)
                {
                    result.Confirmations = 0;
                }
                else if (result.Confirmations == null)
                {
                    var blockNumber = await GetBlockNumber();
                    var confirmations = (blockNumber.ToUlong() - result.BlockNumber.ToUlong()) + 1;
                    if (confirmations <= 0)
                    {
                        confirmations = 1;
                    }

                    result.Confirmations = confirmations;
                }

                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getTransactionByHash", properties, e);
                throw new Web3Exception("eth_getTransactionByHash: bad result from backend", e);
            }
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
        {
            await GetNetwork();

            var parameters = new object[] { transactionHash };
            var properties = new Dictionary<string, object>
            {
                { "txHash", transactionHash },
            };
            try
            {
                var result = await Perform<TransactionReceipt>("eth_getTransactionReceipt", parameters);
                CaptureEvent("eth_getTransactionReceipt", properties, result);

                if (result == null)
                {
                    // throw new Exception("transaction receipt not found");
                    return result;
                }

                if (result.BlockNumber == null)
                {
                    result.Confirmations = 0;
                }
                else if (result.Confirmations == null)
                {
                    var blockNumber = await GetBlockNumber();
                    var confirmations = (blockNumber.ToUlong() - result.BlockNumber.ToUlong()) + 1;
                    if (confirmations <= 0)
                    {
                        confirmations = 1;
                    }

                    result.Confirmations = confirmations;
                }

                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getTransactionReceipt", properties, e);
                throw new Web3Exception("eth_getTransactionReceipt: bad result from backend", e);
            }
        }

        public async Task<TransactionResponse> SendTransaction(string signedTx)
        {
            await GetNetwork();

            // TODO: _formater is never assigned, so this code will inevitably fail.
            // Is this method unused? -> Yes it is, only used in BaseSigner's implementation
            // of SendTransaction, which is always overridden
            var tx = formater.Transaction.Parse(signedTx);

            var parameters = new object[] { signedTx };
            var properties = new Dictionary<string, object>
            {
                { "rawTx", signedTx },
            };
            try
            {
                var result = await Perform<string>("eth_sendRawTransaction", parameters);
                CaptureEvent("eth_sendRawTransaction", properties, result);
                return WrapTransaction(tx, result);
            }
            catch (Exception e)
            {
                CaptureError("eth_sendRawTransaction", properties, e);
                throw new Web3Exception("eth_sendRawTransaction: bad result from backend", e);
            }
        }

        public TransactionResponse WrapTransaction(Transaction tx, string hash)
        {
            if (hash != null && hash.Length != 66)
            {
                throw new Web3Exception("invalid response - SendTransaction");
            }

            var result = (TransactionResponse)tx;
            result.Hash = hash;

            result.Wait = async () =>
            {
                var receipt = await WaitForTransactionInternal(hash, 1, 0);
                return receipt;
            };

            result.WaitParams = async (uint confirms, uint timeout) =>
            {
                var receipt = await WaitForTransactionInternal(hash, confirms, timeout);
                return receipt;
            };

            return result;
        }

        public async Task<TransactionReceipt> WaitForTransactionReceipt(
            string transactionHash,
            uint confirmations = 1,
            uint timeout = 0)
        {
            return await WaitForTransactionInternal(transactionHash, confirmations, timeout);
        }

        private async Task<TransactionReceipt> WaitForTransactionInternal(
            string transactionHash,
            uint confirmations = 1,
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
                // TODO: implement exponential backoff?
                await Task.Delay(1000);

                receipt = await GetTransactionReceipt(transactionHash);
                if (receipt != null && receipt.Confirmations >= confirmations)
                {
                    return receipt;
                }

                // if timeout disabled
                if (noTimeout)
                {
                    continue;
                }

                timeout--;

                if (timeout == 0)
                {
                    throw new Web3Exception("timeout waiting for transaction");
                }
            }
        }

        public async Task<FilterLog[]> GetLogs(NewFilterInput filter)
        {
            await GetNetwork();

            var parameters = new object[] { filter };
            var properties = new Dictionary<string, object>
            {
                { "filter", filter },
            };
            try
            {
                var result = await Perform<FilterLog[]>("eth_getLogs", parameters);
                CaptureEvent("eth_getLogs", properties, result);
                return result;
            }
            catch (Exception e)
            {
                CaptureError("eth_getLogs", properties, e);
                throw new Web3Exception("eth_getLogs: bad result from backend", e);
            }
        }

        public virtual Task<T> Perform<T>(string method, params object[] parameters)
        {
            throw new Exception(method + " not implemented");
        }

        private async Task<Network.Network> Ready()
        {
            if (network != null)
            {
                return network;
            }

            network = await DetectNetwork();
            return network;
        }

        private void CaptureEvent(string method, Dictionary<string, object> properties, object result)
        {
            properties["result"] = result;
            Capture("Call", method, properties);
        }

        private void CaptureError(string method, Dictionary<string, object> properties, Exception error)
        {
            properties["error"] = error;
            Capture("Error", method, properties);
        }

        private void Capture(string eventName, string method, Dictionary<string, object> properties)
        {
            PopulateEventProperties(properties);

            var network = this.network ?? new Network.Network
            {
                ChainId = 0,
                Name = "unknown",
            };

            properties["chainId"] = network.ChainId;
            properties["network"] = network.Name;
            properties["method"] = method;

            // PostHog.Client.Capture("[TEST] " + eventName, properties); // TODO: remove [TEST]
        }

        protected virtual void PopulateEventProperties(Dictionary<string, object> properties)
        {
        }

        // TODO: unused
        /*
        private Task<ulong> _getFastBlockNumber()
        {
            var now = BaseProvider.GetTime();

            // Stale block number, request a newer value
            if (now - fastQueryDate > 2 * (ulong)PollingInterval.Milliseconds)
            {
                fastQueryDate = now;
                fastBlockNumberTask = Task.Run(async () =>
                {
                    var blockNumber = (await GetBlockNumber()).ToUlong();
                    if (fastBlockNumber == null || blockNumber > fastBlockNumber)
                    {
                        fastBlockNumber = blockNumber;
                    }

                    return blockNumber;
                });
            }

            return fastBlockNumberTask;
        }
        */

        private void SetFastBlockNumber(ulong blockNumber)
        {
            // Older block, maybe a stale request
            if (fastBlockNumber != null && blockNumber < fastBlockNumber)
            {
                return;
            }

            // Update the time we updated the blocknumber
            fastQueryDate = BaseProvider.GetTime();

            // Newer block number, use  it
            if (fastBlockNumber == null || blockNumber > fastBlockNumber)
            {
                fastBlockNumber = blockNumber;
            }
        }

        // TODO: this can be refactored into a method
        private void SetPollLoopState(bool enabled)
        {
            switch (enabled)
            {
                case true when pollLoopCts == null:
                    pollLoopCts = new();
                    RunPollLoop(pollLoopCts.Token);
                    break;

                case false when pollLoopCts != null:
                    // This will eventually cause the poll loop to stop.
                    // Note that restarting the poll loop will make a new task
                    // with a new cancellation token source and will not interfere
                    // with the one we're stopping here.
                    pollLoopCts.Cancel();
                    pollLoopCts = null;
                    break;
            }
        }

        private async void RunPollLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Since Poll is async, we can't just wait one second.
                // Also, we can't just ignore the result of Poll, since
                // we don't want to have multiple poll operations happening
                // at the same time if the network is unstable and polls
                // take longer than PollingInterval. So we measure the time
                // we would like the next poll to happen before starting
                // the current poll.
                var nextPollTime = DateTime.Now + PollingInterval;

                try
                {
                    await Poll();
                }
                catch (Exception ex)
                {
                    environment.LogWriter.LogError(ex.ToString());
                }

                var now = DateTime.Now;
                if (now < nextPollTime)
                {
                    await Task.Delay(nextPollTime - now);
                }
            }
        }

        private async Task<ulong> GetInternalBlockNumber(ulong maxAge)
        {
            await Ready();

            // Allowing stale data up to maxAge old
            if (maxAge > 0 && internalBlockNumber != null)
            {
                var internalBlockNumber = this.internalBlockNumber;

                if (GetTime() - internalBlockNumber.RespTime <= maxAge)
                {
                    return internalBlockNumber.BlockNumber;
                }
            }

            var reqTime = GetTime();

            var blockNumber = (await GetBlockNumber()).ToUlong();
            if (blockNumber < maxInternalBlockNumber)
            {
                blockNumber = maxInternalBlockNumber;
            }

            maxInternalBlockNumber = blockNumber;

            var internalBlock = new InternalBlockNumber
            {
                BlockNumber = blockNumber,
                ReqTime = reqTime,
                RespTime = GetTime(),
            };

            internalBlockNumber = internalBlock;

            return internalBlock.BlockNumber;
        }

        internal virtual async Task Poll()
        {
            var pollId = nextPollId++;

            // blockNumber through _getInternalBockNumber
            ulong blockNumber;
            try
            {
                blockNumber = await GetInternalBlockNumber(100 + ((ulong)PollingInterval.TotalMilliseconds / 2));
            }
            catch (Exception e)
            {
                environment.LogWriter.LogError(e.ToString());
                Emit("error", new object[] { e });
                return;
            }

            SetFastBlockNumber(blockNumber);

            Emit("poll", new object[] { pollId, blockNumber });

            if (blockNumber == lastBlockNumber)
            {
                Emit("didPoll", new object[] { pollId });
                return;
            }

            if (emittedBlock == -2)
            {
                emittedBlock = (long)blockNumber - 1;
            }

            if (Math.Abs(emittedBlock - (long)blockNumber) > 1000)
            {
                Emit("error", new object[] { new Exception("network block skew detected") });
                Emit("block", new object[] { blockNumber });
            }
            else
            {
                for (var i = emittedBlock + 1; i <= (long)blockNumber; i++)
                {
                    Emit("block", new object[] { i });
                }
            }

            lastBlockNumber = blockNumber;
        }

        private bool Emit(string eventName, object[] data)
        {
            var result = false;
            var stopped = new List<Event>();

            events = events.FindAll(e =>
            {
                if (e.Tag != eventName)
                {
                    return true;
                }

                // TODO: this should execute the event on the foreground thread.
                e.Apply(data);

                result = true;

                if (!e.Once)
                {
                    return true;
                }

                stopped.Add(e);
                return false;
            });

            stopped.ForEach(e => StopEvent(e));
            StopEvent();

            return result;
        }

        private void StopEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual void StartEvent()
        {
            SetPollLoopState(events.Any(e => e.IsPollable));
        }

        protected virtual void StopEvent(Event @event)
        {
            SetPollLoopState(events.Any(e => e.IsPollable));
        }

        protected virtual BaseProvider AddEventListener<T>(string eventName, Func<T, object> listener, bool once)
        {
            var eEvent = new Event<T>(eventName, listener, once);
            events.Add(eEvent);
            StartEvent();
            return this;
        }

        public virtual BaseProvider On<T>(string eventName, Func<T, object> listener)
        {
            return AddEventListener(eventName, listener, false);
        }

        public virtual BaseProvider Once<T>(string eventName, Func<T, object> listener)
        {
            return AddEventListener(eventName, listener, true);
        }

        public virtual BaseProvider RemoveAllListeners()
        {
            events.RemoveAll(_ => true);
            StopEvent();
            return this;
        }
    }

    public class InternalBlockNumber
    {
        public ulong BlockNumber { get; set; }

        public ulong ReqTime { get; set; }

        public ulong RespTime { get; set; }
    }
}