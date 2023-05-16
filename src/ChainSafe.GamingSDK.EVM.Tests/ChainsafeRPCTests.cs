using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Utils;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    [TestFixture]
    public class ChainSafeRpcTests
    {
        private JsonRpcProvider infuraProvider;
        private JsonRpcProvider ganacheProvider;
        private string goerliAddress;
        private string nftAddress;
        private string nftABi;

        [OneTimeSetUp]
        public void SetUp()
        {
            ganacheProvider = ProviderMigration.NewJsonRpcProvider("http://127.0.0.1:7545");
            infuraProvider = ProviderMigration.NewJsonRpcProvider("https://goerli.infura.io/v3/904006115c764661965dc0909e5ed473");
            goerliAddress = "0xaBed4239E4855E120fDA34aDBEABDd2911626BA1";
            nftAddress = "0xc81fa2eacc1c45688d481b11ce94c24a136e125d";
            nftABi =
                "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"_data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        }

        [Test]
        public void GetNetworkTest()
        {
            var network = infuraProvider.GetNetwork().Result;
            Assert.AreEqual("Goerli", network.Name);
            Assert.AreEqual(5, network.ChainId);
        }

        [Test]
        public void GetBalanceTest()
        {
            var balance = infuraProvider.GetBalance(goerliAddress).Result;
            var balanceFormatted = Units.FormatEther(balance);
            Assert.Greater(balanceFormatted, "0");
        }

        [Test]
        public void GetCodeTest()
        {
            var code = infuraProvider.GetCode(goerliAddress).Result;
            Assert.AreEqual("0x", code);
        }

        [Test]
        public void GetLastBlockTest()
        {
            var latestBlock = infuraProvider.GetBlock().Result;
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
            var currentBlockNumber = infuraProvider.GetBlockNumber().Result;
            Assert.Greater(currentBlockNumber.ToString(), "0");
        }

        [Test]
        public void GetBlockByNumberTest()
        {
            var currentBlockNumber = infuraProvider.GetBlockNumber().Result;
            var blockByNumber = infuraProvider.GetBlock(new BlockParameter(currentBlockNumber)).Result;
            Assert.AreEqual(blockByNumber.Number, currentBlockNumber);
        }

        [Test]
        public void GetBlockWithTransactionsTest()
        {
            var currentBlockNumber = infuraProvider.GetBlockNumber().Result;
            var blockParameter = new BlockParameter(currentBlockNumber.ToUlong());
            var blockWithTx = infuraProvider.GetBlockWithTransactions(blockParameter).Result;

            var firstTransaction = blockWithTx.Transactions[0];
            Assert.AreEqual(5, firstTransaction.ChainId.ToUlong());
            Assert.AreEqual(currentBlockNumber, firstTransaction.BlockNumber);
        }

        [Test]
        public void GetPreviousBlockTest()
        {
            var currentBlockNumber = infuraProvider.GetBlockNumber().Result;
            var previousBlockNumber = currentBlockNumber.ToUlong() - 1;
            var previousBlock = infuraProvider.GetBlock(new BlockParameter(previousBlockNumber)).Result;
            Assert.AreEqual(previousBlockNumber, previousBlock.Number.ToUlong());
            Assert.True(previousBlock.BlockHash.StartsWith("0x"));
        }

        [Test]
        public void GetTransactionCountTest()
        {
            var txCount = infuraProvider.GetTransactionCount(goerliAddress).Result;
            Assert.Greater(txCount.ToUlong(), 100);
        }

        [Test]
        public void GetFeeDataTest()
        {
            var feeData = infuraProvider.GetFeeData().Result;
            Assert.Greater(Units.FormatUnits(feeData.MaxFeePerGas, "gwei"), "0");
            Assert.Greater(Units.FormatUnits(feeData.GasPrice, "gwei"), "0");
            Assert.AreEqual(Units.FormatUnits(feeData.MaxPriorityFeePerGas, "gwei"), "1.5");
        }

        [Test]
        public void GetGasPriceTest()
        {
            var gasPrice = infuraProvider.GetGasPrice().Result;
            var gwei = Units.FormatUnits(gasPrice, "gwei");
            Assert.Greater(gasPrice.ToString(), "0");
            Assert.Greater(gasPrice.ToString(), gwei);
        }

        [Test]
        public void GetTransactionReceiptTest()
        {
            var latestBlockWithTx = infuraProvider.GetBlockWithTransactions().Result;
            var firstTransactionInBlock = latestBlockWithTx.Transactions[0];
            var receipt =
                infuraProvider.GetTransactionReceipt(
                    firstTransactionInBlock.Hash).Result;
            Assert.AreEqual(firstTransactionInBlock.Hash, receipt.TransactionHash);
            Assert.AreEqual(firstTransactionInBlock.BlockHash, receipt.BlockHash);
        }

        [Test]
        public void CallContractMethodTest()
        {
            var contract = new Contract(nftABi, nftAddress, infuraProvider);

            var name = contract.Call("name").Result;
            Assert.AreEqual("MyNFT", name[0]);

            var symbol = contract.Call("symbol").Result;
            Assert.AreEqual("MNFT", symbol[0]);

            var tokenUri = contract.Call("tokenURI", new object[] { 1 }).Result;
            Assert.AreEqual("http://mynft.com/1", tokenUri[0]);

            var ownerOf = contract.Call("ownerOf", new object[] { 1 }).Result;
            Assert.AreEqual("0x7f686f9B2740e09469a4A5A0F2eDE2d317D4aeE5", ownerOf[0]);

            var balanceOf = contract.Call("balanceOf", new[] { ownerOf[0] }).Result;
            Assert.AreEqual(new BigInteger(6), balanceOf[0]);
        }

        [Test]
        public void EstimateGasContractMethodTest()
        {
            var contract = new Contract(nftABi, goerliAddress, infuraProvider);
            var result = contract.EstimateGas("ownerOf", new object[] { 1 }).Result;
            Assert.AreEqual("21204", result.ToString());
        }

        [Test]
        public void GetAccountsTest()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);

            var accounts = ganacheProvider.ListAccounts().Result;
            Assert.AreEqual(10, accounts.Length);
            foreach (var account in accounts)
            {
                var accountBalance = ganacheProvider.GetBalance(account).Result;
                Assert.GreaterOrEqual(Units.FormatEther(accountBalance), "0");
            }
        }

        [Test]
        public void GetSignerTest()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);

            var signer = ganacheProvider.GetSigner();
            var accounts = ganacheProvider.ListAccounts().Result;
            Assert.AreEqual(accounts[0], signer.GetAddress().Result);

            var accountBalance = ganacheProvider.GetBalance(accounts[0]).Result;
            Assert.AreEqual(accountBalance, signer.GetBalance().Result);
            Assert.AreEqual(1337, signer.GetChainId().Result);
        }

        [Test]
        public void SendContractTest()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);

            var signer = ganacheProvider.GetSigner();

            var contract = new Contract(nftABi, nftAddress, ganacheProvider);
            var ret = contract.Connect(signer).Send("mint").Result;

            Assert.IsNotNull(ret);
        }

        [Test]
        public void SendContractOverrideGasTest()
        {
            TestHelper.VerifyGanacheConnection(ganacheProvider);

            var signer = ganacheProvider.GetSigner();

            var contract = new Contract(nftABi, nftAddress, ganacheProvider);
            var ret = contract.Attach(nftAddress).Connect(signer).Send("mint", null, new TransactionRequest
            {
                GasLimit = new HexBigInteger("10000"),
                GasPrice = new HexBigInteger("100000000"),
            }).Result;

            Assert.IsNotNull(ret);
        }
    }
}