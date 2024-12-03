﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxService : ILootboxService, ILifecycleParticipant
    {
        public const int GasPerUnit = 300000;

        private readonly IContractBuilder contractBuilder;
        private readonly LootboxServiceConfig config;
        private readonly ISigner signer;
        private readonly IRpcProvider rpcProvider;
        private readonly IAnalyticsClient analyticsClient;
        private readonly IEventManager eventManager;
        private readonly ILogWriter logWriter;

        private Contract contract;
        private Dictionary<string, RewardType> rewardTypeByTokenAddress;

        public LootboxService(
            LootboxServiceConfig config,
            IContractBuilder contractBuilder,
            IRpcProvider rpcProvider,
            IAnalyticsClient analyticsClient,
            IEventManager eventManager,
            ILogWriter logWriter)
        {
            this.rpcProvider = rpcProvider;
            this.analyticsClient = analyticsClient;
            this.eventManager = eventManager;
            this.logWriter = logWriter;
            this.config = config;
            this.contractBuilder = contractBuilder;
        }

        public LootboxService(
            LootboxServiceConfig config,
            IContractBuilder contractBuilder,
            IRpcProvider rpcProvider,
            ISigner signer,
            IAnalyticsClient analyticsClient,
            IEventManager eventManager,
            ILogWriter logWriter)
            : this(config, contractBuilder, rpcProvider, analyticsClient, eventManager, logWriter)
        {
            this.signer = signer;
        }

        public event Action<LootboxRewards>? OnRewardsClaimed;

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            var contractAbi = this.config.ContractAbi.AssertNotNull(nameof(this.config.ContractAbi));
            var contractAddress = this.config.LootboxAddress.AssertNotNull(nameof(this.config.LootboxAddress));
            await eventManager.Subscribe<RewardsClaimedEvent>(ExtractRewards, contractAddress);

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = "Lootboxes Initialized",
                PackageName = "io.chainsafe.web3-unity.lootboxes",
            });

            this.contract = this.contractBuilder.Build(contractAbi, contractAddress);

            this.rewardTypeByTokenAddress = await MapTokenAddressToRewardType();

            async Task<Dictionary<string, RewardType>> MapTokenAddressToRewardType()
            {
                var tokenAddresses = (List<string>)(await this.contract.Call("getAllowedTokens"))[0];

                // Array of token reward types in the same order as getAllowedTokens()
                var rewardTypes = ((List<BigInteger>)(await this.contract.Call("getAllowedTokenTypes"))[0])
                    .Select(bi => (int)bi)
                    .Cast<RewardType>()
                    .ToList();

                if (tokenAddresses.Count != rewardTypes.Count)
                {
                    throw new Web3Exception(
                        "Element count mismatch between \"getAllowedTokens\" and \"getAllowedTokenTypes\"");
                }

                return Enumerable.Range(0, tokenAddresses.Count)
                    .ToDictionary(index => tokenAddresses[index], index => rewardTypes[index]);
            }
        }

        async ValueTask ILifecycleParticipant.WillStopAsync()
        {
            var contractAddress = this.config.LootboxAddress.AssertNotNull(nameof(this.config.LootboxAddress));
            await eventManager.Unsubscribe<RewardsClaimedEvent>(ExtractRewards, contractAddress);
        }

        public async Task<List<int>> GetLootboxTypes()
        {
            var response = await this.contract.Call("getLootboxTypes");
            var bigIntTypes = (List<BigInteger>)response[0];

            if (bigIntTypes.Any(v => v > int.MaxValue))
            {
                throw new Web3Exception(
                    "Internal Error. Lootbox type is greater than int.MaxValue.");
            }

            var types = bigIntTypes.Select(bigInt => (int)bigInt).ToList();

            return types;
        }

        public async Task<int> BalanceOf(int lootboxType)
        {
            var playerAddress = this.GetCurrentPlayerAddress();

            return await this.BalanceOf(playerAddress, lootboxType);
        }

        public async Task<int> BalanceOf(string account, int lootboxType)
        {
            var response = await this.contract.Call(
                "balanceOf",
                new object[] { account, lootboxType });
            var bigIntBalance = (BigInteger)response[0];

            if (bigIntBalance > int.MaxValue)
            {
                throw new Web3Exception(
                    "Internal Error. Balance is greater than int.MaxValue.");
            }

            var balance = (int)bigIntBalance;

            return balance;
        }

        public async Task<BigInteger> CalculateOpenPrice(int lootboxType, int lootboxCount)
        {
            var rewardCount = lootboxType * lootboxCount;
            var rawGasPrice = (await this.rpcProvider.GetGasPrice()).AssertNotNull("gasPrice").Value;
            var safeGasPrice = (rawGasPrice * 2) + BigInteger.Divide(rawGasPrice, new BigInteger(2)); // 300%

            var response = await this.contract.Call(
                "calculateOpenPrice",
                new object[] { 100000 + (GasPerUnit * rewardCount), safeGasPrice, rewardCount, });
            var openPrice = (BigInteger)response[0];

            return openPrice;
        }

        public async Task<bool> IsOpeningLootbox()
        {
            var playerAddress = this.GetCurrentPlayerAddress();
            var response = await this.contract.Call("openerRequests", new object[] { playerAddress });
            var requests = (BigInteger)response[0];
            return requests > 0;
        }

        public async Task<int> OpeningLootboxType()
        {
            var playerAddress = this.GetCurrentPlayerAddress();

            // This response is actually very different from all the others since it returns several components
            var response =
                (List<ParameterOutput>)(await this.contract.Call(
                    "getOpenerRequestDetails",
                    new object[] { playerAddress }))[0];
            var address = (string)response[0].Result;
            var unitsToGet = (BigInteger)response[1].Result;
            var lootboxType = ((List<BigInteger>)response[2].Result)[0];
            if (!string.IsNullOrEmpty(address) && unitsToGet == 0)
            {
                // we can early return here, but, it's not necessary since unitstoget will be 0 regardless and this
                // call will fulfill every request that's been missing.
                await this.contract.Send(
                    "recoverBoxes",
                    new object[] { playerAddress });
            }

            if (unitsToGet > uint.MaxValue)
            {
                throw new Web3Exception("Internal Error. Units to get is greater than int.MaxValue.");
            }

            return (int)lootboxType;
        }

        public async Task OpenLootbox(int lootboxType, int lootboxCount = 1)
        {
            var rewardCount = lootboxType * lootboxCount;
            var openPrice = await this.CalculateOpenPrice(lootboxCount, lootboxCount);

            await this.contract.Send(
                "open",
                new object[] { 100000 + (GasPerUnit * rewardCount), new[] { lootboxType }, new[] { lootboxCount } },
                new TransactionRequest { Value = new HexBigInteger(openPrice) });
        }

        public async Task RecoverLootboxes()
        {
            var playerAddress = this.GetCurrentPlayerAddress();

            await this.contract.Send(
                "recoverBoxes",
                new object[] { playerAddress });
        }

        public async Task<LootboxItemList> GetInventory()
        {
            var result = await this.contract.Call("getInventory");
            var jsonResult = JsonConvert.DeserializeObject<LootboxItemList>(JsonConvert.SerializeObject(result));
            return jsonResult;
        }

        public async Task<bool> CanClaimRewards()
        {
            var playerAddress = this.GetCurrentPlayerAddress();

            return await this.CanClaimRewards(playerAddress);
        }

        public async Task<bool> CanClaimRewards(string account)
        {
            var response = await this.contract.Call(
                "canClaimRewards",
                new object[] { account });
            var canClaimRewards = (bool)response[0];

            return canClaimRewards;
        }

        public async Task ClaimRewards()
        {
            await this.contract.Send("claimRewards", new object[] { signer.PublicAddress });
        }

        public async Task ClaimRewards(string account)
        {
            await this.contract.Send("claimRewards", new object[] { account });
        }

        public async Task<BigInteger> GetPrice()
        {
            var response = await this.contract.Call("getPrice", new object[] { });
            return BigInteger.Parse(response[0].ToString());
        }

        public async Task SetPrice(BigInteger price)
        {
            await this.contract.Send("setPrice", new object[] { price });
        }

        public async Task Buy(int amount, BigInteger maxPrice)
        {
            var pricePerLootbox = await GetPrice();
            var priceToSend = pricePerLootbox * amount;
            await this.contract.Send("buy", new object[] { amount, maxPrice }, new TransactionRequest { Value = new HexBigInteger(priceToSend) });
        }

        private void ExtractRewards(RewardsClaimedEvent rewardsClaimedEvent)
        {
            var rewards = LootboxRewards.Empty;
            var rewardType = this.rewardTypeByTokenAddress[rewardsClaimedEvent.TokenAddress];
            switch (rewardType)
            {
                case RewardType.Erc20:
                    rewards.Erc20Rewards.Add(new Erc20Reward
                    {
                        ContractAddress = rewardsClaimedEvent.TokenAddress,
                        AmountRaw = rewardsClaimedEvent.Amount,
                    });
                    break;
                case RewardType.Erc721:
                    rewards.Erc721Rewards.Add(new Erc721Reward
                    {
                        ContractAddress = rewardsClaimedEvent.TokenAddress,
                        TokenId = rewardsClaimedEvent.TokenId,
                    });
                    break;
                case RewardType.Erc1155:
                    rewards.Erc1155Rewards.Add(new Erc1155Reward
                    {
                        ContractAddress = rewardsClaimedEvent.TokenAddress,
                        TokenId = rewardsClaimedEvent.TokenId,
                        Amount = rewardsClaimedEvent.Amount,
                    });
                    break;
                case RewardType.Erc1155Nft:
                    rewards.Erc1155NftRewards.Add(new Erc1155NftReward
                    {
                        ContractAddress = rewardsClaimedEvent.TokenAddress,
                        TokenId = rewardsClaimedEvent.TokenId,
                    });
                    break;
                case RewardType.Unset:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnRewardsClaimed?.Invoke(rewards);
        }

        private string GetCurrentPlayerAddress()
        {
            if (this.signer is null)
            {
                throw new Web3Exception($"No {nameof(ISigner)} was registered. Can't get current user's address.");
            }

            return signer.PublicAddress;
        }
    }
}