using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    // todo move all functionality out of this class and delete this file
    public abstract class BaseProvider : IRpcProvider, ILifecycleParticipant
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

        public virtual async ValueTask WillStartAsync()
        {
            network ??= await GetNetwork();
        }

        public virtual ValueTask WillStopAsync() => new(Task.CompletedTask);

        public virtual Task<Network.Network> DetectNetwork()
        {
            throw new Web3Exception("provider does not support network detection");
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

            var blockNumber = (await this.GetBlockNumber()).ToUlong();
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