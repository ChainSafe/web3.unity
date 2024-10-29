using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.UnityPackage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Web3 = ChainSafe.Gaming.Web3.Web3;

public class LootboxManager : MonoBehaviour
{
    [SerializeField] private Button claimLootboxButton, postToSocialsButton;
    [SerializeField] private TextMeshProUGUI lootboxAmountText;
    private string lootBoxContract = "0xa31B74DF647979D50f155C7de5b80e9BA3A0C979";
    private LootboxUsageSample lootboxUsageSample;
    
    private void Awake()
    {
        claimLootboxButton.onClick.AddListener(OpenLootBox);
        postToSocialsButton.onClick.AddListener(PostOnSocialMedia);
        Web3Unity.Web3Initialized += Web3Initialized;
    }

    private async void Web3Initialized((Web3 web3, bool isLightweight) valueTuple)
    {
        if (valueTuple.isLightweight) return;
        if (lootboxUsageSample != null)
        {
            await lootboxUsageSample.DisposeAsync();
        }
        lootboxUsageSample = await valueTuple.web3.ContractBuilder.Build<LootboxUsageSample>(lootBoxContract);
        Debug.Log("Lootbox contract built");
        CheckLootBoxBalanceTypes();
    }

    public async void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
        await lootboxUsageSample.DisposeAsync();
    }

    private async void CheckLootBoxBalanceTypes()
    {
        var lootBoxTypes = await lootboxUsageSample.GetLootboxTypes();
        foreach (var lootBoxType in lootBoxTypes)
        {
            Debug.Log($"Lootbox balance types: {lootBoxType}");
        }
        CheckLootBoxBalanceBatch(lootBoxTypes);
    }
    
    private async void CheckLootBoxBalanceBatch(List<BigInteger> types)
    {
        var lootBoxTypeBalanceIds = await lootboxUsageSample.BalanceOfBatch(new[] { Web3Unity.Instance.Address }, types);
        foreach (var lootBoxTypeId in lootBoxTypeBalanceIds)
        {
            Debug.Log($"Lootbox type id: {lootBoxTypeId}");
        }
        Debug.Log($"checking balance for first lootbox type id: {lootBoxTypeBalanceIds[0]}");
        CheckLootBoxBalance(lootBoxTypeBalanceIds[0]);
    }

    private async void CheckLootBoxBalance(BigInteger id)
    {
        var lootBoxAmount = await lootboxUsageSample.BalanceOf(Web3Unity.Instance.Address, id);
        Debug.Log($"LootBox Balance: {lootBoxAmount}");
        lootboxAmountText.text = lootBoxAmount.ToString();
    }

    private async void OpenLootBox()
    {
        var gas = BigInteger.Parse("90000");
        var lootIds = new [] {BigInteger.Parse("1")};
        var lootAmount = new [] {BigInteger.Parse("1")};
        var response = await lootboxUsageSample.OpenWithReceipt(gas, lootIds, lootAmount);
        Debug.Log($"Open call response receipt: {response}");
        Debug.Log("Claiming rewards");
        ClaimLootBoxRewards(response.ToString());
    }

    private async void ClaimLootBoxRewards(string receipt)
    {
        var response = await lootboxUsageSample.ClaimRewardsWithReceipt(receipt);
        Debug.Log($"Rewards call response receipt: {response}");
    }

    private void PostOnSocialMedia()
    {
        string message = "I just opened a lootBox!";
        string url = "https://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(message);
        Application.OpenURL(url);
    }
}
