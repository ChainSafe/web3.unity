using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class QrCodeView : VisualElement
    {
        public const string Name = "qrcode-view";
        public static readonly string NameSubtitle = $"{Name}__subtitle";
        public static readonly string NameCodeContainer = $"{Name}__code-container";
        public static readonly string NameIconContainer = $"{Name}__icon-container";
        public static readonly string NameLogoContainer = $"{Name}__logo-container";
        public static readonly string NameLogo = $"{Name}__logo";
        public static readonly string NameWalletIconContainer = $"{Name}__wallet-icon-container";
        public static readonly string NameWalletIcon = $"{Name}__wallet-icon";
        public static readonly string NameCopyLink = $"{Name}__copy-link";

        public readonly QrCode qrCode;
        public readonly Link copyLink;
        public readonly VisualElement logoContainer;
        public readonly VisualElement walletIconContainer;
        public readonly Image walletIcon;

        private RemoteSprite<Image> _walletIconRemoteSprite;

        public new class UxmlFactory : UxmlFactory<QrCodeView>
        {
        }

        public QrCodeView() : this(null)
        {
        }

        public QrCodeView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/QrCodeView/QrCodeView");
            asset.CloneTree(this);

            name = Name;

            qrCode = this.Q<QrCode>();
            copyLink = this.Q<Link>(NameCopyLink);
            logoContainer = this.Q<VisualElement>(NameLogoContainer);
            walletIconContainer = this.Q<VisualElement>(NameWalletIconContainer);
            walletIcon = this.Q<Image>(NameWalletIcon);

            DisableWalletIcon();
        }

        public void EnableWalletIcon(RemoteSprite<Image> iconRemoteSprite)
        {
            logoContainer.style.display = DisplayStyle.None;
            walletIconContainer.style.display = DisplayStyle.Flex;
            _walletIconRemoteSprite = iconRemoteSprite;
            _walletIconRemoteSprite.SubscribeImage(walletIcon);
        }

        public void DisableWalletIcon()
        {
            logoContainer.style.display = DisplayStyle.Flex;
            walletIconContainer.style.display = DisplayStyle.None;
            if (_walletIconRemoteSprite != null)
            {
                _walletIconRemoteSprite.UnsubscribeImage(walletIcon);
                _walletIconRemoteSprite = null;
            }
        }
    }
}