using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reown.Core.Common.Model.Errors;
using Reown.Sign.Interfaces;
using Reown.Sign.Models;
using Reown.Sign.Models.Engine;
using Reown.Sign.Unity;
using Reown.Sign.Utils;
using UnityEngine;

namespace Reown.AppKit.Unity
{
    public class WalletConnectConnectionProposal : ConnectionProposal
    {
        public string Uri { get; private set; } = string.Empty;

        private readonly ISignClient _client;
        private readonly ConnectOptions _connectOptions;
        private readonly SiweController _siweController;
        private readonly WaitForSecondsRealtime _refreshInterval = new(240); // 4 minutes

        private bool _disposed;

        public WalletConnectConnectionProposal(Connector connector, ISignClient signClient, ConnectOptions connectOptions, SiweController siweController) : base(connector)
        {
            _client = signClient;
            _connectOptions = connectOptions;
            _siweController = siweController;

            _client.SessionAuthenticated += SessionAuthenticatedHandler;
            _client.SessionConnected += SessionConnectedHandler;
            _client.SessionConnectionErrored += SessionConnectionErroredHandler;

            RefreshConnection();

            UnityEventsDispatcher.Instance.StartCoroutine(RefreshOnIntervalRoutine());
        }

        private async void SessionAuthenticatedHandler(object sender, SessionAuthenticatedEventArgs e)
        {
            try
            {
                var cacao = e.Auths[0];
                var message = cacao.FormatMessage();

                var isSignatureValid = await _siweController.VerifyMessageAsync(new SiweVerifyMessageArgs
                {
                    Message = message,
                    Signature = cacao.Signature.S,
                    Cacao = cacao
                });

                if (!isSignatureValid)
                {
                    await _client.Disconnect();
                    return;
                }

                var chainId = CacaoUtils.ExtractDidChainId(cacao.Payload.Iss);
                _ = await _siweController.GetSessionAsync(new GetSiweSessionArgs
                {
                    Address = CacaoUtils.ExtractDidAddress(cacao.Payload.Iss),
                    ChainIds = new[]
                    {
                        Core.Utils.ExtractChainReference(chainId)
                    }
                });

                IsSignarureRequested = false;
                IsConnected = true;
                connected?.Invoke(this);
            }
            catch (Exception)
            {
                await _client.Disconnect();
                throw;
            }
        }

        private void SessionConnectedHandler(object sender, SessionStruct e)
        {
            IsSignarureRequested = _siweController.IsEnabled;
            IsConnected = true;
            connected?.Invoke(this);
        }

        private void SessionConnectionErroredHandler(object sender, Exception e)
        {
            AppKit.NotificationController.Notify(NotificationType.Error, e.Message);
            RefreshConnection();

            AppKit.EventsController.SendEvent(new Event
            {
                name = "CONNECT_ERROR",
                properties = new Dictionary<string, object>
                {
                    { "message", e.Message }
                }
            });
        }

        private IEnumerator RefreshOnIntervalRoutine()
        {
            while (!_disposed)
            {
                yield return _refreshInterval;

#pragma warning disable S2589
                if (!_disposed)
                    RefreshConnection();
#pragma warning enable S2589
            }
        }

        private async void RefreshConnection()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(WalletConnectConnectionProposal));

            try
            {
                if (_siweController.IsEnabled)
                {
                    var nonce = await _siweController.GetNonceAsync();
                    var siweParams = _siweController.Config.GetMessageParams();

                    var proposedNamespace = _connectOptions.OptionalNamespaces.Values.First();
                    var chains = proposedNamespace.Chains;
                    var methods = proposedNamespace.Methods;

                    var authParams = new AuthParams(
                        chains,
                        siweParams.Domain,
                        nonce,
                        siweParams.Domain,
                        null,
                        null,
                        siweParams.Statement,
                        null,
                        null,
                        methods
                    );

                    var authData = await _client.Authenticate(authParams);
                    Uri = authData.Uri;

                    connectionUpdated?.Invoke(this);

                    await authData.Approval;
                }
                else
                {
                    var connectedData = await _client.Connect(_connectOptions);
                    Uri = connectedData.Uri;

                    connectionUpdated?.Invoke(this);

                    await connectedData.Approval;
                }
            }
            catch (ReownNetworkException e) when (e.CodeType == ErrorType.DISAPPROVED_CHAINS)
            {
                // Wallet declined connection, don't throw/log.
                // The `SessionConnectionErroredHandler` will handle the error.
            }
            catch (Exception e)
            {
                Debug.LogError($"[WCCP] Exception: {e.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _client.SessionConnectionErrored -= SessionConnectionErroredHandler;

                _disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}