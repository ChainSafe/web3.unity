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
    [SerializeField] private Button claimLootboxButton, recoverLootboxesButton, postToSocialsButton, claimRewardsAfterButton;
    [SerializeField] private TextMeshProUGUI lootboxAmountText;
    [SerializeField] private TMP_Dropdown lootboxDropdown;
    [SerializeField] private GameObject rewardsMenu;
    private ILootboxService lootboxService;
    private Dictionary<uint, uint> lootboxBalances = new Dictionary<uint, uint>();

    private void Awake()
    {
        claimRewardsAfterButton.onClick.AddListener(ClaimRewardsClicked);
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
        lootboxService.OnRewardsClaimed += OpenRewardsMenu;
        GetLootboxTypes();
    }

    public void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
        lootboxService.OnRewardsClaimed -= OpenRewardsMenu;
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

    private async void ClaimRewardsClicked()
    {
        rewardsMenu.SetActive(true);
        await lootboxService.ClaimRewards();
    }

    private async Task CheckLootBoxBalance(uint id)
    {
        var lootBoxAmount = await lootboxService.BalanceOf(Web3Unity.Instance.Address, id);
        if (lootBoxAmount == 0) return;
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
        if (uint.TryParse(selectedText.Replace("ID: ", ""), out uint selectedId) && lootboxBalances.TryGetValue(selectedId, out var balance))
        {
            claimLootboxButton.interactable = balance > 0;
            lootboxAmountText.text = balance.ToString(); 
        }
        else
        {
            claimLootboxButton.interactable = false;
        }
    }

    private async void OnClaimLootboxClicked()
    {
        await ClaimLootbox();
    }

    private async Task ClaimLootbox()
    {
        var selectedText = lootboxDropdown.options[lootboxDropdown.value].text;
        Debug.Log("Claiming Lootbox");
        if (uint.TryParse(selectedText.Replace("ID: ", ""), out uint selectedId) && lootboxBalances.TryGetValue(selectedId, out uint selectedAmount))
        {
            await lootboxService.OpenLootbox(selectedId);
        }
        Debug.Log("Claimed Lootbox");
        Debug.Log("Claiming rewards");
        await new WaitForSeconds(30);
        await lootboxService.ClaimRewards();
        Debug.Log("Claimed rewards");
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