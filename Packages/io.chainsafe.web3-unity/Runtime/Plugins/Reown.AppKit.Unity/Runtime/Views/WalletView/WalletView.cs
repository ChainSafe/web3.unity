using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class WalletView : VisualElement
    {
        public const string Name = "wallet-view";
        public static readonly string NameGetWalletContainer = $"{Name}__get-wallet-container";
        public static readonly string NameGetWalletLabel = $"{Name}__get-wallet-label";
        public static readonly string NameGetWalletButton = $"{Name}__get-wallet-button";
        public static readonly string NameLandscapeContinueInLabel = $"{Name}__landscape-continue-in-label";

        public event Action GetWalletClicked
        {
            add => GetWalletButton.Clicked += value;
            remove => GetWalletButton.Clicked -= value;
        }

        public Tabbed Tabbed { get; }
        public VisualElement GetWalletContainer { get; }
        public Label GetWalletLabel { get; }
        public Button GetWalletButton { get; }
        public Label LandscapeContinueInLabel { get; }

        public new class UxmlFactory : UxmlFactory<WalletView>
        {
        }
        
        public WalletView() : this(null)
        {
        }

        public WalletView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/WalletView/WalletView");
            asset.CloneTree(this);

            AddToClassList(Name);

            Tabbed = this.Q<Tabbed>();
            GetWalletContainer = this.Q<VisualElement>(NameGetWalletContainer);
            GetWalletLabel = this.Q<Label>(NameGetWalletLabel);
            GetWalletButton = this.Q<Button>(NameGetWalletButton);
            LandscapeContinueInLabel = this.Q<Label>(NameLandscapeContinueInLabel);
        }
    }
}