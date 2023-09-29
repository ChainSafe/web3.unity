using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    public class LootboxService : ILootboxService, ILifecycleParticipant
    {
        public const int GasPerUnit = 100000;

        private readonly IContractBuilder contractBuilder;
        private readonly LootboxServiceConfig config;
        private readonly ISigner signer;
        private readonly IRpcProvider rpcProvider;

        private Contract contract;
        private Dictionary<string, RewardType> rewardTypeByTokenAddress;

        public LootboxService(
            LootboxServiceConfig config,
            IContractBuilder contractBuilder,
            IRpcProvider rpcProvider)
        {
            this.rpcProvider = rpcProvider;
            this.config = config;
            this.contractBuilder = contractBuilder;
        }

        public LootboxService(
            LootboxServiceConfig config,
            IContractBuilder contractBuilder,
            IRpcProvider rpcProvider,
            ISigner signer)
            : this(config, contractBuilder, rpcProvider)
        {
            this.signer = signer;
        }

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            var contractAbi = this.config.ContractAbi.AssertNotNull(nameof(this.config.ContractAbi));
            var contractAddress = this.config.ContractAddress.AssertNotNull(nameof(this.config.ContractAddress));

            // todo check if contract is correct
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

        ValueTask ILifecycleParticipant.WillStopAsync() => new ValueTask(Task.CompletedTask);

        public async Task<List<uint>> GetLootboxTypes()
        {
            var response = await this.contract.Call("getLootboxTypes");
            var bigIntTypes = (List<BigInteger>)response[0];

            if (bigIntTypes.Any(v => v > int.MaxValue))
            {
                throw new Web3Exception(
                    "Internal Error. Lootbox type is greater than int.MaxValue.");
            }

            var types = bigIntTypes.Select(bigInt => (uint)bigInt).ToList();

            return types;
        }

        public async Task<uint> BalanceOf(uint lootboxType)
        {
            var playerAddress = await this.GetCurrentPlayerAddress();

            return await this.BalanceOf(playerAddress, lootboxType);
        }

        public async Task<uint> BalanceOf(string account, uint lootboxType)
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

            var balance = (uint)bigIntBalance;

            return balance;
        }

        public async Task<BigInteger> CalculateOpenPrice(uint lootboxType, uint lootboxCount)
        {
            var rewardCount = lootboxType * lootboxCount;
            var rawGasPrice = (await this.rpcProvider.GetGasPrice()).AssertNotNull("gasPrice").Value;
            var safeGasPrice = (rawGasPrice * 2) + BigInteger.Divide(rawGasPrice, new BigInteger(2)); // 300%

            var response = await this.contract.Call(
                "calculateOpenPrice",
                new object[] { 50000 + (GasPerUnit * rewardCount), safeGasPrice, rewardCount, });
            var openPrice = (BigInteger)response[0];

            return openPrice;
        }

        public async Task<bool> IsOpeningLootbox()
        {
            var playerAddress = await this.GetCurrentPlayerAddress();
            var response = await this.contract.Call("openerRequests", new object[] { playerAddress });
            var requests = (BigInteger)response[0];
            return requests > 0;
        }

        public async Task<uint> OpeningLootboxType()
        {
            throw new NotImplementedException();
        }

        public async Task OpenLootbox(uint lootboxType, uint lootboxCount = 1)
        {
            var rewardCount = lootboxType * lootboxCount;
            var openPrice = await this.CalculateOpenPrice(lootboxCount, lootboxCount);

            await this.contract.Send(
                "open",
                new object[] { 50000 + (GasPerUnit * rewardCount), new[] { lootboxType }, new[] { lootboxCount } },
                new TransactionRequest { Value = new HexBigInteger(openPrice) });
        }

        public async Task<bool> CanClaimRewards()
        {
            var playerAddress = await this.GetCurrentPlayerAddress();

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

        public async Task<LootboxRewards> ClaimRewards()
        {
            var playerAddress = await this.GetCurrentPlayerAddress();

            return await this.ClaimRewards(playerAddress);
        }

        public async Task<LootboxRewards> ClaimRewards(string account)
        {
            var (_, receipt) = await this.contract.SendWithReceipt("claimRewards", new object[] { account });
            var logs = receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
            var eventAbi = EventExtensions.GetEventABI<RewardsClaimedEvent>();
            var eventLogs = logs
                .Select(log => eventAbi.DecodeEvent<RewardsClaimedEvent>(log))
                .Where(l => l != null);

            if (!eventLogs.Any())
            {
                throw new Web3Exception("No \"RewardsClaimed\" events were found in log's receipt.");
            }

            return ExtractRewards(eventLogs);

            LootboxRewards ExtractRewards(IEnumerable<EventLog<RewardsClaimedEvent>> eventLogs)
            {
                var rewards = LootboxRewards.Empty;

                foreach (var eventLog in eventLogs)
                {
                    var eventData = eventLog.Event;
                    var rewardType = this.rewardTypeByTokenAddress[eventData.TokenAddress];

                    switch (rewardType)
                    {
                        case RewardType.Erc20:
                            rewards.Erc20Rewards.Add(new Erc20Reward
                            {
                                ContractAddress = eventData.TokenAddress,
                                AmountRaw = eventData.Amount,
                            });
                            break;
                        case RewardType.Erc721:
                            rewards.Erc721Rewards.Add(new Erc721Reward
                            {
                                ContractAddress = eventData.TokenAddress,
                                TokenId = eventData.TokenId,
                            });
                            break;
                        case RewardType.Erc1155:
                            rewards.Erc1155Rewards.Add(new Erc1155Reward
                            {
                                ContractAddress = eventData.TokenAddress,
                                TokenId = eventData.TokenId,
                                Amount = eventData.Amount,
                            });
                            break;
                        case RewardType.Erc1155Nft:
                            rewards.Erc1155NftRewards.Add(new Erc1155NftReward
                            {
                                ContractAddress = eventData.TokenAddress,
                                TokenId = eventData.TokenId,
                            });
                            break;
                        case RewardType.Unset:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return rewards;
            }
        }

        private async Task<string> GetCurrentPlayerAddress()
        {
            if (this.signer is null)
            {
                throw new Web3Exception($"No {nameof(ISigner)} was registered. Can't get current user's address.");
            }

            return await this.signer.GetAddress();
        }
    }
}