using UnityEngine;
using UnityEngine.UIElements;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.Components
{
    public class AccountView : VisualElement
    {
        public const string Name = "account-view";
        public static readonly string NameProfile = $"{Name}__profile";
        public static readonly string NameProfileAddress = $"{Name}__profile-address";
        public static readonly string NameProfileAvatarImage = $"{Name}__profile-avatar-image";
        public static readonly string NameProfileBalanceValue = $"{Name}__profile-balance-value";
        public static readonly string NameProfileBalanceSymbol = $"{Name}__profile-balance-symbol";
        public static readonly string NameExplorerButton = $"{Name}__explorer-button";
        public static readonly string NameCopyLink = $"{Name}__profile-address-copy-link";
        public static readonly string NameButtons = $"{Name}__buttons";

        public VisualElement Profile { get; }
        public Label ProfileAddress { get; }
        public Image ProfileAvatarImage { get; }
        public Label ProfileBalanceValue { get; }
        public Label ProfileBalanceSymbol { get; }
        public Button ExplorerButton { get; }
        public IconLink CopyLink { get; }
        public VisualElement Buttons { get; }


        public new class UxmlFactory : UxmlFactory<AccountView>
        {
        }

        public AccountView() : this(null)
        {
        }

        public AccountView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/AccountView/AccountView");
            asset.CloneTree(this);

            name = Name;

            Profile = this.Q<VisualElement>(NameProfile);
            ProfileAddress = Profile.Q<Label>(NameProfileAddress);
            ProfileAvatarImage = Profile.Q<Image>(NameProfileAvatarImage);
            ProfileBalanceValue = Profile.Q<Label>(NameProfileBalanceValue);
            ProfileBalanceSymbol = Profile.Q<Label>(NameProfileBalanceSymbol);
            ExplorerButton = Profile.Q<Button>(NameExplorerButton);
            CopyLink = Profile.Q<IconLink>(NameCopyLink);
            Buttons = this.Q<VisualElement>(NameButtons);
        }

        public void SetProfileName(string value)
        {
            ProfileAddress.text = value.FontWeight600();
        }

        public void SetBalance(string value)
        {
            ProfileBalanceValue.text = value.FontWeight500();
        }

        public void SetBalanceSymbol(string value)
        {
            ProfileBalanceSymbol.text = value.FontWeight500();
        }
    }
}