using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.Web3;
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
    private LootboxUsageSample _lootboxUsageSample;
    
    private void Awake()
    {
        claimLootboxButton.onClick.AddListener(OpenLootBox);
        postToSocialsButton.onClick.AddListener(PostOnSocialMedia);
        Web3Unity.Web3Initialized += Web3Initialized;
    }

    private async void Web3Initialized((Web3 web3, bool isLightweight) valueTuple)
    {
        if (_lootboxUsageSample != null)
        {
            await _lootboxUsageSample.DisposeAsync();
        }
        _lootboxUsageSample = await valueTuple.web3.ContractBuilder.Build<LootboxUsageSample>(lootBoxContract);
        CheckLootBoxBalanceTypes();
    }

    public async void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
        await _lootboxUsageSample.DisposeAsync();
    }

    private async void CheckLootBoxBalanceTypes()
    {
        var lootBoxTypes = await _lootboxUsageSample.GetLootboxTypes();
        foreach (var lootBoxType in lootBoxTypes)
        {
            Debug.Log(lootBoxType);
        }
        CheckLootBoxBalanceBatch(lootBoxTypes);
    }
    
    private async void CheckLootBoxBalanceBatch(BigInteger[] types)
    {
        var lootBoxTypeBalanceIds = await _lootboxUsageSample.BalanceOfBatch(new[] { Web3Unity.Instance.Address }, types);
        foreach (var lootBoxTypeId in lootBoxTypeBalanceIds)
        {
            Debug.Log(lootBoxTypeId);
        }
        CheckLootBoxBalance(lootBoxTypeBalanceIds[0]);
    }

    private async void CheckLootBoxBalance(BigInteger id)
    {
        var lootBoxAmount = await _lootboxUsageSample.BalanceOf(Web3Unity.Instance.Address, id);
        Debug.Log($"LootBox Balance: {lootBoxAmount}");
        lootboxAmountText.text = lootBoxAmount.ToString();
    }

    private async void OpenLootBox()
    {
        var gas = BigInteger.Parse("90000");
        var lootIds = new [] {BigInteger.Parse("1")};
        var lootAmount = new [] {BigInteger.Parse("1")};
        var response = await _lootboxUsageSample.OpenWithReceipt(gas, lootIds, lootAmount);
        Debug.Log($"Open call response receipt: {response}");
        Debug.Log("Claiming rewards");
        ClaimLootBoxRewards(response.ToString());
    }

    private async void ClaimLootBoxRewards(string receipt)
    {
        var response = await _lootboxUsageSample.ClaimRewardsWithReceipt(receipt);
        Debug.Log($"Rewards call response receipt: {response}");
    }

    private void PostOnSocialMedia()
    {
        string message = "I just opened a lootBox!";
        string url = "https://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(message);
        Application.OpenURL(url);
    }
}
