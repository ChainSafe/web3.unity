using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Util;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject nftModal;
    [SerializeField] private LootboxItem lootboxItemPrefab;
    [SerializeField] private Transform inventoryContainer;

    private LootboxServiceConfig lootboxServiceConfig;
    private ILootboxService lootboxService;

    private void OnEnable()
    {
        EventManager.ToggleInventoryItems += SpawnObjects;
        FetchAndProcessInventory();
    }

    private void OnDisable() => EventManager.ToggleInventoryItems -= SpawnObjects;

    private void Awake()
    {
        lootboxServiceConfig = Web3Unity.Web3.ServiceProvider.GetService<LootboxServiceConfig>();
        lootboxService = Web3Unity.Web3.Chainlink().Lootboxes();
    }

    private async void FetchAndProcessInventory()
    {
        try
        {
            var inventoryResponseJson = await lootboxService.GetInventory();
            var jsonDeserialized = JsonConvert.DeserializeObject<LootboxItemList>(JsonConvert.SerializeObject(inventoryResponseJson));

            foreach (var outerItem in jsonDeserialized)
            {
                foreach (var innerItem in outerItem)
                {
                    foreach (var item in innerItem)
                    {
                        if (item.Parameter.Name == "rewardToken")
                        {
                            Debug.Log($"{item.Parameter.Name}: {item.Result}");
                            var rewardTypeItem = innerItem.Find(x => x.Parameter.Name == "rewardType");
                            if (rewardTypeItem != null)
                            {
                                HandleRewardType(int.Parse(rewardTypeItem.Result.ToString()), item, innerItem);
                                break;
                            }
                        }
                    }
                }
            }
            await InitializeContracts();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching inventory: {e.Message}");
        }
    }

    private void HandleRewardType(int rewardType, Item item, List<Item> innerItem)
    {
        switch (rewardType)
        {
            case 1: // ERC20
                if (!lootboxServiceConfig.Erc20Contracts.Contains(item.Result.ToString()))
                    lootboxServiceConfig.Erc20Contracts.Add(item.Result.ToString());
                break;

            case 2: // ERC721
                if (!lootboxServiceConfig.Erc721Contracts.Contains(item.Result.ToString()))
                    lootboxServiceConfig.Erc721Contracts.Add(item.Result.ToString());
                ParseExtraRewards(innerItem, lootboxServiceConfig.Erc721TokenIds);
                break;

            case 3: // ERC1155
                if (!lootboxServiceConfig.Erc1155Contracts.Contains(item.Result.ToString()))
                    lootboxServiceConfig.Erc1155Contracts.Add(item.Result.ToString());
                ParseExtraRewards(innerItem, lootboxServiceConfig.Erc1155TokenIds);
                break;

            default:
                Debug.Log("Reward Type: UNDEFINED");
                break;
        }
    }

    private void ParseExtraRewards(List<Item> innerItem, List<BigInteger> tokenIds)
    {
        var extraItem = innerItem.Find(x => x.Parameter.Name == "extra");
        if (extraItem != null)
        {
            try
            {
                var extraList = JsonConvert.DeserializeObject<List<List<ExtraRewardInfo>>>(extraItem.Result.ToString());
                foreach (var extra in extraList)
                {
                    var tokenId = BigInteger.Parse(extra[0].Id.ToString());
                    if (!tokenIds.Contains(tokenId))
                        tokenIds.Add(tokenId);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing extra field: {e.Message}");
            }
        }
    }

    private async Task InitializeContracts()
    {
        Debug.Log("Initializing Contracts");
        var erc20Contracts = await BuildContracts<Erc20Contract>(lootboxServiceConfig.Erc20Contracts);
        var erc721Contracts = await BuildContracts<Erc721Contract>(lootboxServiceConfig.Erc721Contracts);
        var erc1155Contracts = await BuildContracts<Erc1155Contract>(lootboxServiceConfig.Erc1155Contracts);

        var items = new List<ItemData>();
        await CheckBalances(erc20Contracts, lootboxServiceConfig.Erc721TokenIds, items);
        await CheckBalances(erc721Contracts, lootboxServiceConfig.Erc721TokenIds, items);
        await CheckBalances(erc1155Contracts, lootboxServiceConfig.Erc1155TokenIds, items);

        if (items.Count > 0)
        {
            EventManager.OnToggleInventoryItems(items.ToArray());
        }
        else
        {
            Debug.Log("No tokens found for the user.");
        }
    }

    private async Task<List<T>> BuildContracts<T>(List<string> addresses) where T : ICustomContract, new()
    {
        var contracts = new List<T>();
        if (addresses == null) return contracts;

        foreach (var address in addresses)
        {
            var contract = await Web3Unity.Web3.ContractBuilder.Build<T>(address);
            contracts.Add(contract);
        }

        return contracts;
    }

    private async Task CheckBalances<T>(
        List<T> contracts,
        List<BigInteger> tokenIds,
        List<ItemData> items) where T : ICustomContract
    {
        foreach (var contract in contracts)
        {
            if (contract is Erc20Contract erc20Contract)
            {
                var balance = await erc20Contract.BalanceOf(Web3Unity.Web3.Signer.PublicAddress);
                if (balance > BigInteger.Zero)
                {
                    var ethBalance = UnitConversion.Convert.FromWei(balance);
                    items.Add(new ItemData { itemType = "ERC20", itemName = "ERC20 Token", itemAmount = ethBalance.ToString() });
                }
            }
            else if (contract is Erc721Contract erc721Contract)
            {
                foreach (var tokenId in tokenIds)
                {
                    var owner = await erc721Contract.OwnerOf(tokenId);
                    if (owner == Web3Unity.Web3.Signer.PublicAddress)
                    {
                        var uri = await erc721Contract.TokenURI(tokenId.ToString());
                        var data = await FetchDataWithRetry(uri);
                        var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
                        items.Add(new ItemData { itemType = "ERC721", itemId = tokenId.ToString(), itemName = jsonResponse.name, itemAmount = "1", itemImage = jsonResponse.image});
                    }
                }
            }
            else if (contract is Erc1155Contract erc1155Contract)
            {
                foreach (var tokenId in tokenIds)
                {
                    var balance = await erc1155Contract.BalanceOf(Web3Unity.Web3.Signer.PublicAddress, tokenId);
                    if (balance > BigInteger.Zero)
                    {
                        var uri = await erc1155Contract.Uri(tokenId.ToString());
                        var data = await FetchDataWithRetry(uri);
                        var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
                        items.Add(new ItemData { itemType = "ERC1155", itemId = tokenId.ToString(), itemName = jsonResponse.name, itemAmount = balance.ToString(), itemImage = jsonResponse.image });
                    }
                }
            }
        }
    }

    private async Task<string> FetchDataWithRetry(string uri, int maxRetries = 10, float delayBetweenRetries = 5.0f)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            attempt++;
            Debug.Log($"Attempt {attempt}: Fetching data from {uri}");
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    return webRequest.downloadHandler.text;
                }
                await Task.Delay((int)(delayBetweenRetries * 1000));
            }
        }
        return string.Empty;
    }

    private async void SpawnObjects(ItemData[] itemDataArray)
    {
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in itemDataArray)
        {
            var newItem = Instantiate(lootboxItemPrefab, inventoryContainer);
            await newItem.Initialize(item);
        }
    }

    // TODO make open on event
    private void OpenNftModal()
    {
        nftModal.SetActive(true);
    }
}