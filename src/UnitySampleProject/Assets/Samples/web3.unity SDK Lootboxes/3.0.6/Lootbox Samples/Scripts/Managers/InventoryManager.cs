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
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages lootbox inventory items & object spawning.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject nftModal;
    [SerializeField] private LootboxItem lootboxItemPrefab;
    [SerializeField] private Transform inventoryContainer;
    private LootboxServiceConfig lootboxServiceConfig;
    private ILootboxService lootboxService;

    #endregion

    #region Methods

    /// <summary>
    /// Subscribes to events when enabled & populates inventory.
    /// </summary>
    private void OnEnable()
    {
        EventManager.ToggleInventoryItems += SpawnObjects;
        EventManager.ToggleNftModal += ToggleNftModal;
        FetchAndProcessInventory();
    }

    /// <summary>
    /// Unsubscribes from events when disabled to save on memory.
    /// </summary>
    private void OnDisable()
    {
        EventManager.ToggleInventoryItems -= SpawnObjects;
        EventManager.ToggleNftModal -= ToggleNftModal;
    }

    /// <summary>
    /// Initialize services for later use.
    /// </summary>
    private void Awake()
    {
        lootboxServiceConfig = Web3Unity.Web3.ServiceProvider.GetService<LootboxServiceConfig>();
        lootboxService = Web3Unity.Web3.Chainlink().Lootboxes();
    }

    /// <summary>
    /// Fetches & populates the inventory.
    /// </summary>
    private async void FetchAndProcessInventory()
    {
        try
        {
            var inventoryResponseJson = await lootboxService.GetInventory();
            var jsonDeserialized =
                JsonConvert.DeserializeObject<LootboxItemList>(JsonConvert.SerializeObject(inventoryResponseJson));

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

    /// <summary>
    /// Sorts the data by reward type.
    /// </summary>
    /// <param name="rewardType">Lootbox type/rarity/loot per box.</param>
    /// <param name="item">Data item.</param>
    /// <param name="innerItem">Inner data item.</param>
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

    /// <summary>
    /// Sorts through the data in the extraReward object member.
    /// </summary>
    /// <param name="innerItem">Inner data item.</param>
    /// <param name="tokenIds">Token id list to check.</param>
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

    /// <summary>
    /// Initializes the contracts used in the lootbox so data can be called.
    /// </summary>
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

    /// <summary>
    /// Generic method to build contracts.
    /// </summary>
    /// <param name="addresses">The list of contract addresses to build.</param>
    /// <typeparam name="T">The type of contract to be built.</typeparam>
    /// <returns>List of built contracts.</returns>
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

    /// <summary>
    /// Generic method to check balances for erc20/721/1155 contracts with token IDs if specified.
    /// </summary>
    /// <param name="contracts">Contracts to balance check.</param>
    /// <param name="tokenIds">TokenIds to balance check (1155).</param>
    /// <param name="items">List of item data.</param>
    /// <typeparam name="T">The type of contract to check.</typeparam>
    private async Task CheckBalances<T>(
        List<T> contracts,
        List<BigInteger> tokenIds,
        List<ItemData> items) where T : ICustomContract
    {
        foreach (var contract in contracts)
        {
            switch (contract)
            {
                case Erc20Contract erc20Contract:
                {
                    var balance = await erc20Contract.BalanceOf(Web3Unity.Web3.Signer.PublicAddress);
                    if (balance > BigInteger.Zero)
                    {
                        var ethBalance = UnitConversion.Convert.FromWei(balance);
                        var itemName = await Web3Unity.Web3.Erc20.GetName(contract.ContractAddress);
                        items.Add(new ItemData
                            { itemType = "ERC20", itemName = itemName, itemAmount = ethBalance.ToString() });
                    }

                    break;
                }
                case Erc721Contract erc721Contract:
                {
                    foreach (var tokenId in tokenIds)
                    {
                        var owner = await erc721Contract.OwnerOf(tokenId);
                        if (owner == Web3Unity.Web3.Signer.PublicAddress)
                        {
                            var uri = await erc721Contract.TokenURI(tokenId.ToString());
                            var data = await FetchDataWithRetry(uri);
                            var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
                            items.Add(new ItemData
                            {
                                itemType = "ERC721", itemId = tokenId.ToString(), itemName = jsonResponse.name,
                                itemAmount = "1", itemImage = jsonResponse.image
                            });
                        }
                    }

                    break;
                }
                case Erc1155Contract erc1155Contract:
                {
                    foreach (var tokenId in tokenIds)
                    {
                        var balance = await erc1155Contract.BalanceOf(Web3Unity.Web3.Signer.PublicAddress, tokenId);
                        if (balance > BigInteger.Zero)
                        {
                            var uri = await erc1155Contract.Uri(tokenId.ToString());
                            var data = await FetchDataWithRetry(uri);
                            var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
                            items.Add(new ItemData
                            {
                                itemType = "ERC1155", itemId = tokenId.ToString(), itemName = jsonResponse.name,
                                itemAmount = balance.ToString(), itemImage = jsonResponse.image
                            });
                        }
                    }

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Fetches metadata with retries to circumvent SSL issues.
    /// </summary>
    /// <param name="uri">The URI to call./</param>
    /// <param name="maxRetries">The maximum amount of retries.</param>
    /// <param name="delayBetweenRetries">The delay between retries in seconds.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Spawns & populates inventory objects.
    /// </summary>
    /// <param name="itemDataArray">The item data array to populate objects from.</param>
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

    /// <summary>
    /// Toggles the NFT model to display data for a single item on a larger canvas.
    /// </summary>
    private void ToggleNftModal(ItemData itemData)
    {
        nftModal.SetActive(!nftModal.activeSelf);
        EventManager.OnToggleNftData(itemData);
    }

    #endregion
}