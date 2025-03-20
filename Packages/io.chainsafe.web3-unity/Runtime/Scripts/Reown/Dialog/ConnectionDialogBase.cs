using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Connection;
using ChainSafe.Gaming.Reown.Models;
using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3.Environment.Http;
using UnityEngine;

namespace ChainSafe.Gaming.Reown.Dialog
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
        protected abstract void SpawnRedirectOptions(
            List<WalletOptionConfig> supportedWallets,
            string getWalletIconEndpoint,
            HttpHeader[] httpHeaders);
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

                    var walletOptionConfigs = config.LocalWalletOptions
                        .Select(data => new WalletOptionConfig(data, () => OnLocalWalletButtonClick(data.Id)))
                        .ToList();
                    
                    var localWalletsAvailable = walletOptionConfigs.Any();

                    if (!localWalletsAvailable)
                    {
                        Debug.Log("Local wallet selection is enabled, but there are no wallets supported for the current platform meeting the provided filters. Disabling local wallet connection option...");
                    }

                    SetRedirectOptionsVisible(localWalletsAvailable);
                    SpawnRedirectOptions(walletOptionConfigs, config.WalletIconEndpoint, config.HttpHeaders);
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

        private void OnLocalWalletButtonClick(string walletId)
        {
            try
            {
                config.RedirectToWallet(walletId);
            }
            catch (Exception e)
            {
                OnException(e);
            }

        }
    }
}