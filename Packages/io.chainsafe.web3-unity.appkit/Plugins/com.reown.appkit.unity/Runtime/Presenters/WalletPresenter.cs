using System.Collections.Generic;
using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Model;
using Reown.AppKit.Unity.Utils;
using Reown.Sign.Unity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public class WalletPresenter : Presenter<WalletView>
    {
        private Wallet _wallet;

        private readonly Tabbed _tabbed;
        private readonly VisualElement _tabsContainer;

        private readonly QrCodePresenter _qrCodePresenter;
        private readonly DeepLinkPresenter _deepLinkPresenter;
        private readonly WebAppPresenter _webAppPresenter;

        private readonly Label _qrCodeTab;
        private readonly Label _deepLinkTab;
        private readonly Label _browserTab;
        private readonly VisualElement _qrCodeContent;
        private readonly VisualElement _deepLinkContent;
        private readonly VisualElement _webAppContent;

        private readonly Dictionary<VisualElement, PresenterBase> _tabContentToViewController = new();

        public WalletPresenter(RouterController router, VisualElement parent) : base(router, parent)
        {
            // --- Tabbed View References
            _tabbed = View.Q<Tabbed>();
            _tabsContainer = View.Q<VisualElement>(className: Tabbed.ClassNameTabsContainer);

            // --- Tabs References
            _qrCodeTab = View.Q<Label>("QrCodeTab");
            _deepLinkTab = View.Q<Label>("DeepLinkTab");
            _browserTab = View.Q<Label>("WebAppTab");

            // --- Tabs Content References and Controllers           
            _qrCodeContent = View.Q<VisualElement>("QrCodeContent");
            _deepLinkContent = View.Q<VisualElement>("DeepLinkContent");
            _webAppContent = View.Q<VisualElement>("WebAppContent");

            _qrCodePresenter = new QrCodePresenter(router, _qrCodeContent, false);
            _deepLinkPresenter = new DeepLinkPresenter(router, _deepLinkContent, false);
            _webAppPresenter = new WebAppPresenter(router, _webAppContent, false);

            _tabContentToViewController.Add(_qrCodeContent, _qrCodePresenter);
            _tabContentToViewController.Add(_deepLinkContent, _deepLinkPresenter);
            _tabContentToViewController.Add(_webAppContent, _webAppPresenter);

            // --- Events
            _tabbed.ContentShown += element => _tabContentToViewController[element].OnVisible();
            _tabbed.ContentHidden += element => _tabContentToViewController[element].OnDisable();
            View.GetWalletClicked += OnGetWalletClicked;

            // --- Additional Setup
            HideAllTabs();
#if UNITY_ANDROID || UNITY_IOS
            _deepLinkTab.text = "Mobile";
#endif
        }

        private void HideAllTabs()
        {
            HideTab(_qrCodeTab);
            HideTab(_deepLinkTab);
            HideTab(_browserTab);
        }

        protected override void OnVisibleCore()
        {
            base.OnVisibleCore();

            if (!WalletUtils.TryGetLastViewedWallet(out var wallet))
                return;

            _wallet = wallet;
            Title = wallet.Name;

            ConfigureTabs(wallet);
            ConfigureGetWalletContainer(wallet);

            SendAnalyticsEvent(wallet);
        }

        private static void SendAnalyticsEvent(Wallet wallet)
        {
            var eventProperties = new Dictionary<string, object>
            {
                { "name", wallet.Name },
                { "explorer_id", wallet.Id }
            };

            var walletPlatform = EventUtils.GetWalletPlatform(wallet);
            if (walletPlatform != null)
                eventProperties.Add("platform", walletPlatform);

            AppKit.EventsController.SendEvent(new Event
            {
                name = "SELECT_WALLET",
                properties = eventProperties
            });
        }

        private void ConfigureGetWalletContainer(Wallet wallet)
        {
            var visible = !WalletUtils.IsWalletInstalled(wallet);
            View.GetWalletContainer.style.display = visible
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            if (!visible)
                return;

            View.GetWalletLabel.text = $"Don't have {wallet.Name}?".FontWeight500();
            View.LandscapeContinueInLabel.text = $"Continue in {wallet.Name}".FontWeight500();
        }

        protected void OnGetWalletClicked()
        {
            AppKit.EventsController.SendEvent(new Event
            {
                name = "CLICK_GET_WALLET"
            });

#if UNITY_IOS
            Application.OpenURL(_wallet.AppStore);
#elif UNITY_ANDROID
            Application.OpenURL(_wallet.PlayStore);
#else
            // TODO: on desktop and webgl show the list of all available options
            Application.OpenURL(_wallet.Homepage);
#endif
        }

        protected override void OnHideCore()
        {
            base.OnHideCore();
            WalletUtils.RemoveLastViewedWallet();
        }

        protected override void OnDisableCore()
        {
            base.OnDisableCore();

            WalletUtils.RemoveLastViewedWallet();
            HideAllTabs();
        }

        private void ConfigureTabs(Wallet wallet)
        {
            var tabsCount = 0;
#if UNITY_IOS || UNITY_ANDROID
            // Mobile: Deep Link
            if (wallet is { MobileLink: not null })
            {
                ShowTab(_deepLinkTab);
                tabsCount++;
            }
#else
            // Desktop: QR Code
            ShowTab(_qrCodeTab);
            tabsCount++;

            // Desktop: Deep Link
            if (wallet.DesktopLink != null)
            {
                ShowTab(_deepLinkTab);
                tabsCount++;
            }
#endif
            // All: Browser
            if (wallet is { WebappLink: not null })
            {
                ShowTab(_browserTab);
                tabsCount++;
            }

            if (tabsCount == 1)
                HideTabsContainer();
            else
                ShowTabsContainer();

            UnityEventsDispatcher.InvokeNextFrame(_tabbed.ActivateFirstVisibleTab);
        }

        private void HideTabsContainer()
        {
            _tabsContainer.AddToClassList(Tabbed.ClassNameTabsContainerHidden);
        }

        private void ShowTabsContainer()
        {
            if (_tabsContainer.ClassListContains(Tabbed.ClassNameTabsContainerHidden))
                _tabsContainer.RemoveFromClassList(Tabbed.ClassNameTabsContainerHidden);
        }

        private static void HideTab(VisualElement tab)
        {
            tab.AddToClassList(Tabbed.ClassNameTabHidden);
        }

        private static void ShowTab(VisualElement tab)
        {
            if (tab.ClassListContains(Tabbed.ClassNameTabHidden))
                tab.RemoveFromClassList(Tabbed.ClassNameTabHidden);
        }
    }
}