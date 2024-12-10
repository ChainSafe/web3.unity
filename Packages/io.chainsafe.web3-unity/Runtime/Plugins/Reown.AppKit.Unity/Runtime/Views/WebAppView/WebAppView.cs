using System;
using UnityEngine;
using UnityEngine.UIElements;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.Components
{
    public class WebAppView : VisualElement
    {
        public const string Name = "web-app-view";
        public static readonly string NameWalletIcon = $"{Name}__wallet-icon";
        public static readonly string NameContinueIn = $"{Name}__continue-in-label";
        public static readonly string NameOpenButton = $"{Name}__open-button";
        public static readonly string NameCopyLink = $"{Name}__copy-link";

        public readonly Image walletIcon;
        public readonly Label continueIn;
        public readonly Button openButton;
        public readonly Link copyLink;

        private RemoteSprite<Image> _walletIconRemoteSprite;

        public string ContinueInText
        {
            get => continueIn.text;
            set => continueIn.text = value;
        }

        public event Action CopyLinkClicked
        {
            add => copyLink.Clicked += value;
            remove => copyLink.Clicked -= value;
        }

        public event Action OpenLinkClicked
        {
            add => openButton.Clicked += value;
            remove => openButton.Clicked -= value;
        }

        public new class UxmlFactory : UxmlFactory<WebAppView>
        {
        }

        public WebAppView() : this(null)
        {
        }

        public WebAppView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/WebAppView/WebAppView");
            asset.CloneTree(this);

            name = Name;

            walletIcon = this.Q<Image>(NameWalletIcon);
            continueIn = this.Q<Label>(NameContinueIn);
            copyLink = this.Q<Link>(NameCopyLink);
            openButton = this.Q<Button>(NameOpenButton);

            RegisterCallback<DetachFromPanelEvent, Image>(
                (_, i) => _walletIconRemoteSprite?.UnsubscribeImage(i), walletIcon
            );
        }

        public void SetWalletInfo(RemoteSprite<Image> iconSprite, string continueInText)
        {
            ContinueInText = continueInText.FontWeight500();
            SetWalletIcon(iconSprite);
        }

        public void SetWalletIcon(RemoteSprite<Image> iconSprite)
        {
            _walletIconRemoteSprite?.UnsubscribeImage(walletIcon);

            _walletIconRemoteSprite = iconSprite;
            _walletIconRemoteSprite.SubscribeImage(walletIcon);
        }
    }
}