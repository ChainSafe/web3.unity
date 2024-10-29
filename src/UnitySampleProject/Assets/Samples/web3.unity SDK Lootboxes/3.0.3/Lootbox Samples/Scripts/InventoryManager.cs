using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
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
    private string collectionContract = "0x4be0f897c0853f2b05b85f74c4180d8e243c6ccf";
    private string lootBoxContract = "0xa31B74DF647979D50f155C7de5b80e9BA3A0C979";
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
        while (!Web3Unity.Connected)
        {
            await Task.Delay(1000);
        }
        _erc1155 = await Web3Unity.Web3.ContractBuilder.Build<Erc1155Contract>(collectionContract);
        var tokenId = 0;
        var balance = await _erc1155.BalanceOf(Web3Unity.Web3.Signer.PublicAddress, tokenId);
        if (balance < BigInteger.Parse("1"))
        {
            Debug.Log("No nft items found!");
            return;
        }
        var uri = await _erc1155.Uri(tokenId.ToString());
        Debug.Log(uri);
        var tokenData = JsonConvert.DeserializeObject<Token>(uri);
        ItemData[] items = new ItemData[]
        {
            new ItemData
            {
                itemType = tokenData.tokenType,
                itemId = 0.ToString(),
                itemName = tokenData.metadata.attributes[0].trait_type,
                itemAmount = balance.ToString()
            }
        };
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

            // Assign the specific item data to UI elements and button
            if (itemDataArray != null && i < itemDataArray.Length)
            {
                ItemData currentItemData = itemDataArray[i];
                typeText.text = currentItemData.itemType;
                idText.text = $"#{currentItemData.itemId}";
                nameText.text = currentItemData.itemName;
                amountText.text = currentItemData.itemAmount;
                
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
    
    public class Token
    {
        public string description { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string tokenType { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public List<Attribute> attributes { get; set; }
    }

    public class Attribute
    {
        public string trait_type { get; set; }
    }
}