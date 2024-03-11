using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZXing.QrCode;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public class ConnectionDialogChainSafe : ConnectionDialogBase
    {
        public new Animation animation;
        public string showAnimationClipName;
        public string hideAnimationClipName;
        public RawImage qrCodeImage;
        public Vector2Int qrCodeSize = new(512, 512);
        public int qrCodePadding = 20;
        public GameObject qrCodePanel;
        public GameObject localWalletButtonsPanel;
        public LocalWalletButton localWalletButtonPrefab;
        public RectTransform localWalletButtonsContainer;
        public GameObject singleButtonForLocalWalletsPanel;
        public Button singleButtonForLocalWallets;

        private LocalWalletButton[] loadedLocalWalletButtons;

        private void Awake()
        {
            singleButtonForLocalWallets.onClick.AddListener(OpenLocalWalletOsManaged);
        }

        protected override void PlayShowDialog() => animation.Play(showAnimationClipName);
        protected override void PlayHideDialog() => animation.Play(hideAnimationClipName);

        protected override void SetRedirectOptionsVisible(bool visible) => localWalletButtonsPanel.SetActive(visible);
        protected override void SetQrCodeElementVisible(bool visible) => qrCodePanel.SetActive(visible);

        protected override void SetSingleButtonForRedirectVisible(bool visible) =>
            singleButtonForLocalWalletsPanel.SetActive(visible);

        protected override void SpawnRedirectOptions(List<WalletOptionConfig> supportedWallets)
        {
            loadedLocalWalletButtons = supportedWallets
                .Select(w =>
                {
                    var button = Instantiate(localWalletButtonPrefab, localWalletButtonsContainer);
                    button.Set(w.Data, w.OnClick);
                    return button;
                })
                .ToArray();
        }

        protected override void CreateQrCodeElement(QrCodeBuilder builder)
        {
            qrCodeImage.texture = builder.GenerateQrCode(new QrCodeEncodingOptions
            {
                Width = qrCodeSize.x,
                Height = qrCodeSize.y,
                Margin = qrCodePadding
            }
            );
        }

        protected override void ClearDynamicElements()
        {
            if (loadedLocalWalletButtons != null)
            {
                foreach (var localWalletButton in loadedLocalWalletButtons)
                {
                    Destroy(localWalletButton.gameObject);
                }

                loadedLocalWalletButtons = null;
            }
        }

        public void Close()
        {
            OnException(new Exception("User closed the connection dialog."));
        }
    }
}