using System.Collections.Generic;
using System.Threading.Tasks;
using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Model;
using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public class WalletSearchPresenter : Presenter<WalletSearchView>
    {
        public override string Title
        {
            get => "All wallets";
        }

        private bool _isPageLoading;

        private int _maxWalletsCount = -1;
        private int _nextPageToLoad = 1;
        private bool _reachedMaxWalletsCount = false;
        private int _countPerPageRealtime = 0;
        private int _walletsShown = 0;
        private string _searchQuery = null;

        private Task _loadNextPageTask;

        private const int WalletsPerPage = 32;

        private readonly List<VisualElement> _items = new();

        public WalletSearchPresenter(RouterController router, VisualElement parent) : base(router, parent)
        {
            View.ScrollValueChanged += OnScrollValueChanged;
            View.SearchInputValueChanged += OnSearchInputValueChanged;
            View.QrCodeLinkClicked += () => Router.OpenView(ViewType.QrCode);
        }

        private void OnSearchInputValueChanged(string value)
        {
            var searchQuery = value.Trim();
            _searchQuery = string.IsNullOrWhiteSpace(searchQuery) ? null : value.Trim();
            _nextPageToLoad = 1;
            _walletsShown = 0;
            _maxWalletsCount = -1;
            _reachedMaxWalletsCount = false;
            _countPerPageRealtime = WalletsPerPage;
            _isPageLoading = false;

            foreach (var item in _items)
                item.RemoveFromHierarchy();

            _items.Clear();

            LoadNextPage();
        }

        private void ConfigureItemPaddings(IList<VisualElement> items = null)
        {
            var scrollViewWidth = View.scrollView.resolvedStyle.width;
            const float itemWidth = 79;
            var itemsCanFit = Mathf.FloorToInt(scrollViewWidth / itemWidth);

            var padding = (scrollViewWidth - itemsCanFit * itemWidth) / itemsCanFit / 2;
            items ??= _items;

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.style.paddingLeft = padding;
                item.style.paddingRight = padding;
            }
        }

        private void OnScrollValueChanged(float value)
        {
            if (_isPageLoading || _reachedMaxWalletsCount)
                return;

            if (value >= (View.scroller.highValue - View.scroller.lowValue) * 0.7f)
                LoadNextPage();
        }

        private VisualElement MakeItem(Wallet wallet)
        {
            var isWalletInstalled = WalletUtils.IsWalletInstalled(wallet);
            var walletStatusIcon = isWalletInstalled ? StatusIconType.Success : StatusIconType.None;
            var item = new CardSelect(walletStatusIcon)
            {
                LabelText = wallet.Name
            };

            item.Clicked += () =>
            {
                WalletUtils.SetLastViewedWallet(wallet);
                Router.OpenView(ViewType.Wallet);
            };

            item.Icon = wallet.Image;

            _items.Add(item);

            return item;
        }

        private async void LoadNextPage()
        {
            if (_isPageLoading || _reachedMaxWalletsCount)
                return;

            if (_loadNextPageTask is { IsCompleted: false })
                return;

            _loadNextPageTask = LoadNextPageCoroutine();
        }

        private async Task LoadNextPageCoroutine()
        {
            _isPageLoading = true;

            if (_maxWalletsCount != -1 && _nextPageToLoad * _countPerPageRealtime > _maxWalletsCount)
            {
                _countPerPageRealtime = _maxWalletsCount - _walletsShown;
                _reachedMaxWalletsCount = true;
            }

            if (_countPerPageRealtime == 0)
            {
                _isPageLoading = false;
                return;
            }

            var getWalletsResponse = await AppKit.ApiController.GetWallets(_nextPageToLoad, _countPerPageRealtime, _searchQuery);

            // _noWalletFound.SetActive(response.Count == 0);

            if (_maxWalletsCount == -1)
            {
                _maxWalletsCount = getWalletsResponse.Count;

                if (_nextPageToLoad * _countPerPageRealtime > _maxWalletsCount)
                {
                    _countPerPageRealtime = _maxWalletsCount - _walletsShown;
                    _reachedMaxWalletsCount = true;
                }
            }

            var walletsCount = getWalletsResponse.Data.Length;

            var items = new List<VisualElement>(walletsCount);
            for (var i = 0; i < walletsCount; i++)
            {
                var wallet = getWalletsResponse.Data[i];
                var item = MakeItem(wallet);
                items.Add(item);
                View.scrollView.Add(item);
            }

            _walletsShown += walletsCount;
            _nextPageToLoad++;

            ConfigureItemPaddings(items);

            View.scrollView.ForceUpdate();

            _isPageLoading = false;
        }


        protected override void OnVisibleCore()
        {
            base.OnVisibleCore();

            _countPerPageRealtime = WalletsPerPage;
            LoadNextPage();
        }
    }
}