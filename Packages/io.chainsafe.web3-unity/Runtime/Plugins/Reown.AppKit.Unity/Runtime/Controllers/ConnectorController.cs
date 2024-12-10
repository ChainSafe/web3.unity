using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reown.Sign.Unity;
using UnityEngine;

namespace Reown.AppKit.Unity
{
    public class ConnectorController : Connector
    {
        private readonly Dictionary<ConnectorType, Connector> _connectors = new();

        public override bool IsAccountConnected
        {
            get
            {
                if (ActiveConnector == null)
                    return false;

                return ActiveConnector.IsAccountConnected;
            }
        }

        public Connector ActiveConnector
        {
            get => _activeConnector;
            private set
            {
                if (_activeConnector == value)
                    return;

                _activeConnector = value;

                if (value != null)
                    Type = value.Type;
            }
        }

        private Connector _activeConnector;

        protected override async Task InitializeAsyncCore(AppKitConfig config, SignClientUnity signClient)
        {
            DappSupportedChains = config.supportedChains;

#if UNITY_WEBGL && !UNITY_EDITOR
            // --- WebGl Connector
            var webGlConnector = new WebGlConnector();
            webGlConnector.AccountConnected += (_, e) => ConnectorAccountConnected(webGlConnector, e);
            webGlConnector.AccountDisconnected += ConnectorAccountDisconnectedHandler;
            webGlConnector.AccountChanged += AccountChangedHandler;
            webGlConnector.ChainChanged += ChainChangedHandler;

            _connectors.Add(ConnectorType.WebGl, webGlConnector);
            
            // Only one connector is supported on WebGL
            ActiveConnector = webGlConnector;
#else
            // --- WalletConnect Connector
            var walletConnectConnector = new WalletConnectConnector();
            walletConnectConnector.AccountConnected += (_, e) => ConnectorAccountConnected(walletConnectConnector, e);
            walletConnectConnector.AccountDisconnected += ConnectorAccountDisconnectedHandler;
            walletConnectConnector.AccountChanged += AccountChangedHandler;
            walletConnectConnector.ChainChanged += ChainChangedHandler;
            walletConnectConnector.SignatureRequested += SignatureRequestedHandler;
            _connectors.Add(ConnectorType.WalletConnect, walletConnectConnector);
#endif

            await Task.WhenAll(_connectors.Values.Select(c => c.InitializeAsync(config, signClient)));
        }

        protected override async Task<bool> TryResumeSessionAsyncCore()
        {
            if (ActiveConnector != null)
                return await ActiveConnector.TryResumeSessionAsync();

            if (!TryGetLastConnector(out var connectorType))
                return false;

            var connector = _connectors[connectorType];
            var sessionResumed = await connector.TryResumeSessionAsync();

            if (sessionResumed)
                ActiveConnector = connector;

            return sessionResumed;
        }

        // ConnectorController creates WC connection. 
        // All other connections are created by their respective connectors.
        protected override ConnectionProposal ConnectCore()
        {
            if (!TryGetConnector<WalletConnectConnector>(ConnectorType.WalletConnect, out var wcConnector))
                throw new Exception("No WC connector"); // TODO: use custom exception

            return wcConnector.Connect();
        }

        protected override async Task DisconnectAsyncCore()
        {
            await ActiveConnector.DisconnectAsync();
        }

        protected override Task ChangeActiveChainAsyncCore(Chain chain)
        {
            return ActiveConnector.ChangeActiveChainAsync(chain);
        }

        protected override Task<Account> GetAccountAsyncCore()
        {
            return ActiveConnector.GetAccountAsync();
        }

        protected override Task<Account[]> GetAccountsAsyncCore()
        {
            return ActiveConnector.GetAccountsAsync();
        }

        public bool TryGetConnector<T>(ConnectorType connectorType, out T connector) where T : Connector
        {
            var ok = _connectors.TryGetValue(connectorType, out var uncasedConnector);
            connector = (T)uncasedConnector;
            return ok;
        }

        private static bool TryGetLastConnector(out ConnectorType connectorType)
        {
            const string key = "RE_LAST_CONNECTOR_TYPE";

            if (PlayerPrefs.HasKey(key))
            {
                var connectorTypeInt = PlayerPrefs.GetInt(key);
                connectorType = (ConnectorType)connectorTypeInt;
                return connectorType != ConnectorType.None;
            }

            connectorType = ConnectorType.None;
            return false;
        }

        private void ConnectorAccountConnected(Connector connector, AccountConnectedEventArgs e)
        {
            PlayerPrefs.SetInt("RE_LAST_CONNECTOR_TYPE", (int)connector.Type);
            ActiveConnector = connector;

            OnAccountConnected(e);
        }

        private void ConnectorAccountDisconnectedHandler(object sender, EventArgs e)
        {
            ActiveConnector = null;
            OnAccountDisconnected(AccountDisconnectedEventArgs.Empty);
        }

        private void AccountChangedHandler(object sender, AccountChangedEventArgs e)
        {
            OnAccountChanged(e);
        }

        private void ChainChangedHandler(object sender, ChainChangedEventArgs e)
        {
            OnChainChanged(e);
        }

        private void SignatureRequestedHandler(object sender, SignatureRequest e)
        {
            PlayerPrefs.SetInt("RE_LAST_CONNECTOR_TYPE", (int)e.Connector.Type);
            ActiveConnector = e.Connector;

            OnSignatureRequested();
        }

        protected override void ConnectionConnectedHandler(ConnectionProposal connectionProposal)
        {
            PlayerPrefs.SetInt("RE_LAST_CONNECTOR_TYPE", (int)connectionProposal.connector.Type);
            AppKit.NotificationController.Notify(NotificationType.Success, "Connected!");

            base.ConnectionConnectedHandler(connectionProposal);
        }
    }
}