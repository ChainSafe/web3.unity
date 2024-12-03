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

/// <summary>
/// Manages lootbox reads & writes, displays them nicely on screen.
/// </summary>
public class LootboxManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI lootboxAmountText;
    [SerializeField] private TMP_Dropdown lootboxDropdown;
    [SerializeField] private GameObject rewardsMenu;
    private ILootboxService lootboxService;
    private Dictionary<int, int> lootboxBalances = new Dictionary<int, int>();
    [SerializeField] private Button claimLootboxButton, recoverLootboxesButton, postToSocialsButton;
    [SerializeField] private GameObject debugButtonsContainer;
    [SerializeField] private Button buyButton, getPriceButton, setPriceButton, claimRewardsAfterButton;
    [SerializeField] private bool debugLootboxes;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes button & functions.
    /// </summary>
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

    /// <summary>
    /// Executes when the web3 object is initialized.
    /// </summary>
    /// <param name="valueTuple">Value tuple containing web3 object and lightweight bool.</param>
    private void Web3Initialized((Web3 web3, bool isLightweight) valueTuple)
    {
        if (valueTuple.isLightweight) return;
        lootboxService = Web3Unity.Web3.Chainlink().Lootboxes();
        lootboxService.OnRewardsClaimed += OpenRewardsMenu;
        GetLootboxTypes();
    }

    /// <summary>
    /// Unsubscribes from the event to free up memory.
    /// </summary>
    public void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
        lootboxService.OnRewardsClaimed -= OpenRewardsMenu;
        lootboxDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    /// <summary>
    /// Gets lootbox types, these correlate to the amount of items in the lootbox.
    /// i.e 1 = 1 item, 2 = 2 items, these numbers can also be used to specify rarity.
    /// </summary>
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

    /// <summary>
    /// Executes when the claim rewards button is clicked.
    /// opens the rewards menu and make a write call to claim rewards.
    /// </summary>
    private async void ClaimRewardsClicked()
    {
        rewardsMenu.SetActive(true);
        await lootboxService.ClaimRewards();
    }

    /// <summary>
    /// Checks lootbox balances for a user.
    /// </summary>
    /// <param name="id">The lootbox id/type to check.</param>
    private async Task CheckLootBoxBalance(int id)
    {
        var lootBoxAmount = await lootboxService.BalanceOf(Web3Unity.Instance.PublicAddress, id);
        if (lootBoxAmount == 0) return;
        Debug.Log($"LootBox Balance for ID {id} = {lootBoxAmount}");
        lootboxBalances[id] = lootBoxAmount;
        lootboxDropdown.options.Add(new TMP_Dropdown.OptionData($"ID: {id}"));
    }

    /// <summary>
    /// Updates balance text for the chose lootbox id/type.
    /// </summary>
    /// <param name="index">The dropdown index changed to.</param>
    private void OnDropdownValueChanged(int index)
    {
        UpdateBalanceText();
    }

    /// <summary>
    /// Updates balance text for the chose lootbox id/type.
    /// </summary>
    private void UpdateBalanceText()
    {
        var selectedText = lootboxDropdown.options[lootboxDropdown.value].text;
        if (int.TryParse(selectedText.Replace("ID: ", ""), out int selectedId) &&
            lootboxBalances.TryGetValue(selectedId, out var balance))
        {
            claimLootboxButton.interactable = balance > 0;
            lootboxAmountText.text = balance.ToString();
        }
        else
        {
            claimLootboxButton.interactable = false;
        }
    }

    /// <summary>
    /// Executes when claim lootbox is clicked.
    /// </summary>
    private async void OnClaimLootboxClicked()
    {
        await ClaimLootbox();
    }

    /// <summary>
    /// Claims a lootbox for the user.
    /// It will first open a lootbox if available, then it will wait for the proof to be published.
    /// Finally, it will attempt to claim the lootbox rewards & populate the reward model display.
    /// </summary>
    private async Task ClaimLootbox()
    {
        var selectedText = lootboxDropdown.options[lootboxDropdown.value].text;
        Debug.Log("Claiming Lootbox");
        if (int.TryParse(selectedText.Replace("ID: ", ""), out int selectedId) &&
            lootboxBalances.TryGetValue(selectedId, out int selectedAmount))
        {
            int amountToOpen = 1;
            await lootboxService.OpenLootbox(selectedId, amountToOpen);
        }

        Debug.Log("Claiming rewards");
        await new WaitForSeconds(30);
        await lootboxService.ClaimRewards();
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

    #region Debug Methods

    /// <summary>
    /// DEBUG: Fires when rcover lootboxes is clicked.
    /// </summary>
    private async void RecoverLootboxesClicked()
    {
        await RecoverLootboxes();
    }

    /// <summary>
    /// DEBUG: Attempts to recover lootboxes if something goes wrong when claiming.
    /// </summary>
    private async Task RecoverLootboxes()
    {
        await lootboxService.RecoverLootboxes();
    }

    /// <summary>
    /// DEBUG: Gets the price to open a lootbox.
    /// </summary>
    private async void GetPrice()
    {
        var response = await lootboxService.GetPrice();
        Debug.Log(response);
    }

    /// <summary>
    /// DEBUG: - LOOTBOX DEPLOYER ONLY - Sets the price to open a lootbox.
    /// </summary>
    private async void SetPrice()
    {
        // 0.000000001 in eth 18 decimals, setting price low to simulate gas
        var priceToSet = BigInteger.Parse("1000000000");
        await lootboxService.SetPrice(priceToSet);
        Debug.Log($"Price set at: {priceToSet}");
    }

    /// <summary>
    /// DEBUG: Buys a lootbox granted that the price has been set above 0.
    /// </summary>
    private async void Buy()
    {
        var amountToBuy = 1;
        // 0.0001 in eth 18 decimals,
        var maxPriceToPay = BigInteger.Parse("100000000000000");
        await lootboxService.Buy(amountToBuy, maxPriceToPay);
        Debug.Log($"{amountToBuy} Lootbox purchased");
    }

    #endregion

    #endregion
}