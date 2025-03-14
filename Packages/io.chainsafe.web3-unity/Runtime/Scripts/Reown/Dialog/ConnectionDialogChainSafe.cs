using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment.Http;
using UnityEngine;
using UnityEngine.UI;
using ZXing.QrCode;

namespace ChainSafe.Gaming.Reown.Dialog
{
    public class ConnectionDialogChainSafe : ConnectionDialogBase
    {
        [Header("QR Code")]
        public GameObject qrCodePanel;
        public RawImage qrCodeImage;
        public Vector2Int qrCodeSize;
        public int qrCodePadding;
        
        [Header("Local Wallets")]
        public GameObject localWalletsPanel;
        public LocalWalletButton localWalletButtonPrefab;
        public RectTransform localWalletsContainer;
        
        [Header("Single Local Wallet")]
        public GameObject singleLocalWalletPanel;
        public Button singleLocalWalletButton;
        
        [Header("Other")]
        public GameObject separator;
        public Button closeButton;
        public ScrollRect scrollRectToReset;

        private LocalWalletButton[] loadedLocalWalletButtons;

        private void Awake()
        {
            singleLocalWalletButton.onClick.AddListener(OpenLocalWalletOsManaged);
            closeButton.onClick.AddListener(Close);
        }

        protected override void PlayShowDialog()
        {
            gameObject.SetActive(true); // GuiScreen manages animation
            scrollRectToReset.verticalNormalizedPosition = 1f; // Reset ScrollRect
        }

        protected override void PlayHideDialog()
        {
            gameObject.SetActive(false);
        }

        protected override void SetRedirectOptionsVisible(bool visible)
        {
            localWalletsPanel.SetActive(visible);
            UpdateSeparatorVisibility();
        }

        protected override void SetQrCodeElementVisible(bool visible)
        {
            qrCodePanel.SetActive(visible);
            UpdateSeparatorVisibility();
        }

        protected override void SetSingleButtonForRedirectVisible(bool visible)
        {
            singleLocalWalletPanel.SetActive(visible);
            UpdateSeparatorVisibility();
        }

        protected override void SpawnRedirectOptions(
            List<WalletOptionConfig> supportedWallets,
            string getWalletIconEndpoint,
            HttpHeader[] httpHeaders)
        {
            loadedLocalWalletButtons = supportedWallets
                .Select(w =>
                {
                    var button = Instantiate(localWalletButtonPrefab, localWalletsContainer);
                    button.Set(w.Data, getWalletIconEndpoint, httpHeaders, w.OnClick);
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
            OnException(new TaskCanceledException("User closed the Reown connection dialog."));
        }

        private void UpdateSeparatorVisibility()
        {
            var separatorVisible = qrCodePanel.activeSelf &&
                                   (localWalletsPanel.activeSelf || singleLocalWalletPanel.activeSelf);
            
            separator.SetActive(separatorVisible);
        }
    }
}