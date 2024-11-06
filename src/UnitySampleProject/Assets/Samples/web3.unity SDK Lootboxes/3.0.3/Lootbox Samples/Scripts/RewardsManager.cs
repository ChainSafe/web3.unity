using ChainSafe.Gaming.Lootboxes.Chainlink;
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

    private void SpawnRewards(LootboxRewards rewards)
    {
        // Clear previous items
        ClearPreviousRewards();

        // Loop through and spawn rewards based on type
        foreach (var reward in rewards.Erc20Rewards)
        {
            SpawnRewardItem("ERC20", null, null, reward.AmountRaw.ToString());
        }
        foreach (var reward in rewards.Erc721Rewards)
        {
            SpawnRewardItem("ERC721", reward.TokenId.ToString(), reward.TokenName, "1");
        }
        foreach (var reward in rewards.Erc1155Rewards)
        {
            SpawnRewardItem("ERC1155", reward.TokenId.ToString(), reward.TokenName, reward.Amount.ToString());
        }
    }

    // Helper method to create and populate a reward item UI element
    private void SpawnRewardItem(string type, string tokenId, string tokenName, string amount)
    {
        GameObject newItem = Instantiate(rewardsObjectPrefab, rewardsContainer);
        TextMeshProUGUI typeText = newItem.transform.Find("TypeText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI idText = newItem.transform.Find("IdText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = newItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = newItem.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();

        typeText.text = type;
        idText.text = $"#{tokenId}";
        nameText.text = tokenName;
        amountText.text = amount;
    }
}