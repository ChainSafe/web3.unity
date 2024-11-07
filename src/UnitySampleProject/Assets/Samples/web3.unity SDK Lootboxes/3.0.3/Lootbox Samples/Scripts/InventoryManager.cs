using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryObjectPrefab, nftModal;
    [SerializeField] private Transform inventoryContainer;
    private LootboxServiceConfig lootboxServiceConfig;
    private Erc20Contract _erc20;
    private Erc721Contract _erc721;
    private Erc1155Contract _erc1155;

    private void OnEnable()
    {
        EventManager.ToggleInventoryItems += SpawnObjects;
    }

    private void OnDisable()
    {
        EventManager.ToggleInventoryItems -= SpawnObjects;
    }

    private async void Start()
    {
        // Initialize contracts from service config
        lootboxServiceConfig = new LootboxServiceConfig();
        // Initialize the ERC20, ERC721, and ERC1155 contracts dynamically
        List<Erc20Contract> erc20Contracts = new List<Erc20Contract>();
        List<Erc721Contract> erc721Contracts = new List<Erc721Contract>();
        List<Erc1155Contract> erc1155Contracts = new List<Erc1155Contract>();

        if (lootboxServiceConfig.Erc20Contracts != null)
            foreach (var erc20Address in lootboxServiceConfig.Erc20Contracts)
            {
                var erc20Contract = await Web3Unity.Web3.ContractBuilder.Build<Erc20Contract>(erc20Address);
                erc20Contracts.Add(erc20Contract);
            }

        if (lootboxServiceConfig.Erc721Contracts != null)
            foreach (var erc721Address in lootboxServiceConfig.Erc721Contracts)
            {
                var erc721Contract = await Web3Unity.Web3.ContractBuilder.Build<Erc721Contract>(erc721Address);
                erc721Contracts.Add(erc721Contract);
            }

        if (lootboxServiceConfig.Erc1155Contracts != null)
            foreach (var erc1155Address in lootboxServiceConfig.Erc1155Contracts)
            {
                var erc1155Contract = await Web3Unity.Web3.ContractBuilder.Build<Erc1155Contract>(erc1155Address);
                erc1155Contracts.Add(erc1155Contract);
            }

        List<ItemData> items = new List<ItemData>();

        // Check ERC20 balances
        foreach (var erc20 in erc20Contracts)
        {
            var erc20Balance = await erc20.BalanceOf(Web3Unity.Web3.Signer.PublicAddress);
            if (erc20Balance > BigInteger.Zero)
            {
                items.Add(new ItemData
                {
                    itemType = "ERC20",
                    itemId = "",
                    itemName = "ERC20 Token",
                    itemAmount = erc20Balance.ToString()
                });
            }
        }

        // Check ERC721 balances for each token ID
        foreach (var erc721 in erc721Contracts)
        {
            if (lootboxServiceConfig.Erc721TokenIds == null) continue;
            foreach (var tokenId in lootboxServiceConfig.Erc721TokenIds)
            {
                var erc721Owner = await erc721.OwnerOf(tokenId);
                if (erc721Owner == Web3Unity.Web3.Signer.PublicAddress)
                {
                    var uri = await erc721.TokenURI(tokenId.ToString());
                    Debug.Log(uri);
                    var tokenData = JsonConvert.DeserializeObject<TokenModel.Token>(uri);

                    items.Add(new ItemData
                    {
                        itemType = "ERC721",
                        itemId = tokenId.ToString(),
                        itemName = tokenData.name,
                        itemAmount = "1"
                    });
                }
            }
        }

        // Check ERC1155 balances for each token ID
        foreach (var erc1155 in erc1155Contracts)
        {
            if (lootboxServiceConfig.Erc1155TokenIds == null) continue;
            foreach (var tokenId in lootboxServiceConfig.Erc1155TokenIds)
            {
                var erc1155Balance = await erc1155.BalanceOf(Web3Unity.Web3.Signer.PublicAddress, tokenId);
                if (erc1155Balance > BigInteger.Zero)
                {
                    var uri = await erc1155.Uri(tokenId.ToString());
                    Debug.Log(uri);
                    var tokenData = JsonConvert.DeserializeObject<TokenModel.Token>(uri);

                    items.Add(new ItemData
                    {
                        itemType = "ERC1155",
                        itemId = tokenId.ToString(),
                        itemName = tokenData.name,
                        itemAmount = erc1155Balance.ToString()
                    });
                }
            }
        }

        // Check if any data is found
        if (items.Count == 0)
        {
            Debug.Log("No tokens found for the user!");
            return;
        }

        // Pass all collected items to inventory event
        EventManager.OnToggleInventoryItems(items.ToArray());
    }

    private void SpawnObjects(ItemData[] itemDataArray)
    {
        // Clear previous items
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < itemDataArray.Length; i++)
        {
            GameObject newItem = Instantiate(inventoryObjectPrefab, inventoryContainer);
            TextMeshProUGUI typeText = newItem.transform.Find("TypeText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI idText = newItem.transform.Find("IdText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = newItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI amountText = newItem.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            Button modalButton = newItem.transform.Find("Image").GetComponent<Button>();

            if (itemDataArray != null && i < itemDataArray.Length)
            {
                ItemData currentItemData = itemDataArray[i];

                // Set UI elements based on item type
                switch (currentItemData.itemType)
                {
                    case "ERC20":
                        typeText.text = "ERC20";
                        idText.text = $"#{currentItemData.itemId}";
                        nameText.text = currentItemData.itemName;
                        amountText.text = $"Balance: {currentItemData.itemAmount}";
                        break;

                    case "ERC721":
                        typeText.text = "ERC721";
                        idText.text = $"#{currentItemData.itemId}";
                        nameText.text = currentItemData.itemName;
                        amountText.text = "Unique Item";
                        break;

                    case "ERC1155":
                        typeText.text = "ERC1155";
                        idText.text = $"#{currentItemData.itemId}";
                        nameText.text = currentItemData.itemName;
                        amountText.text = $"Amount: {currentItemData.itemAmount}";
                        break;
                }

                // Set up button to open modal with the item's data if opened
                modalButton.onClick.AddListener(() => OpenNftModal(currentItemData));
            }
        }
    }

    private void OpenNftModal(ItemData itemData)
    {
        nftModal.SetActive(true);
        EventManager.OnToggleNftModal(itemData);
    }
}