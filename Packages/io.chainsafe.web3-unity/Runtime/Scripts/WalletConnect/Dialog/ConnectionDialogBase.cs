using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Models;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public abstract class ConnectionDialogBase : ConnectionDialog
    {
        protected class WalletOptionConfig
        {
            public WalletConnectWalletModel Data { get; }
            public Action OnClick { get; }

            public WalletOptionConfig(WalletConnectWalletModel data, Action onClick)
            {
                Data = data;
                OnClick = onClick;
            }
        }

        private TaskCompletionSource<bool> tcs;
        private bool visible;
        private ConnectionDialogConfig config;

        protected abstract void PlayShowDialog();
        protected abstract void PlayHideDialog();

        protected abstract void SetRedirectOptionsVisible(bool visible);
        protected abstract void SetQrCodeElementVisible(bool visible);
        protected abstract void SetSingleButtonForRedirectVisible(bool visible);
        protected abstract void SpawnRedirectOptions(List<WalletOptionConfig> supportedWallets);
        protected abstract void CreateQrCodeElement(QrCodeBuilder builder);
        protected abstract void ClearDynamicElements();

        public override Task ShowAndConnectUserWallet(ConnectionDialogConfig config)
        {
            this.config = config;
        
            if (visible)
            {
                throw new Exception("Tried showing dialog, but it's already visible. This shouldn't happen.");
            }
        
            visible = true;
        
            ResetDialog();

            var localEnabled = config.WalletLocationOptions.LocalEnabled();
            var remoteEnabled = config.WalletLocationOptions.RemoteEnabled();

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

        public override void CloseDialog()
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
            config.RedirectOsManaged();
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