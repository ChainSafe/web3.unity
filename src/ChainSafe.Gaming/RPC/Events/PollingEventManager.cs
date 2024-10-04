using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.RPC.Events
{
    public class PollingEventManager : IEventManager, IChainSwitchHandler
    {
        private readonly Dictionary<EventDictionaryKey, Subscription> subscriptions = new();
        private readonly ILogWriter logWriter;
        private readonly IRpcProvider rpcProvider;
        private readonly PollingEventManagerConfig config;

        private CancellationTokenSource pollLoopCts;
        private ulong lastBlockNumber;

        public PollingEventManager(ILogWriter logWriter, IRpcProvider rpcProvider)
            : this(new PollingEventManagerConfig(), rpcProvider, logWriter)
        {
        }

        public PollingEventManager(PollingEventManagerConfig config, IRpcProvider rpcProvider, ILogWriter logWriter)
        {
            this.rpcProvider = rpcProvider;
            this.config = config;
            this.logWriter = logWriter;
        }

        private bool PollingLoopActive => pollLoopCts != null;

        public Task Subscribe<TEvent>(Action<TEvent> handler, params string[] contractAddresses)
            where TEvent : IEventDTO, new()
        {
            var key = new EventDictionaryKey()
            {
                EventType = typeof(TEvent),
                ContractAddresses = contractAddresses,
            };

            if (!subscriptions.ContainsKey(key))
            {
                subscriptions[key] = new Subscription<TEvent>(contractAddresses);
            }

            var subscription = (Subscription<TEvent>)subscriptions[key];
            subscription.Handlers.Add(handler);

            if (!PollingLoopActive)
            {
                SetPollLoopState(true);
            }

            return Task.CompletedTask;
        }

        public Task Unsubscribe<TEvent>(Action<TEvent> handler, params string[] contractAddresses)
            where TEvent : IEventDTO, new()
        {
            var key = new EventDictionaryKey()
            {
                EventType = typeof(TEvent),
                ContractAddresses = contractAddresses,
            };
            if (!subscriptions.TryGetValue(key, out var rawSubscription))
            {
                throw new Web3Exception(contractAddresses.Length == 0
                    ? $"Tried unsubscribing but was not subscribed. Event type: \"{typeof(TEvent).Name}\"."
                    : $"Tried unsubscribing but was not subscribed. Event type: \"{typeof(TEvent).Name}\". " +
                      $"Contract address filter: {string.Join(", ", contractAddresses)}");
            }

            var subscription = (Subscription<TEvent>)rawSubscription;

            if (!subscription.Handlers.Contains(handler))
            {
                throw new Web3Exception(
                    $"Tried unsubscribing but the handler was not subscribed for this event type. " +
                    $"Event type \"{typeof(TEvent).Name}\"");
            }

            subscription.Handlers.Remove(handler);

            if (subscription.Handlers.Count == 0)
            {
                subscriptions.Remove(key);
            }

            if (subscriptions.Count == 0)
            {
                SetPollLoopState(false);
            }

            return Task.CompletedTask;
        }

        public Task HandleChainSwitching()
        {
            if (!PollingLoopActive)
            {
                return Task.CompletedTask;
            }

            SetPollLoopState(false);
            SetPollLoopState(true); // restart the polling loop

            return Task.CompletedTask;
        }

        private void SetPollLoopState(bool enabled)
        {
            switch (enabled)
            {
                case true when pollLoopCts == null:
                    logWriter.Log("Starting event polling loop");
                    pollLoopCts = new();
                    RunPollLoop(pollLoopCts.Token);
                    break;

                case false when pollLoopCts != null:
                    // This will eventually cause the poll loop to stop.
                    // Note that restarting the poll loop will make a new task
                    // with a new cancellation token source and will not interfere
                    // with the one we're stopping here.
                    logWriter.Log("Stopping event polling loop");
                    pollLoopCts.Cancel();
                    pollLoopCts = null;
                    break;
            }
        }

        private async void RunPollLoop(CancellationToken cancellationToken)
        {
            lastBlockNumber = await FetchCurrentBlockNumber();

            while (!cancellationToken.IsCancellationRequested)
            {
                // Since Poll is async, we can't just wait one second.
                // Also, we can't just ignore the result of Poll, since
                // we don't want to have multiple poll operations happening
                // at the same time if the network is unstable and polls
                // take longer than PollInterval. So we measure the time
                // we would like the next poll to happen before starting
                // the current poll.
                var nextPollTime = DateTime.Now + config.PollInterval;

                await TickPoll();

                var now = DateTime.Now;
                if (now >= nextPollTime)
                {
                    continue; // skip delay
                }

                try
                {
                    await Task.Delay(nextPollTime - now, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    // ignore
                }
            }
        }

        private async Task TickPoll()
        {
            var currentBlockNumber = await FetchCurrentBlockNumber();

            if (currentBlockNumber == lastBlockNumber)
            {
                return;
            }

            if (currentBlockNumber < lastBlockNumber)
            {
                throw new Web3Exception(
                    $"New block number ({currentBlockNumber}) is lower than the last registered ({lastBlockNumber}). " +
                    "This should never happen.");
            }

            var fromBlockNumber = lastBlockNumber;
            var toBlockNumber = currentBlockNumber;
            lastBlockNumber = currentBlockNumber;

            foreach (var subscription in subscriptions.Values)
            {
                await subscription.ParseAndRaiseEvents(this, fromBlockNumber, toBlockNumber);
            }
        }

        private async Task<ulong> FetchCurrentBlockNumber() => (await rpcProvider.GetBlockNumber()).ToUlong();

        private abstract class Subscription
        {
            public abstract Task ParseAndRaiseEvents(PollingEventManager manager, ulong fromBlock, ulong toBlock);
        }

        private class Subscription<TEvent> : Subscription
            where TEvent : IEventDTO, new()
        {
            private readonly string[] contractAddresses;
            private readonly object topicFilter;

            public Subscription(string[] contractAddresses)
            {
                this.contractAddresses = contractAddresses;
                topicFilter = Nethereum.Contracts.Event<TEvent>.GetEventABI().GetTopicBuilder().GetSignatureTopic();
            }

            public List<Action<TEvent>> Handlers { get; } = new();

            public override async Task ParseAndRaiseEvents(PollingEventManager manager, ulong fromBlock, ulong toBlock)
            {
                var logs = await manager.rpcProvider.GetLogs(new NewFilterInput
                {
                    FromBlock = new BlockParameter(new HexBigInteger(new BigInteger(fromBlock + 1))), // skipping one block as it should already be processed
                    ToBlock = new BlockParameter(new HexBigInteger(new BigInteger(toBlock))),
                    Address = contractAddresses,
                    Topics = new[] { topicFilter },
                });

                foreach (var log in logs)
                {
                    EventLog<TEvent> decoded;
                    try
                    {
                        decoded = Nethereum.Contracts.Event<TEvent>.DecodeEvent(log);
                    }
                    catch (Exception ex)
                    {
                        throw new Web3Exception($"There was an error processing event log data for event type {nameof(TEvent)}.", ex);
                    }

                    foreach (var handler in Handlers)
                    {
                        try
                        {
                            handler(decoded.Event);
                        }
                        catch (Exception e)
                        {
                            manager.logWriter.LogError($"Error occured in one of the {nameof(TEvent)} handlers: {e.Message}\n{e.StackTrace}");
                        }
                    }
                }
            }
        }
    }
}