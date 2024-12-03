using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.Util;
using Newtonsoft.Json;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardsObjectPrefab;
    [SerializeField] private Transform rewardsContainer;
    [SerializeField] private LootboxItem lootboxItemPrefab;
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
        Debug.Log("Close rewards menu");
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

    private async void SpawnRewards(LootboxRewards rewards)
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
    
    private async void SpawnRewardItem(List<ItemData> itemDataArray)
    {
        foreach (var item in itemDataArray)
        {
            var newItem = Instantiate(lootboxItemPrefab, rewardsContainer);
            await newItem.Initialize(item);
        }
    }
}