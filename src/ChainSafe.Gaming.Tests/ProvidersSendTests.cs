using System.Diagnostics;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Tests.Node;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;
using static ChainSafe.Gaming.Tests.Web3Util;

namespace ChainSafe.Gaming.Tests
{
    using static RpcProviderExtensions;
    using Web3 = ChainSafe.Gaming.Web3.Web3;

    [TestFixture]
    public class ProvidersSendTests
    {
        private Web3 firstAccount;
        private Web3 secondAccount;
        private Process node;

        [OneTimeSetUp]
        public void SetUp()
        {
            node = Emulator.CreateInstance();

            var firstAccountTask = CreateWeb3(0).AsTask();
            firstAccountTask.Wait();
            firstAccount = firstAccountTask.Result;

            var secondAccountTask = CreateWeb3(1).AsTask();
            secondAccountTask.Wait();
            secondAccount = secondAccountTask.Result;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            node?.Kill();
        }

        [Test]
        public void SendTransactionTest()
        {
            var fromAddress = firstAccount.Signer.GetAddress().Result;
            var fromInitialBalance = firstAccount.RpcProvider.GetBalance(fromAddress).Result.Value;

            var toAddress = secondAccount.Signer.GetAddress().Result;
            var toInitialBalance = firstAccount.RpcProvider.GetBalance(toAddress).Result.Value;

            var amount = new HexBigInteger(1000000);
            var tx = firstAccount.TransactionExecutor.SendTransaction(new TransactionRequest
            {
                To = toAddress,
                Value = amount,
            });
            tx.Wait();

            Assert.True(tx.Result.Hash.StartsWith("0x"));

            var txReceipt = firstAccount.RpcProvider.GetTransactionReceipt(tx.Result.Hash);
            txReceipt.Wait();

            Assert.AreEqual(txReceipt.Result.Confirmations, 1);
            Assert.AreEqual(toInitialBalance + amount.Value, firstAccount.RpcProvider.GetBalance(toAddress).Result.Value);
            Assert.AreEqual(
                fromInitialBalance - amount.Value - (txReceipt.Result.CumulativeGasUsed.Value * txReceipt.Result.EffectiveGasPrice.Value),
                firstAccount.RpcProvider.GetBalance(fromAddress).Result.Value);
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
                var txHash = await firstAccount.TransactionExecutor.SendTransaction(transaction);
            });
            Assert.That(ex.Message.Contains("eth_sendTransaction"));
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
                var txHash = await firstAccount.TransactionExecutor.SendTransaction(transaction);
            });

            Assert.That(ex != null && ex.Message.Contains("eth_sendTransaction"));
            var result = ex.InnerException != null && ex.InnerException.Message.Contains("gas too low");
            Assert.That(result);
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

            Assert.ThrowsAsync<Web3Exception>(() => firstAccount.TransactionExecutor.SendTransaction(transaction));
        }
    }
}