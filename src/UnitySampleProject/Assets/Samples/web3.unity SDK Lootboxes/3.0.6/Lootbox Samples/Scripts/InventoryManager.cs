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
    [SerializeField] private GameObject inventoryObjectPrefab, nftModal;
    [SerializeField] private Transform inventoryContainer;

    private Dictionary<string, List<string>> addresses = new Dictionary<string, List<string>>
    {
        { "ERC20", new List<string> { } },
        { "ERC721", new List<string> { } },
        { "ERC1155", new List<string> { } }
    };
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
                                switch (int.Parse(rewardTypeItem.Result.ToString()))
                                {
                                    case 1:
                                        Debug.Log("Reward Type: ERC20");
                                        addresses["ERC20"].Add(item.Result.ToString());
                                        //lootboxServiceConfig.Erc20Contracts?.Add(item.Result.ToString());
                                        break;
                                    case 2:
                                        Debug.Log("Reward Type: ERC721");
                                        addresses["ERC721"].Add(item.Result.ToString());
                                        //lootboxServiceConfig.Erc721Contracts?.Add(item.Result.ToString());
                                        // TODO MOVE THIS INTO A FUNCTION
                                        var extraItem721 = innerItem.Find(x => x.Parameter.Name == "extra");
                                        if (extraItem721 != null)
                                        {
                                            try
                                            {
                                                var extraList = JsonConvert.DeserializeObject<List<List<ExtraRewardInfo>>>(extraItem721.Result.ToString());
                                                foreach (var extra in extraList)
                                                {
                                                    Debug.Log($"Token ID found: {extra[0].Id}");
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Debug.LogError($"Error parsing extra field: {e.Message}");
                                            }
                                        }
                                        break;
                                    case 3:
                                        Debug.Log("Reward Type: ERC1155");
                                        addresses["ERC1155"].Add(item.Result.ToString());
                                        //lootboxServiceConfig.Erc1155Contracts?.Add(item.Result.ToString());
                                        var extraItem1155 = innerItem.Find(x => x.Parameter.Name == "extra");
                                        if (extraItem1155 != null)
                                        {
                                            try
                                            {
                                                var extraList = JsonConvert.DeserializeObject<List<List<ExtraRewardInfo>>>(extraItem1155.Result.ToString());
                                                foreach (var extra in extraList)
                                                {
                                                    Debug.Log($"Token ID found: {extra[0].Id}");
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Debug.LogError($"Error parsing extra field: {e.Message}");
                                            }
                                        }
                                        break;
                                    default:
                                        Debug.Log("Reward Type: UNDEFINED");
                                        break;
                                }
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

    private async Task InitializeContracts()
    {
        Debug.Log("Initializing Contracts");
        List<Erc20Contract> erc20Contracts = await BuildContracts<Erc20Contract>(addresses["ERC20"]);
        List<Erc721Contract> erc721Contracts = await BuildContracts<Erc721Contract>(addresses["ERC721"]);
        List<Erc1155Contract> erc1155Contracts = await BuildContracts<Erc1155Contract>(addresses["ERC1155"]);

        List<ItemData> items = new List<ItemData>();

        Debug.Log("Checking balances");
        await CheckErc20Balances(erc20Contracts, items);
        List<BigInteger> tokenIds721 = new List<BigInteger> { 0, 1, 2 };
        await CheckErc721Balances(erc721Contracts, tokenIds721, items);
        List<BigInteger> tokenIds1155 = new List<BigInteger> { 0,1, 2 };
        await CheckErc1155Balances(erc1155Contracts, tokenIds1155, items);

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

    private async Task CheckErc20Balances(List<Erc20Contract> contracts, List<ItemData> items)
    {
        foreach (var contract in contracts)
        {
            var balance = await contract.BalanceOf(Web3Unity.Web3.Signer.PublicAddress);
            var ethBalance = UnitConversion.Convert.FromWei(balance);
            if (balance > BigInteger.Zero)
            {
                Debug.Log("ERC20 Item found!");
                items.Add(new ItemData
                {
                    itemType = "ERC20",
                    itemName = "ERC20 Token",
                    itemAmount = ethBalance.ToString()
                });
            }
        }
    }

    private async Task CheckErc721Balances(List<Erc721Contract> contracts, List<BigInteger> tokenIds, List<ItemData> items)
    {
        foreach (var contract in contracts)
        {
            foreach (var tokenId in tokenIds)
            {
                var owner = await contract.OwnerOf(tokenId);
                if (owner == Web3Unity.Web3.Signer.PublicAddress)
                {
                    Debug.Log("ERC721 Item found!");
                    var uri = await contract.TokenURI(tokenId.ToString());
                    var data = await FetchDataWithRetry(uri);
                    var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);

                    items.Add(new ItemData
                    {
                        itemType = "ERC721",
                        itemId = tokenId.ToString(),
                        itemName = jsonResponse.name,
                        itemAmount = "1"
                    });
                }
            }
        }
    }

    private async Task CheckErc1155Balances(List<Erc1155Contract> contracts, List<BigInteger> tokenIds, List<ItemData> items)
    {
        foreach (var contract in contracts)
        {
            foreach (var tokenId in tokenIds)
            {
                var balance = await contract.BalanceOf(Web3Unity.Web3.Signer.PublicAddress, tokenId);
                Debug.Log($"Balance owned: {balance}");
                Debug.Log($"Current Address: {Web3Unity.Web3.Signer.PublicAddress}");
                if (balance > BigInteger.Zero)
                {
                    Debug.Log("ERC1155 Item found!");
                    var uri = await contract.Uri(tokenId.ToString());
                    Debug.Log(uri);
                    var data = await FetchDataWithRetry(uri);
                    var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
                    // TODO data not showing up here sometimes due to SSL
                    Debug.Log(data);
                    Debug.Log(jsonResponse.name);
                    Debug.Log("Metadata done"); ;
                    items.Add(new ItemData
                    {
                        itemType = "ERC1155",
                        itemId = tokenId.ToString(),
                        itemName = jsonResponse.name,
                        itemAmount = balance.ToString()
                    });
                }
            }
        }
    }
    
    private class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Always return true to bypass certificate validation
            return true;
        }
    }
    
    private async Task<string> FetchDataWithRetry(string uri, int maxRetries = 20, float delayBetweenRetries = 3.0f)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            attempt++;
            Debug.Log($"Attempt {attempt}: Fetching data from {uri}");
        
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                #if UNITY_EDITOR
                    webRequest.certificateHandler = new BypassCertificateHandler();
                #endif
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Web request succeeded!");
                    return webRequest.downloadHandler.text;
                }

                Debug.Log($"Web request failed (Attempt {attempt}/{maxRetries}): {webRequest.error}");
                if (attempt < maxRetries)
                {
                    Debug.Log($"Retrying in {delayBetweenRetries} seconds...");
                    await Task.Delay((int)(delayBetweenRetries * 1000));
                }
                else
                {
                    Debug.LogError("All retry attempts failed.");
                }
            }
        }
        return string.Empty;
    }

    private void SpawnObjects(ItemData[] itemDataArray)
    {
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in itemDataArray)
        {
            var newItem = Instantiate(inventoryObjectPrefab, inventoryContainer);
            var typeText = newItem.transform.Find("TypeText").GetComponent<TextMeshProUGUI>();
            var idText = newItem.transform.Find("IdText").GetComponent<TextMeshProUGUI>();
            var nameText = newItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            var amountText = newItem.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            var modalButton = newItem.transform.Find("Image").GetComponent<Button>();

            typeText.text = item.itemType;
            idText.text = $"#{item.itemId}";
            nameText.text = item.itemName;
            amountText.text = $"Amount: {item.itemAmount}";

            modalButton.onClick.AddListener(() => OpenNftModal(item));
        }
    }

    private void OpenNftModal(ItemData itemData)
    {
        nftModal.SetActive(true);
        EventManager.OnToggleNftModal(itemData);
    }
}