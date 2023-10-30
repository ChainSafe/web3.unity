#if Lootboxes
using System.IO;
using System.Threading.Tasks;
using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.Tests;
using NUnit.Framework;

namespace ChainSafe.Gaming.Evm.Tests
{
    /// <summary>
    /// Test fixture for testing the Chainlink Lootbox functionality.
    /// </summary>
    [TestFixture]
    public class ChainlinkLootboxTests // todo: not sure if should assert default network values
    {
        private Web3.Web3 web3;

        // todo add automatic emulator boot up
        /// <summary>
        /// Sets up the Web3 instance and other necessary configurations for testing Chainlink Lootboxes.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            var lootBoxesConfig = new LootboxServiceConfig
            {
                ContractAddress = "0x46E334e90454aDDF311Cd75D4Ae19e2fA06285Ff",
                ContractAbi = File.ReadAllText("Resources/LootboxInterface.abi.json"),
            };

            var web3BuildTask = Web3Util.CreateWeb3(
                    accountIndex: 2,
                    configureDelegate: services =>
                    {
                        services.UseChainlinkLootboxService(lootBoxesConfig);
                    })
                .AsTask();

            web3BuildTask.Wait();
            web3 = web3BuildTask.Result;
        }

        /// <summary>
        /// Tears down the test fixture after all tests are executed.
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// Tests the retrieval of lootbox types from the Chainlink Lootbox service.
        /// </summary>
        [Test]
        public async Task GetLootboxTypesTest()
        {
            var lootboxTypes = await web3.Chainlink().Lootboxes().GetLootboxTypes();

            Assert.IsNotNull(lootboxTypes);
            Assert.IsNotEmpty(lootboxTypes);

            Assert.AreEqual(5, lootboxTypes.Count);
            Assert.AreEqual(1, lootboxTypes[0]);
        }

        [Test]
        public async Task BalanceOfTest()
        {
            var lootboxesBalance = await web3.Chainlink().Lootboxes().BalanceOf(1);

            Assert.AreEqual(5, lootboxesBalance);
        }

        [Test]
        public async Task CalculateOpenPriceTest()
        {
            var openPrice = await web3.Chainlink().Lootboxes().CalculateOpenPrice(1, 3);

            Assert.False(openPrice.IsZero);
        }

        // [Test]
        // public async Task OpenLootBoxClaimRewardsTest()
        // {
        //     // Act
        //     await web3.Chainlink().Lootboxes().OpenLootbox(1);
        //
        //     // todo call "npm run hardhat -- fulfill" after this one automatically
        //     await WaitTillCanClaimRewards();
        //     var rewards = await web3.Chainlink().Lootboxes().ClaimRewards();
        //     var hasRewards = rewards.Erc20Rewards.Any() || rewards.Erc721Rewards.Any() ||
        //                      rewards.Erc1155Rewards.Any() || rewards.Erc1155NftRewards.Any();
        //     Assert.IsTrue(hasRewards);
        //
        //     async Task WaitTillCanClaimRewards()
        //     {
        //         var pollDelay = TimeSpan.FromSeconds(1);
        //         var pollTimeOut = TimeSpan.FromMinutes(10);
        //         var pollStartTime = DateTime.Now;
        //         while (DateTime.Now - pollStartTime < pollTimeOut)
        //         {
        //             await Task.Delay(pollDelay);
        //
        //             var canClaimRewards = await web3.Chainlink().Lootboxes().CanClaimRewards();
        //
        //             if (canClaimRewards)
        //             {
        //                 return;
        //             }
        //         }
        //
        //         throw new Exception($"Poll timed out when waiting for " +
        //                             $"{nameof(ILootboxService.CanClaimRewards)} to become true.");
        //     }
        // }

        /// <summary>
        /// Test method to verify that 'OpenInProgress' is true when a lootbox is being opened.
        /// </summary>
        [Test]
        public async Task OpenInProgressIsTrueWhenOpeningTest()
        {
            await web3.Chainlink().Lootboxes().OpenLootbox(1);
            var openInProgress = await web3.Chainlink().Lootboxes().IsOpeningLootbox();

            Assert.IsTrue(openInProgress);
        }

        /// <summary>
        /// Test method to verify that 'OpenInProgress' is false when no lootbox is being opened.
        /// </summary>
        [Test]
        public async Task OpenInProgressIsFalseWhenNotOpeningTest()
        {
            var openInProgress = await web3.Chainlink().Lootboxes().IsOpeningLootbox();

            Assert.IsFalse(openInProgress);
        }

        // [Test]
        // public async Task ThrowsWhenTryOpenBeforeClaim()
        // {
        //     await web3.Chainlink().Lootboxes().OpenLootbox(1);
        //     Assert.ThrowsAsync<Web3Exception>(() => web3.Chainlink().Lootboxes().OpenLootbox(2));
        // }

        // [Test]
        // public async Task OpenLootboxTest()
        // {
        //     await web3.Chainlink().Lootboxes().OpenLootbox(1);
        // }
        //
        // [Test]
        // public async Task CanClaimRewardsTest()
        // {
        //     await web3.Chainlink().Lootboxes().CanClaimRewards();
        // }
        //
        // [Test]
        // public async Task ClaimRewardsTest()
        // {
        //     await web3.Chainlink().Lootboxes().ClaimRewards();
        // }
    }
}
#endif