using System;
using Reown.AppKit.Unity.Utils;
using Reown.AppKit.Unity.Views.SiweView;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public class SiwePresenter : Presenter<SiweView>
    {
        public override string Title
        {
            get => "Sign In";
        }

        public override bool EnableCloseButton
        {
            get => false;
        }

        private SignatureRequest _lastSignatureRequest;
        private bool _success;
        private bool _disposed;
        private RemoteSprite<Image> _walletLogo;
        
        public SiwePresenter(RouterController router, VisualElement parent, bool hideView = true) : base(router, parent, hideView)
        {
            var appName = AppKit.Config.metadata.Name;
            if (!string.IsNullOrWhiteSpace(appName))
            {
                View.Title = $"{appName} wants to connect to your wallet";
            }

            // Load app logo on Presenter creation because it's static
            var appLogoUrl = AppKit.Config.metadata.IconUrl;
            if (!string.IsNullOrWhiteSpace(appLogoUrl))
            {
                RemoteSpriteFactory
                    .GetRemoteSprite<Image>(appLogoUrl)
                    .SubscribeImage(View.LogoAppImage);
            }

            View.CancelButtonClicked += RejectButtonClickedHandler;
            View.ApproveButtonClicked += ApproveButtonClickedHandler;
            
            AppKit.SiweController.Config.SignInSuccess += SignInSuccessHandler;
            AppKit.SiweController.Config.SignOutSuccess += SignOutSuccessHandler;

            AppKit.ConnectorController.SignatureRequested += SignatureRequestedHandler;
        }

        private void SignInSuccessHandler(SiweSession siweSession)
        {
            _success = true;
            AppKit.CloseModal();
        }

        private void SignOutSuccessHandler()
        {
            Router.GoBack();
        }

        private void SignatureRequestedHandler(object sender, SignatureRequest e)
        {
            _lastSignatureRequest = e;
        }

        protected override void OnVisibleCore()
        {
            base.OnVisibleCore();

            _walletLogo?.UnsubscribeImage(View.LogoWalletImage);

            if (WalletUtils.TryGetRecentWallet(out var wallet))
            {
                _walletLogo = wallet.Image;
                _walletLogo.SubscribeImage(View.LogoWalletImage);
            }

            View.ButtonsEnabled = true;
        }

        protected override async void OnHideCore()
        {
            base.OnHideCore();

            if (!_success)
            {
                await AppKit.ConnectorController.DisconnectAsync();
            }
        }

        public async void RejectButtonClickedHandler()
        {
            try
            {
                View.ButtonsEnabled = false;
                AppKit.NotificationController.Notify(NotificationType.Info, "Disconnecting...");

                if (_lastSignatureRequest == null) // This shouldn't happen, but it's better to have a fallback
                {
                    await AppKit.ConnectorController.DisconnectAsync();
                }
                else
                {
                    await _lastSignatureRequest.RejectAsync();
                }
            }
            catch (Exception)
            {
                View.ButtonsEnabled = true;
                throw;
            }
        }

        public async void ApproveButtonClickedHandler()
        {
            try
            {
                View.ButtonsEnabled = false;
                await _lastSignatureRequest.ApproveAsync();
            }
            catch (Exception)
            {
                View.ButtonsEnabled = true;
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                AppKit.SiweController.Config.SignInSuccess -= SignInSuccessHandler;
                AppKit.SiweController.Config.SignOutSuccess -= SignOutSuccessHandler;
                AppKit.ConnectorController.SignatureRequested -= SignatureRequestedHandler;
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}