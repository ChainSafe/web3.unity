using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryObjectPrefab, nftModal;
    [SerializeField] private Transform inventoryContainer;
    private Erc20Contract _erc20;
    private Erc721Contract _erc721;
    private Erc1155Contract _erc1155;

    // TODO use contracts from service adapter config rather than hard coding here
    private string collectionContractErc20 = "0x4be0f897c0853f2b05b85f74c4180d8e243c6ccf";
    private string collectionContractErc721 = "0x4be0f897c0853f2b05b85f74c4180d8e243c6ccf";
    private string collectionContractErc1155 = "0x4be0f897c0853f2b05b85f74c4180d8e243c6ccf";
    private string lootBoxContract = "0xa31B74DF647979D50f155C7de5b80e9BA3A0C979";

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
    // Initialize contracts
    _erc20 = await Web3Unity.Web3.ContractBuilder.Build<Erc20Contract>(collectionContractErc20);
    _erc721 = await Web3Unity.Web3.ContractBuilder.Build<Erc721Contract>(collectionContractErc721);
    _erc1155 = await Web3Unity.Web3.ContractBuilder.Build<Erc1155Contract>(collectionContractErc1155);

    // Fetch balances
    var erc20Balance = await _erc20.BalanceOf(Web3Unity.Web3.Signer.PublicAddress);
    var erc721Balance = await _erc721.BalanceOf(Web3Unity.Web3.Signer.PublicAddress);
    // TODO figure out how to fetch token IDs owned from API
    var tokenId = 0;
    var erc1155Balance = await _erc1155.BalanceOf(Web3Unity.Web3.Signer.PublicAddress, tokenId);
    string tokenType;
    if (erc20Balance > BigInteger.Zero)
    {
        tokenType = "ERC20";
    }
    else if (erc721Balance > BigInteger.Zero)
    {
        tokenType = "ERC721";
    }
    else if (erc1155Balance > BigInteger.Zero)
    {
        tokenType = "ERC1155";
    }
    else
    {
        Debug.Log("No tokens found for the user!");
        return;
    }

    ItemData itemData;
    switch (tokenType)
    {
        case "ERC20":
            itemData = new ItemData
            {
                itemType = "ERC20",
                itemId = "-",
                itemName = "ERC20 Token",
                itemAmount = erc20Balance.ToString()
            };
            break;

        case "ERC721":
            itemData = new ItemData
            {
                itemType = "ERC721",
                itemId = null,
                itemName = "ERC721 Token",
                itemAmount = erc721Balance.ToString()
            };
            break;

        case "ERC1155":
            var uri = await _erc1155.Uri(tokenId.ToString());
            Debug.Log(uri);
            var tokenData = JsonConvert.DeserializeObject<TokenModel.Token>(uri);

            itemData = new ItemData
            {
                itemType = "ERC1155",
                itemId = tokenId.ToString(),
                itemName = tokenData.metadata.attributes[0].trait_type,
                itemAmount = erc1155Balance.ToString()
            };
            break;

        default:
            Debug.LogError("Unsupported token type!");
            return;
    }
    ItemData[] items = new ItemData[] { itemData };
    EventManager.OnToggleInventoryItems(items);
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

                // Set up button to open modal with the item's data
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