using System.Diagnostics;
using ChainSafe.GamingSDK.EVM.Tests.Node;
using ChainSafe.GamingWeb3;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using static ChainSafe.GamingSDK.EVM.Tests.Web3Util;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    using Web3 = ChainSafe.GamingWeb3.Web3;

    [TestFixture]
    public class ProvidersSendTests
    {
        private Web3 web3;
        private Web3 secondAccount;
        private Process node;

        [OneTimeSetUp]
        public async void SetUp()
        {
            const uint port = 8546;
            node = Emulator.CreateInstance(port);
            web3 = await CreateWeb3(0, port);
            secondAccount = await CreateWeb3(1, port);
        }

        [TearDown]
        public void Cleanup()
        {
            node?.Kill();
        }

        [Test]
        public void SendTransactionTest()
        {
            var fromAddress = web3.Signer.GetAddress().Result;
            var fromInitialBalance = web3.RpcProvider.GetBalance(fromAddress).Result.Value;

            var toAddress = secondAccount.Signer.GetAddress().Result;
            var toInitialBalance = web3.RpcProvider.GetBalance(toAddress).Result.Value;

            var amount = new HexBigInteger(1000000);
            var tx = web3.TransactionExecutor.SendTransaction(new TransactionRequest
            {
                To = toAddress,
                Value = amount,
            });
            tx.Wait();

            Assert.True(tx.Result.Hash.StartsWith("0x"));

            var txReceipt = web3.RpcProvider.GetTransactionReceipt(tx.Result.Hash);
            txReceipt.Wait();

            Assert.AreEqual(txReceipt.Result.Confirmations, 1);
            Assert.AreEqual(toInitialBalance + amount.Value, web3.RpcProvider.GetBalance(toAddress).Result.Value);
            Assert.AreEqual(
                fromInitialBalance - amount.Value - (txReceipt.Result.CumulativeGasUsed.Value * txReceipt.Result.EffectiveGasPrice.Value),
                web3.RpcProvider.GetBalance(fromAddress).Result.Value);
        }

        [Test]
        public void SendTransactionWithInvalidAddress()
        {
            const string to = "not_a_valid_address";
            var amount = new HexBigInteger(1000000);
            var transaction = new TransactionRequest
            {
                To = to,
                Value = amount,
                GasLimit = new HexBigInteger("10000"),
                GasPrice = new HexBigInteger("100000000"),
            };

            var ex = Assert.ThrowsAsync<Web3Exception>(async () =>
            {
                var txHash = await web3.TransactionExecutor.SendTransaction(transaction);
            });
            Assert.That(ex.Message.Contains("eth_sendTransaction"));
            Assert.That(ex.Message.Contains("-32700"));
        }

        [Test]
        public void SendTransactionWithLowGasLimit()
        {
            const string to = "0x1234567890123456789012345678901234567890";
            var amount = new HexBigInteger(1000000);
            var gasLimit = new HexBigInteger(1);
            var transaction = new TransactionRequest
            {
                To = to,
                Value = amount,
                GasLimit = gasLimit,
            };

            var ex = Assert.ThrowsAsync<Web3Exception>(async () =>
            {
                var txHash = await web3.TransactionExecutor.SendTransaction(transaction);
            });
            Assert.That(ex.Message.Contains("eth_sendTransaction"));
            Assert.That(ex.Message.Contains("-32000"));
        }

        [Test]
        public void SendTransactionWithLowGasPrice()
        {
            const string to = "0x1234567890123456789012345678901234567890";
            var amount = new HexBigInteger(1000000);
            var gasPrice = new HexBigInteger(1);
            var transaction = new TransactionRequest
            {
                To = to,
                Value = amount,
                GasPrice = gasPrice,
            };

            Assert.ThrowsAsync<Web3Exception>(() => web3.TransactionExecutor.SendTransaction(transaction));
        }
    }
}