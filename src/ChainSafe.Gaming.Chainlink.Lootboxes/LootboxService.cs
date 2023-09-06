using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Debug;
using ChainSafe.GamingWeb3;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    public class LootboxServiceConfig
    {
        public string? ContractAddress { get; set; }

        public string? ContractAbi { get; set; }
    }

    public class LootboxService : ILootboxService, ILifecycleParticipant
    {
        public const int GasPerUnit = 100000;

        private IContractBuilder contractBuilder;
        private LootboxServiceConfig config;
        private Contract contract;
        private ISigner signer;
        private IRpcProvider rpcProvider;

        public LootboxService(LootboxServiceConfig config, IContractBuilder contractBuilder, IRpcProvider rpcProvider)
        {
            this.rpcProvider = rpcProvider;
            this.config = config;
            this.contractBuilder = contractBuilder;
        }

        public LootboxService(LootboxServiceConfig config, IContractBuilder contractBuilder, IRpcProvider rpcProvider, ISigner signer)
        {
            this.signer = signer;
            this.rpcProvider = rpcProvider;
            this.config = config;
            this.contractBuilder = contractBuilder;
        }

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            var contractAbi = config.ContractAbi.AssertNotNull(nameof(config.ContractAbi));
            var contractAddress = config.ContractAddress.AssertNotNull(nameof(config.ContractAddress));

            contract = contractBuilder.Build(contractAbi, contractAddress);
        }

        ValueTask ILifecycleParticipant.WillStopAsync() => new ValueTask(Task.CompletedTask);

        public async Task<List<uint>> GetLootboxTypes()
        {
            var response = await contract.Call("getLootboxTypes");
            var bigIntTypes = (List<BigInteger>)response[0];

            if (bigIntTypes.Any(v => v > int.MaxValue))
            {
                throw new Web3Exception(
                    "Internal Error. Lootbox type is greater than int.MaxValue. Please contact ChainSafe to resolve this problem.");
            }

            var types = bigIntTypes.Select(bigInt => (uint)bigInt).ToList();

            return types;
        }

        public async Task<uint> BalanceOf(uint lootboxType)
        {
            if (signer is null)
            {
                throw new Web3Exception($"No {nameof(ISigner)} was registered. Can't get current user's address.");
            }

            var playerAddress = await signer.GetAddress();

            return await BalanceOf(playerAddress, lootboxType);
        }

        public async Task<uint> BalanceOf(string account, uint lootboxType)
        {
            var response = await contract.Call(
                "balanceOf",
                new object[] { account, lootboxType });
            var bigIntBalance = (BigInteger)response[0];

            if (bigIntBalance > int.MaxValue)
            {
                throw new Web3Exception(
                    "Internal Error. Balance is greater than int.MaxValue. Please contact ChainSafe to resolve this problem.");
            }

            var balance = (uint)bigIntBalance;

            return balance;
        }

        public async Task<BigInteger> CalculateOpenPrice(uint lootboxType, uint lootboxCount)
        {
            var rewardCount = lootboxType * lootboxCount;
            var rawGasPrice = (await rpcProvider.GetGasPrice()).AssertNotNull("gasPrice").Value;
            var safeGasPrice = rawGasPrice + BigInteger.Divide(rawGasPrice, new BigInteger(10)); // 110%

            var response = await contract.Call(
                "calculateOpenPrice",
                new object[] { GasPerUnit * rewardCount, safeGasPrice, rewardCount, });
            var openPrice = (BigInteger)response[0];

            return openPrice;
        }

        public async Task OpenLootbox(uint lootboxType, uint lootboxCount = 1)
        {
            var rewardCount = lootboxType * lootboxCount;
            var openPrice = await CalculateOpenPrice(lootboxCount, lootboxCount);

            await contract.Send(
                "open",
                new object[] { GasPerUnit * rewardCount, new[] { lootboxType }, new[] { lootboxCount } },
                new TransactionRequest { Value = new HexBigInteger(openPrice) });
        }

        public async Task<bool> CanClaimRewards()
        {
            if (signer is null)
            {
                throw new Web3Exception($"No {nameof(ISigner)} was registered. Can't get current user's address.");
            }

            var playerAddress = await signer.GetAddress();

            return await CanClaimRewards(playerAddress);
        }

        public async Task<bool> CanClaimRewards(string account)
        {
            var response = await contract.Call(
                "canClaimRewards",
                new object[] { account });
            var canClaimRewards = (bool)response[0];

            return canClaimRewards;
        }

        public async Task ClaimRewards()
        {
            if (signer is null)
            {
                throw new Web3Exception($"No {nameof(ISigner)} was registered. Can't get current user's address.");
            }

            var playerAddress = await signer.GetAddress();

            await ClaimRewards(playerAddress);
        }

        public async Task ClaimRewards(string account)
        {
            var (_, receipt) = await contract.SendWithReceipt("claimRewards", new object[] { account });
            receipt.
        }
    }
}