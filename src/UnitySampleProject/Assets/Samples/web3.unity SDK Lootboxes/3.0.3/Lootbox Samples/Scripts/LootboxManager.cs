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
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;
using Web3 = ChainSafe.Gaming.Web3.Web3;

public class LootboxManager : MonoBehaviour
{
    [SerializeField] private Button claimLootboxButton, postToSocialsButton;
    [SerializeField] private TextMeshProUGUI lootboxAmountText;
    private string lootBoxContract = "0xa31B74DF647979D50f155C7de5b80e9BA3A0C979";
    private LootboxUsageSample lootboxUsageSample;
    private BigInteger[] lootIds;
    private BigInteger[] lootAmount;

    private void Awake()
    {
        claimLootboxButton.onClick.AddListener(() => OpenLootBox(lootIds, lootAmount));
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
        var addresses = new string[types.Count];
        for (int i = 0; i < types.Count; i++)
        {
            addresses[i] = Web3Unity.Instance.Address;
        }
        var lootBoxTypeBalanceIds = await lootboxUsageSample.BalanceOfBatch(addresses, types);
        lootIds = new BigInteger[lootBoxTypeBalanceIds.Count];
        lootAmount = new BigInteger[lootBoxTypeBalanceIds.Count];

        if (lootBoxTypeBalanceIds.Count > 0)
        {
            for (int i = 0; i < lootBoxTypeBalanceIds.Count; i++)
            {
                var lootBoxTypeId = lootBoxTypeBalanceIds[i];
                Debug.Log($"Lootbox type id: {lootBoxTypeId}, checking balance...");
                await CheckLootBoxBalance(lootBoxTypeId, i);
            }
        }
        else
        {
            Debug.Log("No lootboxes found, mint some from the dashboard or purchase");
        }
    }

    private async Task CheckLootBoxBalance(BigInteger id, int index)
    {
        if (Web3Unity.Instance.Address == null) return;

        var lootBoxAmount = await lootboxUsageSample.BalanceOf(Web3Unity.Instance.Address, id);
        Debug.Log($"LootBox Balance for ID {id} = {lootBoxAmount}");
        lootIds[index] = id;
        lootAmount[index] = lootBoxAmount;

        lootboxAmountText.text = lootBoxAmount.ToString();
    }

    private async void CanClaimRewards(TransactionReceipt txResponse)
    {
        var response = await lootboxUsageSample.CanClaimRewards(Web3Unity.Instance.Address);
        if (response)
        {
            Debug.Log("Rewards can be claimed");
            ClaimLootBoxRewards(txResponse);
        }
        else
        {
            Debug.Log("Rewards cannot be claimed");
        }
    }

    private void ClaimLootBoxRewards(TransactionReceipt response)
    {
        Debug.Log($"Claiming rewards from hash: {response.TransactionHash}");
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
    }

    private async Task<BigInteger> CalculateOpenPrice(BigInteger[] lootIds, BigInteger[] lootAmounts)
    {
        var args = new object[]
        {
            lootIds,
            lootAmounts
        };
        //var gasLimit = await Web3Unity.Instance.GetGasLimit(lootboxUsageSample.ABI, lootBoxContract, "open", args);
        var gasLimit = BigInteger.Parse("100000");
        var gasPriceInWei = await Web3Unity.Instance.GetGasPrice();
        //Debug.Log($"GasLimit: {gasLimit}");
        Debug.Log($"GasPrice: {gasPriceInWei}");
        var units = BigInteger.Parse("1");
        var openPrice = await lootboxUsageSample.CalculateOpenPrice(gasLimit, gasPriceInWei, units);
        Debug.Log($"Open price: {openPrice}");
        return openPrice;
    }

    private async void OpenLootBox(BigInteger[] lootIds, BigInteger[] lootAmounts)
    {
        var gasPriceInWei = await Web3Unity.Instance.GetGasPrice();
        var gasLimit = BigInteger.Parse("100000");
        var openPrice = await CalculateOpenPrice(lootIds, lootAmounts);
        Debug.Log($"Open price: {openPrice}");
        var response = await lootboxUsageSample.OpenWithReceipt(gasLimit, lootIds, lootAmounts, new TransactionRequest { Value = new HexBigInteger(openPrice) });
        //CanClaimRewards(response);
        ClaimLootBoxRewards(response);
    }

    private void PostOnSocialMedia()
    {
        string message = "I just opened a lootBox!";
        string url = "https://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(message);
        Application.OpenURL(url);
    }
}
