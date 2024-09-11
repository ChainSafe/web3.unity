using System;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3.Environment;
using Scripts.EVM.Marketplace;
using Scripts.EVM.Remote;
using UnityEngine;

public class MarketplaceBrowser : MonoBehaviour
{
    [SerializeField] private string projectId;
    [SerializeField] private string marketplaceId;
    [SerializeField] private string marketplaceContractAddress;
    [SerializeField] private MarketplaceListing marketplaceListing;
    [SerializeField] private Transform parent;

    private MarketplaceContract _marketplaceContract;
    private readonly Dictionary<BigInteger, MarketplaceListing> _marketplaceListings = new ();
    private readonly Dictionary<string, MarketplaceModel.MarketplaceItemMetaData> _cachedMetaData = new ();


    private async void Awake()
    {
        _marketplaceContract =
            await Web3Accessor.Web3.ContractBuilder.Build<MarketplaceContract>(marketplaceContractAddress);
        _marketplaceContract.OnItemSold += ItemSold;

        ProcessingMenu.ToggleMenu();

        var marketplaceItems = await Marketplace.GetMarketplaceItems(marketplaceId, projectId);
        foreach (var item in marketplaceItems.items)
        {
            if (item.status == "sold")
                continue;
            var marketplaceItem = Instantiate(marketplaceListing, parent);

            if (!_cachedMetaData.TryGetValue(item.token.uri, out var marketplaceItemMetaData))
            {
                _cachedMetaData[item.token.uri] = marketplaceItemMetaData =
                    await CSServer.GetDataFromUrl<MarketplaceModel.MarketplaceItemMetaData>(item.token.uri);
            }

            await marketplaceItem.Init(marketplaceContractAddress, item, marketplaceItemMetaData);
            _marketplaceListings.Add(BigInteger.Parse(item.id), marketplaceItem);
        }

        ProcessingMenu.ToggleMenu();
    }
    
    private void ItemSold(MarketplaceContract.ItemSoldEventDTO obj)
    {
        if (!_marketplaceListings.TryGetValue(obj.ItemId, out var listing)) return;
        
        IMainThreadRunner mainThreadRunner = (UnityDispatcherAdapter)Web3Accessor.Web3.ServiceProvider.GetService(typeof(IMainThreadRunner));
        mainThreadRunner.Enqueue(() =>
        {
            Destroy(listing.gameObject);
            _marketplaceListings.Remove(obj.ItemId);
        });
    }

}
