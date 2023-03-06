using System;
using System.Numerics;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Utils;

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
        
        [Test]
        public void GetBalanceTest()
        {
            var balance = _provider.GetBalance("0xaBed4239E4855E120fDA34aDBEABDd2911626BA1").Result;
            var balanceFormatted = Units.FormatEther(balance);
            Assert.Greater(balanceFormatted, "0");
        }
        
        [Test]
        public void GetCodeTest()
        {
            var code = _provider.GetCode("0xaBed4239E4855E120fDA34aDBEABDd2911626BA1").Result;
            Assert.AreEqual("0x", code);
        }
        
        [Test]
        public void GetStorageAtTest()
        {
            var slot = _provider.GetStorageAt("0xaBed4239E4855E120fDA34aDBEABDd2911626BA1", new BigInteger(0)).Result;
            Console.WriteLine($"Contract slot 0: {slot}");
        }
        
        [Test]
        public void GetLastBlockTest()
        {
            var latestBlock = _provider.GetBlock().Result;
            Assert.AreEqual("0x0000000000000000", latestBlock.Nonce );
            Assert.AreEqual("30000000", latestBlock.GasLimit.ToString() );
            Assert.True(latestBlock.BlockHash.StartsWith("0x"));
            Assert.True(latestBlock.ParentHash.StartsWith("0x"));
            Assert.IsNotEmpty(latestBlock.Number.ToString());
            Assert.IsNotEmpty(latestBlock.Timestamp.ToString());
        }
        
        [Test]
        public void GetBlockNumberTest()
        {
            var currentBlockNumber = _provider.GetBlockNumber().Result;
            Assert.Greater(currentBlockNumber.ToString(), "0");
        }
    }
}