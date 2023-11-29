using System;
using System.Collections.Generic;
using System.Linq;
using MetaMask.SocketIOClient;
using MetaMask.Unity.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace MetaMask.Unity
{
    public class MetaMaskUnityEventListener : BindableMonoBehavior
    {
        [Inject]
        private MetaMaskUnityEventHandler _eventHandler;
        
        public MetaMaskConnectedEvent MetaMaskConnected = new MetaMaskConnectedEvent();
        public MetaMaskWalletReadyEvent MetaMaskWalletReady = new MetaMaskWalletReadyEvent();
        public MetaMaskWalletPausedEvent MetaMaskWalletPaused = new MetaMaskWalletPausedEvent();
        public MetaMaskConnectingEvent MetamaskConnecting = new MetaMaskConnectingEvent();
        public MetaMaskWalletDisconnectedEvent MetaMaskWalletDisconnected = new MetaMaskWalletDisconnectedEvent();
        public MetaMaskWalletAccountChangedEvent MetaMaskWalletAccountChanged = new MetaMaskWalletAccountChangedEvent();
        public MetaMaskChainIdChangedEvent MetaMaskChainIdChanged = new MetaMaskChainIdChangedEvent();
        public MetaMaskWalletAuthorizedEvent MetaMaskWalletAuthorized = new MetaMaskWalletAuthorizedEvent();
        public MetaMaskWalletUnauthorizedEvent MetaMaskWalletUnauthorized = new MetaMaskWalletUnauthorizedEvent();
        public MetaMaskWalletEthereumRequestResultEvent MetaMaskWalletEthereumRequestResult =
            new MetaMaskWalletEthereumRequestResultEvent();
        public MetaMaskWalletRequestFailedEvent MetaMaskWalletRequestFailed = new MetaMaskWalletRequestFailedEvent();
        public MetaMaskStartConnectingEvent MetaMaskWalletStartConnecting = new MetaMaskStartConnectingEvent();
        
        private List<Action> TeardownActions;
        
        internal void SetupEvents()
        {
            if (_eventHandler == null)
                UnityBinder.Inject(this);
            
            // 1. Unity Event
            // 2. Getter for .NET Event Handler
            // 3. Getter for Unity Event Handler
            // 4. Function to update .NET Event Handler
            var allEvents = new (UnityEvent, Func<EventHandler>, Action<EventHandler>)[]
            {
                (MetaMaskConnected, () => this._eventHandler.WalletConnectedHandler, (eh) => _eventHandler.WalletConnectedHandler = eh),
                (MetaMaskWalletReady, () => this._eventHandler.WalletReadyHandler, (eh) => this._eventHandler.WalletReadyHandler = eh),
                (MetaMaskWalletPaused, () => this._eventHandler.WalletPausedHandler, (eh) => this._eventHandler.WalletPausedHandler = eh),
                (MetaMaskWalletDisconnected, () => this._eventHandler.WalletDisconnectedHandler, (eh) => this._eventHandler.WalletDisconnectedHandler = eh),
                (MetaMaskWalletAccountChanged, () => this._eventHandler.AccountChangedHandler, (eh) => this._eventHandler.AccountChangedHandler = eh),
                (MetaMaskChainIdChanged, () => this._eventHandler.ChainIdChangedHandler, (eh) => this._eventHandler.ChainIdChangedHandler = eh),
                (MetaMaskWalletAuthorized, () => this._eventHandler.WalletAuthorizedHandler, (eh) => this._eventHandler.WalletAuthorizedHandler = eh),
                (MetaMaskWalletUnauthorized, () => this._eventHandler.WalletUnauthorizedHandler, (eh) => this._eventHandler.WalletUnauthorizedHandler = eh),
            };

            TeardownActions = allEvents.Select((e) => SetupEvent(e.Item1, e.Item2, e.Item3)).ToList();

            TeardownActions.Add(SetupEvent(
                MetaMaskWalletStartConnecting,
                () => this._eventHandler.StartConnectingHandler,
                (eh) => this._eventHandler.StartConnectingHandler = eh));

            TeardownActions.Add(SetupEvent(
                MetaMaskWalletEthereumRequestResult,
                () => this._eventHandler.EthereumRequestResultReceivedHandler,
                (eh) => this._eventHandler.EthereumRequestResultReceivedHandler = eh));

            TeardownActions.Add(SetupEvent(
                MetaMaskWalletRequestFailed,
                () => _eventHandler.EthereumRequestFailedHandler,
                (eh) => _eventHandler.EthereumRequestFailedHandler = eh));
        }
        
        private Action SetupEvent(UnityEvent @event, Func<EventHandler> sourceGetter, Action<EventHandler> setter)
        {
            void EventTriggered(object sender, EventArgs e)
            {
                UnityThread.executeInUpdate(() =>
                {
                    @event?.Invoke();
                });
            }

            var source = sourceGetter();
            source += EventTriggered;

            setter(source);

            return () =>
            {
                var currentSource = sourceGetter();
                currentSource -= EventTriggered;
                setter(currentSource);
            };
        }
        
        private Action SetupEvent<T>(UnityEvent<T> @event, Func<EventHandler<T>> sourceGetter, Action<EventHandler<T>> setter) where T : EventArgs
        {
            void EventTriggered(object sender, T e)
            {
                UnityThread.executeInUpdate(() =>
                {
                    @event?.Invoke(e);
                });
            }

            var source = sourceGetter();
            source += EventTriggered;

            setter(source);

            return () =>
            {
                var currentSource = sourceGetter();
                currentSource -= EventTriggered;
                setter(currentSource);
            };
        }
        
        private void OnDestroy()
        {
            TeardownEvents();
        }
        
        private void TeardownEvents()
        {
            if (TeardownActions == null)
                return;
            
            foreach (var action in TeardownActions.Where(action => action != null))
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogError("Error during MetaMask Event teardown");
                    Debug.LogError(e);
                }
            }
            
            TeardownActions.Clear();
        }
    }
}