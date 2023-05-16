using System;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    [TestFixture]
    public class ProvidersSendTests
    {
        private JsonRpcProvider ganacheProvider;

        [OneTimeSetUp]
        public void SetUp()
        {
            ganacheProvider = ProviderMigration.NewJsonRpcProvider("http://127.0.0.1:7545");
        }

        [Test]
        public void SendTransactionTest()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);

            var from = ganacheProvider.GetSigner();
            var fromInitialBalance = from.GetBalance().Result.Value;

            var to = ganacheProvider.GetSigner(1);
            var toInitialBalance = to.GetBalance().Result.Value;

            var amount = new HexBigInteger(1000000);
            var tx = from.SendTransaction(new TransactionRequest
            {
                To = to.GetAddress().Result,
                Value = amount,
            }).Result;
            Assert.True(tx.Hash.StartsWith("0x"));

            var txReceipt = tx.Wait().Result;

            Assert.AreEqual(txReceipt.Confirmations, 1);
            Assert.AreEqual(toInitialBalance + amount.Value, to.GetBalance().Result.Value);
            Assert.AreEqual(fromInitialBalance - amount.Value - (txReceipt.CumulativeGasUsed.Value * txReceipt.EffectiveGasPrice.Value), from.GetBalance().Result.Value);
        }

        [Test]
        public void SendTransactionWithInvalidAddress()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);
            var from = ganacheProvider.GetSigner();
            const string to = "not_a_valid_address";
            var amount = new HexBigInteger(1000000);
            var transaction = new TransactionRequest
            {
                To = to,
                Value = amount,
                GasLimit = new HexBigInteger("10000"),
                GasPrice = new HexBigInteger("100000000"),
            };

            var ex = Assert.ThrowsAsync<Exception>(async () =>
            {
                var txHash = await from.SendTransaction(transaction);
            });
            Assert.AreEqual($"eth_sendTransaction: -32700 Cannot wrap string value \"{to}\" as a json-rpc type; strings must be prefixed with \"0x\". ", ex.Message);
        }

        [Test]
        public void SendTransactionWithLowGasLimit()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);
            var from = ganacheProvider.GetSigner();
            const string to = "0x1234567890123456789012345678901234567890";
            var amount = new HexBigInteger(1000000);
            var gasLimit = new HexBigInteger(1);
            var transaction = new TransactionRequest
            {
                To = to,
                Value = amount,
                GasLimit = gasLimit,
            };

            var ex = Assert.ThrowsAsync<Exception>(async () =>
            {
                var txHash = await from.SendTransaction(transaction);
            });
            Assert.AreEqual("eth_sendTransaction: -32000 intrinsic gas too low ", ex.Message);
        }

        [Test]
        public void SendTransactionWithLowGasPrice()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);
            var from = ganacheProvider.GetSigner();
            const string to = "0x1234567890123456789012345678901234567890";
            var amount = new HexBigInteger(1000000);
            var gasPrice = new HexBigInteger(1);
            var transaction = new TransactionRequest
            {
                To = to,
                Value = amount,
                GasPrice = gasPrice,
            };

            Assert.ThrowsAsync<Exception>(() => from.SendTransaction(transaction));
        }
    }
}