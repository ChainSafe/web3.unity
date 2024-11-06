using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardsObjectPrefab;
    [SerializeField] private Transform rewardsContainer;
    [SerializeField] private Button closeRewardsButton;

    private void OnEnable()
    {
        EventManager.ToggleRewardItems += SpawnRewards;
    }

    private void OnDisable()
    {
        EventManager.ToggleRewardItems -= SpawnRewards;
    }

    private void Awake()
    {
        closeRewardsButton.onClick.AddListener(CloseRewardsMenu);
    }

    private void CloseRewardsMenu()
    {
        ClearPreviousRewards();
        gameObject.SetActive(false);
    }

    private void ClearPreviousRewards()
    {
        foreach (Transform child in rewardsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnRewards(TransactionReceipt txReceipt)
    {
        // Clear previous items
        ClearPreviousRewards();
        ItemData[] itemDataArray = new ItemData[] { };
        // Populate item data
        for (int i = 0; i < itemDataArray.Length; i++)
        {
            GameObject newItem = Instantiate(rewardsObjectPrefab, rewardsContainer);
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
            }
        }
    }
}