using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using NUnit.Framework;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    [TestFixture]
    public class ChainlinkLootboxTests // todo: not sure if should assert default network values
    {
        private GamingWeb3.Web3 web3;

        // todo add emulator boot up
        [OneTimeSetUp]
        public void Setup()
        {
            var lootBoxesConfig = new LootboxServiceConfig
            {
                ContractAddress = "0xfF1Ca68Ac8A7D0267507f756Be8AE813cEEA2E78",
                ContractAbi = File.ReadAllText("Resources/Lootbox.abi.json"),
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

        [Test]
        public async Task GetLootboxTypesTest()
        {
            var lootboxTypes = await web3.Chainlink().Lootboxes().GetLootboxTypes();

            Assert.IsNotNull(lootboxTypes);
            Assert.IsNotEmpty(lootboxTypes);

            Assert.AreEqual(1, lootboxTypes.Count);
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
            var expected = BigInteger.Parse("2079463128399999");

            Assert.AreEqual(expected, openPrice);
        }

        [Test]
        public async Task OpenLootboxTest()
        {
            await web3.Chainlink().Lootboxes().OpenLootbox(1);
        }

        [Test]
        public async Task CanClaimRewardsTest()
        {
            await web3.Chainlink().Lootboxes().CanClaimRewards();
        }

        [Test]
        public async Task ClaimRewardsTest()
        {
            await web3.Chainlink().Lootboxes().ClaimRewards();
        }
    }
}