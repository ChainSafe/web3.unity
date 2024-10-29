using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
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
        claimLootboxButton.onClick.AddListener(CanClaimRewards);
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
        Debug.Log($"Checking balance for first lootbox type id: {lootBoxTypeBalanceIds[0]}");
        CheckLootBoxBalance(lootBoxTypeBalanceIds[0]);
    }

    private async void CheckLootBoxBalance(BigInteger id)
    {
        var lootBoxAmount = await lootboxUsageSample.BalanceOf(Web3Unity.Instance.Address, id);
        Debug.Log($"LootBox Balance: {lootBoxAmount}");
        lootboxAmountText.text = lootBoxAmount.ToString();
    }

    private async void CanClaimRewards()
    {
        var response = await lootboxUsageSample.CanClaimRewards(Web3Unity.Instance.Address);
        if (response)
        {
            Debug.Log("Rewards can be claimed");
            ClaimLootBoxRewards();
        }
        else
        {
            Debug.Log("Rewards cannot be claimed");
        }
    }

    private async void ClaimLootBoxRewards()
    {
        Debug.Log("Claiming rewards");
        var response = await lootboxUsageSample.ClaimRewardsWithReceipt(Web3Unity.Instance.Address);
        Debug.Log($"Rewards call response receipt: {response.TransactionHash}");
        var logs = response.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
        var eventAbi = EventExtensions.GetEventABI<RewardsClaimedEvent>();
        var eventLogs = logs
            .Select(log => eventAbi.DecodeEvent<RewardsClaimedEvent>(log))
            .Where(l => l != null);

        if (!eventLogs.Any())
        {
            throw new Web3Exception("No RewardsClaimed events were found in log's receipt.");
        }

        var rewards = LootboxRewards.Empty;

        foreach (var eventLog in eventLogs)
        {
            var eventData = eventLog.Event;
            // TODO FIND THIS FUNCTION
            //var rewardType = rewardTypeByTokenAddress[eventData.TokenAddress];
            var rewardType = RewardType.Erc1155;
            switch (rewardType)
            {
                // Erc20 Tokens
                case RewardType.Erc20:
                    rewards.Erc20Rewards.Add(new Erc20Reward
                    {
                        ContractAddress = eventData.TokenAddress,
                        AmountRaw = eventData.Amount,
                    });
                    break;
                // Erc721 NFTs
                case RewardType.Erc721:
                    rewards.Erc721Rewards.Add(new Erc721Reward
                    {
                        ContractAddress = eventData.TokenAddress,
                        TokenId = eventData.TokenId,
                    });
                    break;
                // Erc1155 NFTs
                case RewardType.Erc1155:
                    rewards.Erc1155Rewards.Add(new Erc1155Reward
                    {
                        ContractAddress = eventData.TokenAddress,
                        TokenId = eventData.TokenId,
                        Amount = eventData.Amount,
                    });
                    break;
                // Single Erc1155 NFT
                case RewardType.Erc1155Nft:
                    rewards.Erc1155NftRewards.Add(new Erc1155NftReward
                    {
                        ContractAddress = eventData.TokenAddress,
                        TokenId = eventData.TokenId,
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        OpenLootBox();
    }

    private async Task<BigInteger> CalculateOpenPrice()
    {
        var gas = BigInteger.Parse("100000");
        var units = BigInteger.Parse("1");
        var openPrice = await lootboxUsageSample.CalculateOpenPrice(gas, gas, units);
        Debug.Log($"Open price: {openPrice}");
        return openPrice;
    }

    private async void OpenLootBox()
    {
        var gas = BigInteger.Parse("100000");
        var lootIds = new[] { BigInteger.Parse("2") };
        var lootAmount = new[] { BigInteger.Parse("2") };
        var openPrice = await CalculateOpenPrice();
        var response = await lootboxUsageSample.OpenWithReceipt(gas, lootIds, lootAmount, new TransactionRequest { Value = new HexBigInteger(openPrice) });
        Debug.Log($"Open call response receipt: {response.TransactionHash}");
    }

    private void PostOnSocialMedia()
    {
        string message = "I just opened a lootBox!";
        string url = "https://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(message);
        Application.OpenURL(url);
    }
}
