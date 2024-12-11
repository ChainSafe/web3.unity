using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class NetworkLoadingView : VisualElement
    {
        public const string Name = "network-loading-view";
        public static readonly string NameIconContainer = $"{Name}__icon-container";
        public static readonly string NameIcon = $"{Name}__icon";
        public static readonly string NameApproveLabel = $"{Name}__approve-label";
        public static readonly string NameMessageLabel = $"{Name}__message-label";

        public readonly Image icon;

        private RemoteSprite<Image> _networkIconRemoteSprite;

        public new class UxmlFactory : UxmlFactory<NetworkLoadingView>
        {
        }

        public NetworkLoadingView() : this(null)
        {
        }

        public NetworkLoadingView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/NetworkLoadingView/NetworkLoadingView");
            asset.CloneTree(this);

            name = Name;

            icon = this.Q<Image>(NameIcon);

            RegisterCallback<DetachFromPanelEvent, Image>((_, i) => _networkIconRemoteSprite?.UnsubscribeImage(i), icon);
        }

        public void SetNetworkIcon(RemoteSprite<Image> iconSprite)
        {
            _networkIconRemoteSprite?.UnsubscribeImage(icon);

            _networkIconRemoteSprite = iconSprite;
            _networkIconRemoteSprite.SubscribeImage(icon);
        }
    }
}