using System;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Reown.AppKit.Unity.Components.Button;

namespace Reown.AppKit.Unity.Views.SiweView
{
    public class SiweView : VisualElement
    {
        public const string Name = "siwe-view";
        public static readonly string NameLogoAppImage = $"{Name}__logo-app-image";
        public static readonly string NameLogoAppContainer = $"{Name}__logo-app-container";
        public static readonly string NameLogoWalletImage = $"{Name}__logo-wallet-image";
        public static readonly string NameLogoWalletContainer = $"{Name}__logo-wallet-container";
        public static readonly string NameTitle = $"{Name}__title";
        public static readonly string NameCancelButton = $"{Name}__cancel-button";
        public static readonly string NameApproveButton = $"{Name}__approve-button";

        public static readonly string ClassNameLogoMoveLeft = $"{Name}__logo-move-left";
        public static readonly string ClassNameLogoMoveRight = $"{Name}__logo-move-right";

        public string Title
        {
            get => _titleLabel.text;
            set => _titleLabel.text = value;
        }

        public bool ButtonsEnabled
        {
            get => _cancelButton.enabledSelf && _approveButton.enabledSelf;
            set
            {
                _cancelButton.SetEnabled(value);
                _approveButton.SetEnabled(value);
            }
        }

        public Image LogoAppImage { get; }
        public Image LogoWalletImage { get; }

        public event Action CancelButtonClicked
        {
            add => _cancelButton.Clicked += value;
            remove => _cancelButton.Clicked -= value;
        }

        public event Action ApproveButtonClicked
        {
            add => _approveButton.Clicked += value;
            remove => _approveButton.Clicked -= value;
        }
        
        private readonly Label _titleLabel;
        private readonly Button _cancelButton;
        private readonly Button _approveButton;

        public new class UxmlFactory : UxmlFactory<SiweView>
        {
        }

        public SiweView() : this(null)
        {
        }

        public SiweView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/SiweView/SiweView");
            asset.CloneTree(this);

            name = Name;

            var appLogoContainer = this.Q<VisualElement>(NameLogoAppContainer);
            var walletLogoContnaier = this.Q<VisualElement>(NameLogoWalletContainer);
            LogoAppImage = this.Q<Image>(NameLogoAppImage);
            LogoWalletImage = this.Q<Image>(NameLogoWalletImage);
            _titleLabel = this.Q<Label>(NameTitle);

            _cancelButton = this.Q<Button>(NameCancelButton);
            _approveButton = this.Q<Button>(NameApproveButton);
            
            // App and wallet logo animation
            appLogoContainer.RegisterCallback<TransitionEndEvent, VisualElement>((_, x) => x.ToggleInClassList(ClassNameLogoMoveLeft), appLogoContainer);
            walletLogoContnaier.RegisterCallback<TransitionEndEvent, VisualElement>((_, x) => x.ToggleInClassList(ClassNameLogoMoveRight), walletLogoContnaier);
            schedule.Execute(() =>
            {
                appLogoContainer.ToggleInClassList(ClassNameLogoMoveLeft);
                walletLogoContnaier.ToggleInClassList(ClassNameLogoMoveRight);
            }).StartingIn(100);
        }
    }
}