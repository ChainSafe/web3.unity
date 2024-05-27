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

    /// <summary>
    /// Test class for testing various aspects of sending transactions using Web3 providers.
    /// </summary>
    [TestFixture]
    public class ProvidersSendTests
    {
        private Web3 firstAccount;
        private Web3 secondAccount;
        private Process node;

        /// <summary>
        /// One-time setup method that initializes the test environment and resources.
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            // Create a local Ethereum node emulator for testing.
            node = Emulator.CreateInstance();

            // Create Web3 instances for the first and second accounts.
            var firstAccountTask = CreateWeb3(0).AsTask();
            firstAccountTask.Wait();
            firstAccount = firstAccountTask.Result;

            var secondAccountTask = CreateWeb3(1).AsTask();
            secondAccountTask.Wait();
            secondAccount = secondAccountTask.Result;
        }

        /// <summary>
        /// One-time teardown method to clean up and release any resources after all tests have executed.
        /// </summary>
        [OneTimeTearDown]
        public void Cleanup()
        {
            node?.Kill();
        }

        /// <summary>
        /// Test method to validate sending a transaction from one account to another.
        /// </summary>
        [Test]
        public void SendTransactionTest()
        {
            // Get initial balances and addresses for both sender and receiver accounts.
            var fromAddress = firstAccount.Signer.PublicAddress;
            var fromInitialBalance = firstAccount.RpcProvider.GetBalance(fromAddress).Result.Value;

            var toAddress = secondAccount.Signer.PublicAddress;
            var toInitialBalance = firstAccount.RpcProvider.GetBalance(toAddress).Result.Value;

            var amount = new HexBigInteger(1000000);

            // Send a transaction from the first account to the second account.
            var tx = firstAccount.TransactionExecutor.SendTransaction(new TransactionRequest
            {
                To = toAddress,
                Value = amount,
            });
            tx.Wait();

            // Verify the transaction hash and its properties.
            Assert.True(tx.Result.Hash.StartsWith("0x"));

            // Retrieve the transaction receipt and perform additional assertions.
            var txReceipt = firstAccount.RpcProvider.GetTransactionReceipt(tx.Result.Hash);
            txReceipt.Wait();

            Assert.AreEqual(txReceipt.Result.Confirmations, 1);
            Assert.AreEqual(toInitialBalance + amount.Value, firstAccount.RpcProvider.GetBalance(toAddress).Result.Value);
            Assert.AreEqual(
                fromInitialBalance - amount.Value - (txReceipt.Result.CumulativeGasUsed.Value * txReceipt.Result.EffectiveGasPrice.Value),
                firstAccount.RpcProvider.GetBalance(fromAddress).Result.Value);
        }

        /// <summary>
        /// Test method to verify sending a transaction with an invalid destination address.
        /// </summary>
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

        /// <summary>
        /// Test method to validate sending a transaction with an insufficient gas limit.
        /// </summary>
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
        }

        /// <summary>
        /// Test method to validate sending a transaction with a very low gas price.
        /// </summary>
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
