using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Reown.AppKit.Unity
{
    public abstract class NetworkController
    {
        public bool IsInitialized { get; set; }

        public Chain ActiveChain { get; protected set; }

        public ReadOnlyDictionary<string, Chain> Chains { get; protected set; }

        protected ConnectorController ConnectorController { get; private set; }

        public event EventHandler<ChainChangedEventArgs> ChainChanged;

        public NetworkController()
        {
        }

        public async Task InitializeAsync(ConnectorController connectorController, IEnumerable<Chain> supportedChains)
        {
            if (IsInitialized)
                throw new Exception("Already initialized"); // TODO: use custom ex type

            if (supportedChains == null)
                throw new ArgumentNullException(nameof(supportedChains));

            ConnectorController = connectorController ?? throw new ArgumentNullException(nameof(connectorController));

            ConnectorController.ChainChanged += ConnectorChainChangedHandler;
            ConnectorController.AccountConnected += ConnectorAccountConnectedHandler;

            await InitializeAsyncCore(supportedChains);

            Assert.IsFalse(Chains == null || Chains.Count == 0, "No chains initialized");

            IsInitialized = true;
        }

        public async Task ChangeActiveChainAsync(Chain chain)
        {
            if (chain == null)
                throw new ArgumentNullException(nameof(chain));

            if (!Chains.Values.Contains(chain))
                throw new Exception("Chain is not supported"); // TODO: use custom ex type

            await ChangeActiveChainAsyncCore(chain);
        }

        private void ConnectorChainChangedHandler(object sender, Connector.ChainChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.ChainId))
                throw new ArgumentNullException(nameof(e.ChainId));

            ConnectorChainChangedHandlerCore(sender, e);
        }

        private void ConnectorAccountConnectedHandler(object sender, Connector.AccountConnectedEventArgs e)
        {
            ConnectorAccountConnectedHandlerCore(sender, e);
        }

        protected virtual void OnChainChanged(ChainChangedEventArgs e)
        {
            if (e.PreviousChain?.ChainId == e.NewChain?.ChainId)
                return;
            
            ChainChanged?.Invoke(this, e);
        }

        protected abstract Task InitializeAsyncCore(IEnumerable<Chain> supportedChains);

        protected abstract Task ChangeActiveChainAsyncCore(Chain chain);

        protected abstract void ConnectorChainChangedHandlerCore(object sender, Connector.ChainChangedEventArgs e);

        protected abstract void ConnectorAccountConnectedHandlerCore(object sender, Connector.AccountConnectedEventArgs accountConnectedEventArgs);

        public class ChainChangedEventArgs : EventArgs
        {
            public Chain PreviousChain { get; }
            public Chain NewChain { get; }

            public ChainChangedEventArgs(Chain previousChain, Chain newChain)
            {
                PreviousChain = previousChain;
                NewChain = newChain;
            }
        }
    }
}