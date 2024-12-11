using System;
using System.Linq;
using System.Threading.Tasks;
using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Model;
using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using DeviceType = Reown.AppKit.Unity.Utils.DeviceType;

namespace Reown.AppKit.Unity
{
    public class ConnectPresenter : Presenter<VisualElement>
    {
        private bool _disposed;

        public override string Title
        {
            get => "Connect wallet";
        }

        public ConnectPresenter(RouterController router, VisualElement parent) : base(router, parent)
        {
            Build();

            AppKit.Initialized += Web3ModalInitializedHandler;
        }

        private void Web3ModalInitializedHandler(object sender, EventArgs e)
        {
            AppKit.AccountDisconnected += AccountDisconnectedHandler;
        }

        private async void AccountDisconnectedHandler(object sender, EventArgs e)
        {
            await RebuildAsync();
        }

        private async void Build()
        {
            try
            {
                await BuildAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected virtual async Task RebuildAsync()
        {
            foreach (var visualElement in View.Children().ToArray())
                View.Remove(visualElement);

            await BuildAsync();
        }

        protected virtual async Task BuildAsync()
        {
            CreateWalletConnectButton();

            var recentWalletExists = WalletUtils.TryGetRecentWallet(out var recentWallet);
            if (recentWalletExists)
                CreateRecentWalletButton(recentWallet);

            int count = DeviceUtils.GetDeviceType() is DeviceType.Phone
                ? AppKit.Config.connectViewWalletsCountMobile
                : AppKit.Config.connectViewWalletsCountDesktop;

            if (recentWalletExists)
                count++;

            if (AppKit.Config.customWallets is { Length: > 0 })
            {
                foreach (var customWallet in AppKit.Config.customWallets)
                {
                    if (count-- <= 0)
                        break;

                    var walletListItem = BuildWalletListItem(customWallet);
                    View.Add(walletListItem);
                }
            }

            if (count <= 0)
                return;
            
            var response = await AppKit.ApiController.GetWallets(1, count);

            foreach (var wallet in response.Data)
            {
                // Skip recent wallet to avoid duplicates
                if (recentWalletExists && recentWallet.Id == wallet.Id)
                    continue;

                var walletListItem = BuildWalletListItem(wallet);
                View.Add(walletListItem);
            }

            var responseCount = response.Count;
            CreateAllWalletsListItem(responseCount);
        }

        protected virtual void CreateWalletConnectButton()
        {
            var deviceType = DeviceUtils.GetDeviceType();

            if (deviceType is DeviceType.Phone)
                return;
            var listItem = BuildWalletConnectListItem();
            View.Add(listItem);
        }

        protected virtual void CreateRecentWalletButton(Wallet recentWallet)
        {
            var listItem = new ListItem(recentWallet.Name, recentWallet.Image, () => OnWalletListItemClick(recentWallet));
            listItem.RightSlot.Add(new Tag("RECENT", Tag.TagType.Info));
            View.Add(listItem);
        }

        protected virtual void CreateAllWalletsListItem(int responseCount)
        {
            var allWalletsListItem = BuildAllWalletsListItem(responseCount);
            View.Add(allWalletsListItem);
        }

        protected virtual ListItem BuildWalletListItem(Wallet wallet)
        {
            var walletClosure = wallet;
            var isWalletInstalled = WalletUtils.IsWalletInstalled(wallet);
            var walletStatusIcon = isWalletInstalled ? StatusIconType.Success : StatusIconType.None;
            var walletListItem = new ListItem(wallet.Name, wallet.Image, () => OnWalletListItemClick(walletClosure), statusIconType: walletStatusIcon);
            return walletListItem;
        }

        protected virtual ListItem BuildWalletConnectListItem()
        {
            var wcLogo =
                RemoteSpriteFactory.GetRemoteSprite<Image>(
                    $"https://api.web3modal.com/public/getAssetImage/ef1a1fcf-7fe8-4d69-bd6d-fda1345b4400");
            var listItem = new ListItem("WalletConnect", wcLogo, () =>
            {
                WalletUtils.RemoveLastViewedWallet();
                Router.OpenView(ViewType.QrCode);
            });
            listItem.RightSlot.Add(new Tag("QR CODE", Tag.TagType.Accent));
            return listItem;
        }

        protected virtual ListItem BuildAllWalletsListItem(int responseCount)
        {
            var allWalletsListItem = new ListItem("All wallets", (Sprite)null, () =>
            {
                Router.OpenView(ViewType.WalletSearch);
                AppKit.EventsController.SendEvent(new Event
                {
                    name = "CLICK_ALL_WALLETS"
                });
            });
            var roundedCount = MathF.Round((float)responseCount / 10) * 10;
            allWalletsListItem.RightSlot.Add(new Tag($"{roundedCount}+", Tag.TagType.Info));
            return allWalletsListItem;
        }

        protected virtual void OnWalletListItemClick(Wallet wallet)
        {
            WalletUtils.SetLastViewedWallet(wallet);
            Router.OpenView(ViewType.Wallet);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                AppKit.Initialized -= Web3ModalInitializedHandler;
                AppKit.AccountDisconnected -= AccountDisconnectedHandler;
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}