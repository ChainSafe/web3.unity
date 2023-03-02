using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Tests
{
    [TestFixture]
    public class ChainSafeRpcTests
    {
        private JsonRpcProvider _provider;
        [SetUp]
        public void SetUp()
        {
            _provider = new JsonRpcProvider("https://goerli.infura.io/v3/904006115c764661965dc0909e5ed473");
        }
        
        [Test]
        public void GetNetworkTest()
        {
            var network = _provider.GetNetwork().Result;
            Assert.AreEqual(network.Name, "Goerli");
            Assert.AreEqual(network.ChainId, 5);
        }
    }
}