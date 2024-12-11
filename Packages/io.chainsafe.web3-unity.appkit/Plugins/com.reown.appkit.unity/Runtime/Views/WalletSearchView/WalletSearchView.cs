using System;
using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using DeviceType = Reown.AppKit.Unity.Utils.DeviceType;

namespace Reown.AppKit.Unity.Components
{
    public class WalletSearchView : VisualElement
    {
        public const string Name = "wallet-search-view";
        public static readonly string ClassNameList = $"{Name}__list";
        public static readonly string ClassNameInput = $"{Name}__input";
        public static readonly string ClassNameQrCodeLink = $"{Name}__qr-code-link";

        public event Action<float> ScrollValueChanged
        {
            add => scroller.valueChanged += value;
            remove => scroller.valueChanged -= value;
        }

        public event Action QrCodeLinkClicked
        {
            add => qrCodeLink.Clicked += value;
            remove => qrCodeLink.Clicked -= value;
        }

        public event Action<string> SearchInputValueChanged;

        public readonly ScrollView scrollView;
        public readonly Scroller scroller;
        public readonly TextInput searchInput;
        public readonly IconLink qrCodeLink;
        public readonly VisualElement leftSlot;
        public readonly VisualElement rightSlot;

        public new class UxmlFactory : UxmlFactory<WalletSearchView>
        {
        }

        public WalletSearchView() : this(null)
        {
        }

        public WalletSearchView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/WalletSearchView/WalletSearchView");
            asset.CloneTree(this);

            AddToClassList(Name);

            var deviceType = DeviceUtils.GetDeviceType();

            // --- Search Input
            searchInput = this.Q<TextInput>();
            searchInput.leftSlot.Add(new Image
            {
                vectorImage = Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_medium_magnifier")
            });
            searchInput.RegisterCallback<ChangeEvent<string>>(evt => SearchInputValueChanged?.Invoke(evt.newValue));
            searchInput.SetPlaceholder("Search wallet");

            // --- ScrollView
            scrollView = this.Q<ScrollView>();
            scrollView.mode = ScrollViewMode.Vertical;
            scrollView.mouseWheelScrollSize = 65;
            scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

            scroller = scrollView.Q<Scroller>();

            // --- Slots
            leftSlot = this.Q<VisualElement>("left-slot");
            rightSlot = this.Q<VisualElement>("right-slot");

            // --- QR Code Link
            qrCodeLink = this.Q<IconLink>(ClassNameQrCodeLink);
            if (deviceType is DeviceType.Phone)
                qrCodeLink.image.vectorImage = Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_regular_qrcode");
            else
                qrCodeLink.style.display = DisplayStyle.None;
        }
    }
}