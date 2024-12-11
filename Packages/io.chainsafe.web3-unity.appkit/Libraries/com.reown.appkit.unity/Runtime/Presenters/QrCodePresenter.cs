using System;
using System.Collections.Generic;
using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public class QrCodePresenter : Presenter<QrCodeView>
    {
        public override string Title
        {
            get => "QR Code";
        }

        private WalletConnectConnectionProposal _connectionProposal;
        private bool _disposed;

        public QrCodePresenter(RouterController router, VisualElement parent, bool hideView = true) : base(router, parent, hideView)
        {
            View.copyLink.Clicked += OnCopyLinkClicked;

            AppKit.AccountConnected += AccountConnectedHandler;
        }

        protected override QrCodeView CreateViewInstance()
        {
            var view = Parent.Q<QrCodeView>() ?? new QrCodeView();
            return view;
        }

        protected override void OnVisibleCore()
        {
            base.OnVisibleCore();

            if (!AppKit.ConnectorController
                    .TryGetConnector<WalletConnectConnector>
                        (ConnectorType.WalletConnect, out var connector))
                throw new Exception("No WC connector"); // TODO: use custom exception

            if (_connectionProposal == null || _connectionProposal.IsConnected)
            {
                _connectionProposal = (WalletConnectConnectionProposal)connector.Connect();
                _connectionProposal.ConnectionUpdated += ConnectionProposalUpdatedHandler;
            }

            View.qrCode.Data = _connectionProposal.Uri;

            if (WalletUtils.TryGetLastViewedWallet(out var wallet))
            {
                View.EnableWalletIcon(wallet.Image);
            }
            else
            {
                View.DisableWalletIcon();
            }
        }

        private void OnCopyLinkClicked()
        {
            GUIUtility.systemCopyBuffer = _connectionProposal.Uri;
            AppKit.NotificationController.Notify(NotificationType.Success, "Link copied to clipboard");
        }

        private void ConnectionProposalUpdatedHandler(ConnectionProposal connectionProposal)
        {
            View.qrCode.Data = _connectionProposal.Uri;
        }

        private void AccountConnectedHandler(object sender, Connector.AccountConnectedEventArgs e)
        {
            if (!IsVisible)
                return;

            AppKit.EventsController.SendEvent(new Event
            {
                name = "CONNECT_SUCCESS",
                properties = new Dictionary<string, object>
                {
                    { "method", "qrcode" },
                    { "name", "WalletConnect" }
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                AppKit.AccountConnected -= AccountConnectedHandler;
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}