using System;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.NetCore;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    using Web3 = ChainSafe.GamingWeb3.Web3;

    [TestFixture]
    public class ProvidersSendTests
    {
        private Web3 web3;
        private Web3 secondAccount;

        [OneTimeSetUp]
        public void SetUp()
        {
            web3 = Web3Util.CreateWeb3().Result;
            secondAccount = Web3Util.CreateWeb3(1).Result;
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
            }).Result;
            Assert.True(tx.Hash.StartsWith("0x"));

            var txReceipt = tx.Wait().Result;

            Assert.AreEqual(txReceipt.Confirmations, 1);
            Assert.AreEqual(toInitialBalance + amount.Value, web3.RpcProvider.GetBalance(toAddress).Result.Value);
            Assert.AreEqual(
                fromInitialBalance - amount.Value - (txReceipt.CumulativeGasUsed.Value * txReceipt.EffectiveGasPrice.Value),
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