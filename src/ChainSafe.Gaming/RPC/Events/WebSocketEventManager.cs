using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Reactive.Eth.Subscriptions;

namespace ChainSafe.Gaming.RPC.Events
{
    public class WebSocketEventManager : IEventManager, ILifecycleParticipant, IChainSwitchHandler
    {
        private readonly IChainConfig chainConfig;
        private readonly Dictionary<EventDictionaryKey, Subscription> subscriptions = new();
        private readonly ILogWriter logWriter;

        private StreamingWebSocketClient webSocketClient;

        public WebSocketEventManager(IChainConfig chainConfig, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.chainConfig = chainConfig;
        }

        public int ExecutionOrder => 10;

        public async ValueTask WillStartAsync()
        {
            if (string.IsNullOrWhiteSpace(chainConfig.Ws))
            {
                throw new Web3Exception("No WebSocket URL was provided in config. Please toggle the ForceEventPolling to true in order to use event polling instead of WebSockets.");
            }

            webSocketClient = new StreamingWebSocketClient(chainConfig.Ws);
            await webSocketClient.StartAsync();
        }

        public async ValueTask WillStopAsync()
        {
            for (var i = subscriptions.Count - 1; i >= 0; i--)
            {
                var tuple = subscriptions.Last().Key;
                await TerminateSubscriptionForType(tuple.EventType, tuple.ContractAddresses);
            }

            webSocketClient?.Dispose();
        }

        public async Task Subscribe<TEvent>(Action<TEvent> handler, params string[] contractAddresses)
            where TEvent : IEventDTO, new()
        {
            var tuple = new EventDictionaryKey()
            {
                EventType = typeof(TEvent),
                ContractAddresses = contractAddresses,
            };
            if (!subscriptions.TryGetValue(tuple, out var rawSubscription))
            {
                rawSubscription = await InitializeSubscriptionForType<TEvent>(contractAddresses);
            }

            var subscription = (Subscription<TEvent>)rawSubscription;
            subscription.Handlers.Add(handler);
        }

        public async Task Unsubscribe<TEvent>(Action<TEvent> handler, params string[] contractAddresses)
            where TEvent : IEventDTO, new()
        {
            var tuple = new EventDictionaryKey()
            {
                EventType = typeof(TEvent),
                ContractAddresses = contractAddresses,
            };
            if (!subscriptions.ContainsKey(tuple))
            {
                throw new Web3Exception(contractAddresses.Length == 0
                    ? $"Can't unsubscribe. No subscription of type {typeof(TEvent)} was registered."
                    : $"Can't unsubscribe. No subscription of type {typeof(TEvent)} was registered with contract filter " +
                      $"addresses: {string.Join(", ", contractAddresses)}.");
            }

            var subscription = (Subscription<TEvent>)subscriptions[tuple];
            subscription.Handlers.Remove(handler);

            if (subscription.Handlers.Count == 0)
            {
                await TerminateSubscriptionForType<TEvent>(contractAddresses);
            }
        }

        async Task IChainSwitchHandler.HandleChainSwitching()
        {
            // unsubscribe
            foreach (var subscription in subscriptions.Values)
            {
                if (subscription.NethSubscription is not null)
                {
                    await subscription.NethSubscription.UnsubscribeAsync();
                    subscription.NethSubscription = null;
                }
            }

            // dispose web socket client
            if (webSocketClient is not null)
            {
                webSocketClient?.Dispose();
                webSocketClient = null;
            }

            // initialize a new web socket client
            webSocketClient = new StreamingWebSocketClient(chainConfig.Ws);

            // subscribe with a new web socket client
            foreach (var subscription in subscriptions.Values)
            {
                subscription.NethSubscription = new EthLogsObservableSubscription(webSocketClient);
                subscription.NethSubscription
                    .GetSubscriptionDataResponsesAsObservable()
                    .Subscribe(new FilterLogObserver(subscription.LogHandleAction));

                await subscription.NethSubscription.SubscribeAsync(subscription.EventFilter);
            }
        }

        private async Task<Subscription> InitializeSubscriptionForType<TEvent>(string[] contractAddresses)
            where TEvent : IEventDTO, new()
        {
            Subscription rawSubscription = new Subscription<TEvent>(webSocketClient);
            rawSubscription.EventFilter = Event<TEvent>.GetEventABI().CreateFilterInput(contractAddresses);
            var tuple = new EventDictionaryKey()
            {
                EventType = typeof(TEvent),
                ContractAddresses = contractAddresses,
            };
            rawSubscription.LogHandleAction = HandleLog;
            rawSubscription
                .NethSubscription
                .GetSubscriptionDataResponsesAsObservable()
                .Subscribe(new FilterLogObserver(rawSubscription.LogHandleAction));

            await rawSubscription.NethSubscription.SubscribeAsync(rawSubscription.EventFilter);

            subscriptions[tuple] = rawSubscription;
            return rawSubscription;

            void HandleLog(FilterLog log)
            {
                EventLog<TEvent> decoded;
                try
                {
                    decoded = Event<TEvent>.DecodeEvent(log);
                }
                catch (Exception ex)
                {
                    throw new Web3Exception($"There was an error processing event log data for event type {nameof(TEvent)}.", ex);
                }

                var subscription = (Subscription<TEvent>)rawSubscription;
                foreach (var subscriptionHandler in subscription.Handlers)
                {
                    try
                    {
                        subscriptionHandler.Invoke(decoded.Event);
                    }
                    catch (Exception e)
                    {
                        logWriter.LogError($"Error occured in one of the {nameof(TEvent)} handlers: {e.Message}\n{e.StackTrace}");
                    }
                }
            }
        }

        private Task TerminateSubscriptionForType<TEvent>(string[] contractAddresses)
            where TEvent : IEventDTO, new()
        {
            return TerminateSubscriptionForType(typeof(TEvent), contractAddresses);
        }

        private Task TerminateSubscriptionForType(Type type, string[] contractAddresses)
        {
            var tuple = new EventDictionaryKey()
            {
                EventType = type,
                ContractAddresses = contractAddresses,
            };
            var subscription = subscriptions[tuple];
            subscriptions.Remove(tuple);
            return subscription.NethSubscription.UnsubscribeAsync();
        }

        private abstract class Subscription
        {
            public EthLogsObservableSubscription NethSubscription { get; set; }

            public Action<FilterLog> LogHandleAction { get; set; }

            public NewFilterInput EventFilter { get; set; }
        }

        private class Subscription<TEvent> : Subscription
        {
            public Subscription(StreamingWebSocketClient webSocketClient)
            {
                NethSubscription = new EthLogsObservableSubscription(webSocketClient);
                Handlers = new List<Action<TEvent>>();
            }

            public List<Action<TEvent>> Handlers { get; set; }
        }

        private class FilterLogObserver : IObserver<FilterLog>
        {
            private readonly Action<FilterLog> handler;

            public FilterLogObserver(Action<FilterLog> handler)
            {
                this.handler = handler;
            }

            public void OnCompleted()
            {
                // empty
            }

            public void OnError(Exception error)
            {
                // empty
            }

            public void OnNext(FilterLog value)
            {
                handler.Invoke(value);
            }
        }
    }
}