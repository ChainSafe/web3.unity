using System.Collections.Generic;
using System.Globalization;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.Util;
using Newtonsoft.Json;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages lootbox reward data & object spawning.
/// </summary>
public class RewardsManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject rewardsObjectPrefab;
    [SerializeField] private Transform rewardsContainer;
    [SerializeField] private LootboxItem lootboxItemPrefab;
    [SerializeField] private Button closeRewardsButton;

    #endregion

    #region Methods

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        EventManager.Instance.ToggleRewardItems += ParseRewards;
    }

    /// <summary>
    /// Unsubscribes from the event to free up memory.
    /// </summary>
    private void OnDisable()
    {
        EventManager.Instance.ToggleRewardItems -= ParseRewards;
    }

    /// <summary>
    /// Initializes button & functions.
    /// </summary>
    private void Awake()
    {
        closeRewardsButton.onClick.AddListener(CloseRewardsMenu);
    }

    /// <summary>
    /// Closes the rewards menu.
    /// </summary>
    private void CloseRewardsMenu()
    {
        Debug.Log("Close rewards menu");
        ClearPreviousRewards();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Clears the previous reward objects to stop duplicates.
    /// </summary>
    private void ClearPreviousRewards()
    {
        foreach (Transform child in rewardsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Parses reward data and attempts to spawn populated objects.
    /// </summary>
    /// <param name="rewards">Reward data to parse.</param>
    private async void ParseRewards(LootboxRewards rewards)
    {
        // Clear previous items
        ClearPreviousRewards();
        var rewardItems = new List<ItemData>();
        // Add ERC20 rewards
        foreach (var reward in rewards.Erc20Rewards)
        {
            var balance = UnitConversion.Convert.FromWei(reward.AmountRaw);
            rewardItems.Add(new ItemData
            {
                itemType = "ERC20",
                itemName = await Web3Unity.Web3.Erc20.GetName(reward.ContractAddress),
                itemAmount = balance.ToString(CultureInfo.InvariantCulture)
            });
        }

        // Add ERC721 rewards
        foreach (var reward in rewards.Erc721Rewards)
        {
            var data = await Web3Unity.Web3.Erc721.GetUri(ChainSafeContracts.Erc721, reward.TokenId.ToString());
            var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
            rewardItems.Add(new ItemData
            {
                itemImage = jsonResponse.image,
                itemType = "ERC721",
                itemId = $"#{reward.TokenId}",
                itemName = reward.TokenName,
                itemAmount = "1"
            });
        }

        // Add ERC1155 rewards
        foreach (var reward in rewards.Erc1155Rewards)
        {
            var data = await Web3Unity.Web3.Erc1155.GetUri(ChainSafeContracts.Erc1155, reward.TokenId.ToString());
            var jsonResponse = JsonConvert.DeserializeObject<TokenModel.Token>(data);
            rewardItems.Add(new ItemData
            {
                itemImage = jsonResponse.image,
                itemType = "ERC1155",
                itemId = $"#{reward.TokenId}",
                itemName = reward.TokenName,
                itemAmount = reward.Amount.ToString()
            });
        }

        SpawnRewardItem(rewardItems);
    }

    /// <summary>
    /// Spawns reward items.
    /// </summary>
    /// <param name="itemDataArray">Item data to populate objects with.</param>
    private async void SpawnRewardItem(List<ItemData> itemDataArray)
    {
        foreach (var item in itemDataArray)
        {
            var newItem = Instantiate(lootboxItemPrefab, rewardsContainer);
            await newItem.Initialize(item);
        }
    }

    #endregion
}