using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Marketplace.Extensions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Marketplace.Samples
{
    public class MarketplaceSample : MonoBehaviour
    {
        [SerializeField] private Transform parentForItems;
        [SerializeField] private UI_MarketplaceItem marketplaceItem;
        [SerializeField] private Button nextPageButton;

        private MarketplacePage _currentPage;
        private async void Start()
        {
            await Web3Unity.Instance.Initialize(false);
            try
            {
                LoadingOverlay.ShowLoadingOverlay();
                _currentPage =
                    await Web3Unity.Web3.Marketplace().LoadPage();
                nextPageButton.interactable = !string.IsNullOrEmpty(_currentPage.Cursor);
                await DisplayItems();
            }
            catch (Exception e)
            {
                Debug.LogError("Caught an exception whilst loading the marketplace page " + e.Message);
            }
            finally
            {
                LoadingOverlay.HideLoadingOverlay();
            }
        }

        public async void LoadNextPage()
        {
            try
            {
                LoadingOverlay.ShowLoadingOverlay();
                Debug.Log(_currentPage.Cursor);
                _currentPage = await Web3Unity.Web3.Marketplace().LoadPage(_currentPage);
                await DisplayItems();
            }
            catch (Exception e)
            {
                Debug.LogError("Caught an exception whilst loading the marketplace page " + e.Message);
            }
            finally
            {
                LoadingOverlay.HideLoadingOverlay();
            }
        }

        private async Task DisplayItems()
        {
            for (int i = parentForItems.childCount - 1; i >= 0; i--)
            {
                Destroy(parentForItems.GetChild(i).gameObject);
            }

            foreach (var pageItem in _currentPage.Items)
            {
                var item = Instantiate(marketplaceItem, parentForItems);
                await item.Initialize(pageItem);
            }
        }
    }
}