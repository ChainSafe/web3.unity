using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.ABI.Util;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Utils;

namespace Tests
{
    [TestFixture]
    public class ChainSafeRpcTests
    {
        private JsonRpcProvider _infuraProvider;
        private JsonRpcProvider _ganacheProvider;
        private string _goerliAddress;
        private string _nftAddress;
        
        [SetUp]
        public void SetUp()
        {
            _ganacheProvider = new JsonRpcProvider("http://127.0.0.1:7545");
            _infuraProvider = new JsonRpcProvider("https://goerli.infura.io/v3/904006115c764661965dc0909e5ed473");
            _goerliAddress = "0xaBed4239E4855E120fDA34aDBEABDd2911626BA1";
            _nftAddress = "0xc81fa2eacc1c45688d481b11ce94c24a136e125d";
        }
        
        [Test]
        public void GetNetworkTest()
        {
            var network = _infuraProvider.GetNetwork().Result;
            Assert.AreEqual("Goerli", network.Name );
            Assert.AreEqual(5,network.ChainId);
        }
        
        [Test]
        public void GetBalanceTest()
        {
            var balance = _infuraProvider.GetBalance(_goerliAddress).Result;
            var balanceFormatted = Units.FormatEther(balance);
            Assert.Greater(balanceFormatted, "0");
        }
        
        [Test]
        public void GetCodeTest()
        {
            var code = _infuraProvider.GetCode(_goerliAddress).Result;
            Assert.AreEqual("0x", code);
        }
        
        [Test]
        //[Ignore("failing")]
        public void GetStorageAtTest()
        {
            var slot = _infuraProvider.GetStorageAt("0x6B175474E89094C44Da98b954EedeAC495271d0F", new BigInteger(255)).Result;
            Console.WriteLine($"Contract slot 0: {slot}");
        }
        
        [Test]
        public void GetLastBlockTest()
        {
            var latestBlock = _infuraProvider.GetBlock().Result;
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
            var currentBlockNumber = _infuraProvider.GetBlockNumber().Result;
            Assert.Greater(currentBlockNumber.ToString(), "0");
        }
        
        [Test]
        public void GetBlockByNumberTest()
        {
            var currentBlockNumber = _infuraProvider.GetBlockNumber().Result;
            var blockByNumber = _infuraProvider.GetBlock(new BlockParameter(currentBlockNumber)).Result;
            Assert.AreEqual(blockByNumber.Number, currentBlockNumber);
        }
        
        [Test]
        public void GetBlockWithTransactionsTest()
        {
            var currentBlockNumber = _infuraProvider.GetBlockNumber().Result;
            var blockParameter = new BlockParameter(currentBlockNumber.ToUlong());
            var blockWithTx = _infuraProvider.GetBlockWithTransactions(blockParameter).Result;

            var firstTransaction = blockWithTx.Transactions[0];
            Assert.AreEqual(5, firstTransaction.ChainId.ToUlong());
            Assert.AreEqual(currentBlockNumber, firstTransaction.BlockNumber);
        }
        
        [Test]
        public void GetPreviousBlockTest()
        {
            var currentBlockNumber = _infuraProvider.GetBlockNumber().Result;
            var previousBlockNumber = currentBlockNumber.ToUlong() - 1;
            var previousBlock =  _infuraProvider.GetBlock(new BlockParameter(previousBlockNumber)).Result;
            Assert.AreEqual(previousBlockNumber, previousBlock.Number.ToUlong());
            Assert.True(previousBlock.BlockHash.StartsWith("0x"));
        }
        
        [Test]
        public void GetTransactionCountTest()
        {
            var txCount = _infuraProvider.GetTransactionCount(_goerliAddress).Result;
            Assert.Greater(txCount.ToUlong(), 100);
        }
        
        [Test]
        public void GetFeeDataTest()
        {
            var feeData = _infuraProvider.GetFeeData().Result;
            Assert.Greater(Units.FormatUnits(feeData.MaxFeePerGas, "gwei"), "0");
            Assert.Greater(Units.FormatUnits(feeData.GasPrice, "gwei"), "0");
            Assert.AreEqual(Units.FormatUnits(feeData.MaxPriorityFeePerGas, "gwei"), "1.5");
        }
        
        [Test]
        public void GetGasPriceTest()
        {
            var gasPrice = _infuraProvider.GetGasPrice().Result;
            var gwei = Units.FormatUnits(gasPrice, "gwei");
            Assert.Greater(gasPrice.ToString(), "0");
            Assert.Greater(gasPrice.ToString(), gwei);
        }
        
        [Test]
        public void GetTransactionReceiptTest()
        {
            var latestBlockWithTx = _infuraProvider.GetBlockWithTransactions().Result;
            var firstTransactionInBlock = latestBlockWithTx.Transactions[0];
            var receipt =
                _infuraProvider.GetTransactionReceipt(
                    firstTransactionInBlock.Hash).Result;
            Assert.AreEqual(firstTransactionInBlock.Hash, receipt.TransactionHash);
            Assert.AreEqual(firstTransactionInBlock.BlockHash, receipt.BlockHash);
        }
        
        [Test]
        public void CallContractMethodTest()
        {
            var _abi =
                "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"_data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
            var contract = new Contract(_abi, _nftAddress, _infuraProvider);

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
            var abi =
                "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"}],\"name\":\"safeMint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
            
            var contract = new Contract(abi, _goerliAddress, _infuraProvider);
            var result = contract.EstimateGas("ownerOf", new object[] { 1 }).Result;
            Assert.AreEqual("21204", result.ToString());
        }
        
        [Test]
        public void GetAccountsTest()
        {
            if (_ganacheProvider is not JsonRpcProvider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
            }

            var accounts = _ganacheProvider.ListAccounts().Result;
            Assert.AreEqual(10, accounts.Length);
            foreach (var account in accounts)
            {
                var accountBalance = _ganacheProvider.GetBalance(account).Result;
                Assert.GreaterOrEqual(Units.FormatEther(accountBalance), "0");
            }
        }
        
        [Test]
        public void GetSignerTest()
        {
            if (_infuraProvider is not JsonRpcProvider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
            }
            
            var signer = _infuraProvider.GetSigner(); // default signer at index 0
            // var signer = provider.GetSigner(1); // signer at index 1
            // var signer = provider.GetSigner("0x3c44cdddb6a900fa2b585dd299e03d12fa4293bc"); // signer by address

            Console.WriteLine($"Signer address: {signer.GetAddress().Result}");
            Console.WriteLine($"Signer balance: {Units.FormatEther(signer.GetBalance().Result)} ETH");
            Console.WriteLine($"Signer chain id: {signer.GetChainId().Result}");
            Console.WriteLine($"Signer tx count: {signer.GetTransactionCount().Result}");
        }
        
        [Test]
        public void SubscribeToNewBlockEventTest()
        {
            _ganacheProvider.On("block", (ulong blockNumber) =>
            {
                Console.WriteLine($"New block {blockNumber}");
                return null;
            });

            _ganacheProvider.On("chainChanged", (ulong chainId) =>
            {
                Console.WriteLine($"Chain changed {chainId}");
                return null;
            });
            
            Console.WriteLine(Task.FromResult(true));
        }
    }
}