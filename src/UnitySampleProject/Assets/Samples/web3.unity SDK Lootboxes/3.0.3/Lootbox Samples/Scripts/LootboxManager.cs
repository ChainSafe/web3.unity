using System.Collections.Generic;
using System.Threading.Tasks;
using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Web3 = ChainSafe.Gaming.Web3.Web3;

public class LootboxManager : MonoBehaviour
{
    [SerializeField] private Button claimLootboxButton, recoverLootboxesButton, postToSocialsButton;
    [SerializeField] private TextMeshProUGUI lootboxAmountText;
    [SerializeField] private TMP_Dropdown lootboxDropdown;
    [SerializeField] private GameObject rewardsMenu;
    private ILootboxService lootboxService;
    private Dictionary<uint, uint> lootboxBalances = new Dictionary<uint, uint>();

    private void Awake()
    {
        claimLootboxButton.onClick.AddListener(OnClaimLootboxClicked);
        recoverLootboxesButton.onClick.AddListener(RecoverLootboxesClicked);
        postToSocialsButton.onClick.AddListener(PostOnSocialMedia);
        lootboxDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        Web3Unity.Web3Initialized += Web3Initialized;
    }

    private void Web3Initialized((Web3 web3, bool isLightweight) valueTuple)
    {
        if (valueTuple.isLightweight) return;
        lootboxService = Web3Unity.Web3.Chainlink().Lootboxes();
        GetLootboxTypes();
    }

    public void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
        lootboxDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    private async void GetLootboxTypes()
    {
        var lootBoxTypes = await lootboxService.GetLootboxTypes();
        lootboxDropdown.options.Clear();
        lootboxBalances.Clear();

        foreach (var lootBoxType in lootBoxTypes)
        {
            await CheckLootBoxBalance(lootBoxType);
        }

        if (lootboxDropdown.options.Count > 0)
        {
            lootboxDropdown.value = 0;
            lootboxDropdown.RefreshShownValue();
            UpdateBalanceText();
        }
    }

    private async Task CheckLootBoxBalance(uint id)
    {
        var lootBoxAmount = await lootboxService.BalanceOf(Web3Unity.Instance.Address, id);
        Debug.Log($"LootBox Balance for ID {id} = {lootBoxAmount}");
        lootboxBalances[id] = lootBoxAmount;
        lootboxDropdown.options.Add(new TMP_Dropdown.OptionData($"ID: {id}"));
    }

    private void OnDropdownValueChanged(int index)
    {
        UpdateBalanceText();
    }

    private void UpdateBalanceText()
    {
        var selectedText = lootboxDropdown.options[lootboxDropdown.value].text;
        if (uint.TryParse(selectedText.Replace("ID: ", ""), out uint selectedId) && lootboxBalances.ContainsKey(selectedId))
        {
            lootboxAmountText.text = lootboxBalances[selectedId].ToString();
        }
    }

    private async void OnClaimLootboxClicked()
    {
        var selectedText = lootboxDropdown.options[lootboxDropdown.value].text;
        if (uint.TryParse(selectedText.Replace("ID: ", ""), out uint selectedId) && lootboxBalances.TryGetValue(selectedId, out uint selectedAmount))
        {
            await OpenLootBox(selectedId, selectedAmount);
        }
        // Claim rewards after opening
        var rewards = await lootboxService.ClaimRewards();
        OpenRewardsMenu(rewards);
    }

    private async Task OpenLootBox(uint lootId, uint lootAmount)
    {
        await lootboxService.OpenLootbox(lootId, lootAmount);
    }

    private async void RecoverLootboxesClicked()
    {
        await RecoverLootboxes();
    }

    private async Task RecoverLootboxes()
    {
        await lootboxService.RecoverLootboxes();
    }

    private void OpenRewardsMenu(LootboxRewards rewards)
    {
        rewardsMenu.SetActive(true);
        EventManager.OnToggleRewardItems(rewards);
    }

    private void PostOnSocialMedia()
    {
        string message = "I just opened a lootBox!";
        string url = "https://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(message);
        Application.OpenURL(url);
    }
}