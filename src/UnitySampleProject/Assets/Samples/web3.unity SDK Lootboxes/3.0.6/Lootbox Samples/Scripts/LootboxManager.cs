using System;
using System.Collections.Generic;
using System.Numerics;
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
    [SerializeField] private TextMeshProUGUI lootboxAmountText;
    [SerializeField] private TMP_Dropdown lootboxDropdown;
    [SerializeField] private GameObject rewardsMenu;
    private ILootboxService lootboxService;
    private Dictionary<int, int> lootboxBalances = new Dictionary<int, int>();
    [SerializeField] private Button claimLootboxButton, recoverLootboxesButton, postToSocialsButton;
    [SerializeField] private GameObject debugButtonsContainer;
    [SerializeField] private Button buyButton, getPriceButton, setPriceButton, claimRewardsAfterButton;
    [SerializeField] private bool debugLootboxes;

    private void Awake()
    {
        claimLootboxButton.onClick.AddListener(OnClaimLootboxClicked);
        recoverLootboxesButton.onClick.AddListener(RecoverLootboxesClicked);
        postToSocialsButton.onClick.AddListener(PostOnSocialMedia);
        lootboxDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        if (debugLootboxes)
        {
            debugButtonsContainer.SetActive(true);
            getPriceButton.onClick.AddListener(GetPrice);
            setPriceButton.onClick.AddListener(SetPrice);
            buyButton.onClick.AddListener(Buy);
            claimRewardsAfterButton.onClick.AddListener(ClaimRewardsClicked);
        }
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

    private async Task CheckLootBoxBalance(int id)
    {
        var lootBoxAmount = await lootboxService.BalanceOf(Web3Unity.Instance.PublicAddress, id);
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
        if (int.TryParse(selectedText.Replace("ID: ", ""), out int selectedId) && lootboxBalances.TryGetValue(selectedId, out var balance))
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
        if (int.TryParse(selectedText.Replace("ID: ", ""), out int selectedId) && lootboxBalances.TryGetValue(selectedId, out int selectedAmount))
        {
            int amountToOpen = 1;
            await lootboxService.OpenLootbox(selectedId, amountToOpen);
        }
        Debug.Log("Claiming rewards");
        await new WaitForSeconds(30);
        await lootboxService.ClaimRewards();
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

    private async void GetPrice()
    {
        var response = await lootboxService.GetPrice();
        Debug.Log(response);
    }
    
    private async void SetPrice()
    {
        // 0.000000001 in eth 18 decimals, setting price low to simulate gas
        var priceToSet = BigInteger.Parse("1000000000");
        await lootboxService.SetPrice(priceToSet);
        Debug.Log($"Price set at: {priceToSet}");
    }
    
    private async void Buy()
    {
        var amountToBuy = 1;
        // 0.0001 in eth 18 decimals,
        var maxPriceToPay = BigInteger.Parse("100000000000000");
        await lootboxService.Buy(amountToBuy, maxPriceToPay);
        Debug.Log($"{amountToBuy} Lootbox purchased");
    }
}