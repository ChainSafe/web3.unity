using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Connection;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.WalletConnect.Storage;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialogBase : ConnectionHandlerBehaviour
    {
        protected class WalletOptionConfig
        {
            public WalletModel Data { get; }
            public Action OnClick { get; }

            public WalletOptionConfig(WalletModel data, Action onClick)
            {
                Data = data;
                OnClick = onClick;
            }
        }

        private TaskCompletionSource<bool> tcs;
        private bool visible;
        private ConnectionHandlerConfig config;

        protected abstract void PlayShowDialog();
        protected abstract void PlayHideDialog();

        protected abstract void SetRedirectOptionsVisible(bool visible);
        protected abstract void SetQrCodeElementVisible(bool visible);
        protected abstract void SetSingleButtonForRedirectVisible(bool visible);
        protected abstract void SpawnRedirectOptions(List<WalletOptionConfig> supportedWallets);
        protected abstract void CreateQrCodeElement(QrCodeBuilder builder);
        protected abstract void ClearDynamicElements();

        public override Task ConnectUserWallet(ConnectionHandlerConfig config)
        {
            this.config = config;

            if (visible)
            {
                throw new Exception("Tried showing dialog, but it's already visible. This shouldn't happen.");
            }

            visible = true;

            ResetDialog();

            var localEnabled = config.WalletLocationOption.LocalEnabled();
            var remoteEnabled = config.WalletLocationOption.RemoteEnabled();

            if (localEnabled)
            {
                if (!config.DelegateLocalWalletSelectionToOs)
                {
                    SetRedirectOptionsVisible(true);

                    var walletOptionConfigs = config.LocalWalletOptions
                        .Select(data => new WalletOptionConfig(data, () => OnLocalWalletButtonClick(data.Name)))
                        .ToList();

                    SpawnRedirectOptions(walletOptionConfigs);
                }
                else
                {
                    SetSingleButtonForRedirectVisible(true);
                }
            }

            if (remoteEnabled)
            {
                SetQrCodeElementVisible(true);
                CreateQrCodeElement(new QrCodeBuilder(config.ConnectRemoteWalletUri));
            }

            PlayShowDialog();

            tcs = new TaskCompletionSource<bool>();
            return tcs.Task;
        }

        public override void Terminate()
        {
            if (!visible)
            {
                Debug.LogError("Tried hiding dialog, but it's already hidden. This shouldn't happen.");
                return;
            }

            visible = false;

            PlayHideDialog();
        }

        protected void OpenLocalWalletOsManaged()
        {
            try
            {
                config.RedirectOsManaged();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        protected void OnException(Exception e)
        {
            tcs.SetException(e);
        }

        private void ResetDialog()
        {
            SetRedirectOptionsVisible(false);
            SetSingleButtonForRedirectVisible(false);
            SetQrCodeElementVisible(false);
            ClearDynamicElements();
        }

        private void OnLocalWalletButtonClick(string walletName)
        {
            try
            {
                config.RedirectToWallet(walletName);
            }
            catch (Exception e)
            {
                OnException(e);
            }

        }
    }
}