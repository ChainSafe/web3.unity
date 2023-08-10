using System;
using System.Diagnostics;
using ChainSafe.GamingSDK.EVM.Tests.Node;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Utils;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    using Web3 = ChainSafe.GamingWeb3.Web3;

    [TestFixture]
    public class ChainSafeRpcTests
    {
        // todo: we need to actually seed the Ganache instance with some transactions before the tests will run
        private Web3 web3;
        private string walletAddress;

        private Process node;
        /* private string contractAddress; */
        /*
        private string nftAddress;
        private string nftAbi;
        */

        [OneTimeSetUp]
        public void SetUp()
        {
            node = Emulator.CreateInstance();

            // We can't get a .Result from ValueTask
            // that is why we need to explicitly wait for
            // it to finish
            var web3Task = Web3Util.CreateWeb3().AsTask();
            web3Task.Wait();
            web3 = web3Task.Result;

            var secondAccountTask = Web3Util.CreateWeb3(1).AsTask();
            secondAccountTask.Wait();

            var walletAddressTask = web3.Signer.GetAddress();
            walletAddressTask.Wait();

            walletAddress = walletAddressTask.Result;
            var toAddressTask = secondAccountTask.Result.Signer.GetAddress();
            toAddressTask.Wait();

            var amount = new HexBigInteger(1000000);
            var txTask = web3.TransactionExecutor.SendTransaction(new TransactionRequest
            {
                To = toAddressTask.Result,
                Value = amount,
            });
            txTask.Wait();

            Console.Out.Write("Setup Transaction Hash: {0}", txTask.Result.Hash);

            // web3.ContractBuilder.Build()

            /*
            nftAddress = "0xc81fa2eacc1c45688d481b11ce94c24a136e125d";
            nftAbi =
                "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"_data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
            */

            // Deploy a contract with https://remix.ethereum.org/ and your Ganache instance.
            // Make sure to select Ganache Provider as the Environment. Once deployed, replace the _contractAddress value with your contract address
            // todo: This is impossible to automate
            /* contractAddress = "0x2061c2B28F007DD0eD5064A61d352521CC1d2827"; */
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            node?.Kill();
        }

        [Test]
        public void GetNetworkTest()
        {
            var network = web3.RpcProvider.GetNetwork().Result;
            Assert.AreEqual("GoChain Testnet", network.Name);
            Assert.AreEqual(31337, network.ChainId);
        }

        [Test]
        public void GetBalanceTest()
        {
            var balance = web3.RpcProvider.GetBalance(walletAddress).Result;
            var balanceFormatted = Units.FormatEther(balance);
            Assert.Greater(balanceFormatted, "9990");
        }

        [Test]
        public void GetCodeTest()
        {
            var code = web3.RpcProvider.GetCode(walletAddress).Result;
            Assert.AreEqual("0x", code);
        }

        [Test]
        public void GetLastBlockTest()
        {
            var latestBlock = web3.RpcProvider.GetBlock().Result;
            Assert.AreEqual("0x0000000000000000", latestBlock.Nonce);
            Assert.AreEqual("30000000", latestBlock.GasLimit.ToString());
            Assert.True(latestBlock.BlockHash.StartsWith("0x"));
            Assert.True(latestBlock.ParentHash.StartsWith("0x"));
            Assert.IsNotEmpty(latestBlock.Number.ToString());
            Assert.IsNotEmpty(latestBlock.Timestamp.ToString());
        }

        [Test]
        public void GetBlockNumberTest()
        {
            var currentBlockNumber = web3.RpcProvider.GetBlockNumber().Result;
            Assert.Greater(currentBlockNumber.ToString(), "0");
        }

        [Test]
        public void GetBlockByNumberTest()
        {
            var currentBlockNumber = web3.RpcProvider.GetBlockNumber().Result;
            var blockByNumber = web3.RpcProvider.GetBlock(new BlockParameter(currentBlockNumber)).Result;
            Assert.AreEqual(blockByNumber.Number, currentBlockNumber);
        }

        [Test]
        public void GetBlockWithTransactionsTest()
        {
            var currentBlockNumber = web3.RpcProvider.GetBlockNumber().Result;
            var blockParameter = new BlockParameter(currentBlockNumber.ToUlong());
            var blockWithTx = web3.RpcProvider.GetBlockWithTransactions(blockParameter).Result;

            var firstTransaction = blockWithTx.Transactions[0];
            Assert.AreEqual(31337, firstTransaction.ChainId.ToUlong());
            Assert.AreEqual(currentBlockNumber, firstTransaction.BlockNumber);
        }

        [Test]
        public void GetPreviousBlockTest()
        {
            var currentBlockNumber = web3.RpcProvider.GetBlockNumber().Result;
            var previousBlockNumber = currentBlockNumber.ToUlong() - 1;
            var previousBlock = web3.RpcProvider.GetBlock(new BlockParameter(previousBlockNumber)).Result;
            Assert.AreEqual(previousBlockNumber, previousBlock.Number.ToUlong());
            Assert.True(previousBlock.BlockHash.StartsWith("0x"));
        }

        [Test]
        public void GetTransactionCountTest()
        {
            var txCount = web3.RpcProvider.GetTransactionCount(walletAddress).Result;
            Assert.Greater(txCount.ToUlong(), 0);
        }

        [Test]
        public void GetFeeDataTest()
        {
            var feeData = web3.RpcProvider.GetFeeData().Result;
            Assert.Greater(Units.FormatUnits(feeData.MaxFeePerGas, "gwei"), "0");
            Assert.Greater(Units.FormatUnits(feeData.GasPrice, "gwei"), "0");
            Assert.AreEqual(Units.FormatUnits(feeData.MaxPriorityFeePerGas, "gwei"), "1.5");
        }

        [Test]
        public void GetGasPriceTest()
        {
            var gasPrice = web3.RpcProvider.GetGasPrice().Result;
            var gwei = Units.FormatUnits(gasPrice, "gwei");
            Assert.Greater(gasPrice.ToString(), "0");
            Assert.Greater(gasPrice.ToString(), gwei);
        }

        [Test]
        public void GetTransactionReceiptTest()
        {
            var latestBlockWithTx = web3.RpcProvider.GetBlockWithTransactions().Result;
            var firstTransactionInBlock = latestBlockWithTx.Transactions[0];
            var receipt =
                web3.RpcProvider.GetTransactionReceipt(
                    firstTransactionInBlock.Hash).Result;
            Assert.AreEqual(firstTransactionInBlock.Hash, receipt.TransactionHash);
            Assert.AreEqual(firstTransactionInBlock.BlockHash, receipt.BlockHash);
        }

        /*
         * To run this test you need to deploy a contract using your Ganache instance and the contract needs to have a safeMint function that increments the tokenId counter
         * Example contract .sol file:
         * contract Contract is ERC721, Ownable {
                using Counters for Counters.Counter;
                Counters.Counter private _tokenIdCounter;
                constructor() ERC721("Contract", "MNFT") {}

                function safeMint(address to) public onlyOwner {
                    uint256 tokenId = _tokenIdCounter.current();
                    _tokenIdCounter.increment();
                    _safeMint(to, tokenId);
                }
            }
         */
        /* Commented out as this test cannot be run without configuration in Ganache
        [Test]
        public void CallContractMethodTest()
        {
            var address = web3.Signer.GetAddress().Result;
            var contract = web3.ContractBuilder.Build(nftAbi, contractAddress);

            var ret = contract.Send("safeMint", new object[] { address }).Result;

            var name = contract.Call("name").Result;
            Assert.AreEqual("Contract", name[0]);

            var symbol = contract.Call("symbol").Result;
            Assert.AreEqual("MNFT", symbol[0]);

            var tokenUri = contract.Call("tokenURI", new object[] { 0 }).Result;
            Assert.AreEqual(string.Empty, tokenUri[0]);

            var ownerOf = contract.Call("ownerOf", new object[] { 0 }).Result;
            StringAssert.AreEqualIgnoringCase(address, ownerOf[0].ToString());

            var balanceOf = contract.Call("balanceOf", new[] { ownerOf[0] }).Result;
            Assert.GreaterOrEqual(balanceOf[0].ToString(), "1");
        }
        */

        /*
        [Test]
        public void EstimateGasContractMethodTest()
        {
            var contract = web3.ContractBuilder.Build(nftAbi, walletAddress);
            var result = contract.EstimateGas("ownerOf", new object[] { 1 }).Result;
            Assert.AreEqual("21204", result.ToString());
        }
        */

        [Test]
        public void GetAccountsTest()
        {
            var accounts = (web3.RpcProvider as JsonRpcProvider)?.ListAccounts().Result;
            Assert.AreEqual(10, accounts?.Length);
            foreach (var account in accounts)
            {
                var accountBalance = web3.RpcProvider.GetBalance(account).Result;
                Assert.GreaterOrEqual(Units.FormatEther(accountBalance), "0");
            }
        }

        /*
        [Test]
        public void SendContractTest()
        {
            var contract = web3.ContractBuilder.Build(nftAbi, nftAddress);
            var ret = contract.Send("safeMint").Result;

            Assert.IsNotNull(ret);
        }
        */

        /*
        [Test]
        public void SendContractOverrideGasTest()
        {
            var contract = web3.ContractBuilder.Build(nftAbi, nftAddress);
            var ret = contract.Send("mint", null, new TransactionRequest
            {
                GasLimit = new HexBigInteger("10000"),
                GasPrice = new HexBigInteger("100000000"),
            }).Result;

            Assert.IsNotNull(ret);
        }
        */
    }
}