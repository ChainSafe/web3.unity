using System;
using UnityEngine;
using UnityEngine.UIElements;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.Components
{
    public class DeepLinkView : VisualElement
    {
        public const string Name = "deep-link-view";
        public static readonly string NameWalletIcon = $"{Name}__wallet-icon";
        public static readonly string NameContinueIn = $"{Name}__continue-in-label";
        public static readonly string NameCopyLink = $"{Name}__copy-link";
        public static readonly string NameTryAgainLink = $"{Name}__try-again-link";

        public Image WalletIcon { get; }
        public Label ContinueIn { get; }
        public Link CopyLink { get; }
        public Link TryAgainLink { get; }

        private RemoteSprite<Image> _walletIconRemoteSprite;

        public string ContinueInText
        {
            get => ContinueIn.text;
            set => ContinueIn.text = value;
        }

        public event Action CopyLinkClicked
        {
            add => CopyLink.Clicked += value;
            remove => CopyLink.Clicked -= value;
        }

        public event Action TryAgainLinkClicked
        {
            add => TryAgainLink.Clicked += value;
            remove => TryAgainLink.Clicked -= value;
        }

        public new class UxmlFactory : UxmlFactory<DeepLinkView>
        {
        }

        public DeepLinkView() : this(null)
        {
        }

        public DeepLinkView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/DeepLinkView/DeepLinkView");
            asset.CloneTree(this);

            name = Name;

            WalletIcon = this.Q<Image>(NameWalletIcon);
            ContinueIn = this.Q<Label>(NameContinueIn);
            CopyLink = this.Q<Link>(NameCopyLink);
            TryAgainLink = this.Q<Link>(NameTryAgainLink);

            RegisterCallback<DetachFromPanelEvent>(
                _ => _walletIconRemoteSprite?.UnsubscribeImage(WalletIcon)
            );
        }

        public void SetWalletInfo(RemoteSprite<Image> iconSprite, string continueInText)
        {
            ContinueInText = continueInText.FontWeight500();
            SetWalletIcon(iconSprite);
        }

        public void SetWalletIcon(RemoteSprite<Image> iconSprite)
        {
            _walletIconRemoteSprite?.UnsubscribeImage(WalletIcon);

            _walletIconRemoteSprite = iconSprite;
            _walletIconRemoteSprite.SubscribeImage(WalletIcon);
        }
    }
}