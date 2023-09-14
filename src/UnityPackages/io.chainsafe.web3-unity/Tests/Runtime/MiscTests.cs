using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Prefabs;

public class MiscTests : SampleTestsBase
{
    private UnsortedSample _sample;

    private const string ContractSendMethodName = "addTotal";
    
    private const string Abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    
    private const string ContractAddress = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";

    #region Mint721

    private const string Mint721Abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"burn\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"grantRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"renounceRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"revokeRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"previousAdminRole\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"newAdminRole\",\"type\":\"bytes32\"}],\"name\":\"RoleAdminChanged\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleGranted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleRevoked\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"string\",\"name\":\"uri\",\"type\":\"string\"}],\"name\":\"safeMint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"DEFAULT_ADMIN_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"}],\"name\":\"getRoleAdmin\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"hasRole\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"MINTER_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    
    private const string Mint721Address = "0x0B102638532be8A1b3d0ed1fcE6eC603Bec37848";

    private const string MintUri = "ipfs://QmNn5EaGR26kU7aAMH7LhkNsAGcmcyJgun3Wia4MftVicW/1.json";

    #endregion

    #region Array

    private const string SendArrayAbi = "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    
    private const string SendArrayAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
    
    private const string SendArrayMethodName = "setStore";

    private static readonly List<string> ArrayToSend = new List<string>()
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region Send/Transfer

    private const string SendToAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    
    private const string TransferErc20ContractAddress = "0xc778417e063141139fce010982780140aa0cd5ab";
    private const string TransferErc721ContractAddress = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
    private const string TransferErc1155ContractAddress = "0xae283E79a5361CF1077bf2638a1A953c872AD973";

    #endregion
    
    [UnitySetUp]
    public override IEnumerator Setup()
    {
        yield return base.Setup();

        _sample = new UnsortedSample(Web3Result);
    }

    [UnityTest]
    public IEnumerator TestContractSend()
    {
        var sendContract = _sample.ContractSend(ContractSendMethodName, Abi, ContractAddress);

        yield return new WaitUntil(() => sendContract.IsCompleted);
        
        //sendContract.Result
    }

    [UnityTest]
    public IEnumerator TestGetArray()
    {
        var getArray = _sample.GetArray();

        yield return new WaitUntil(() => getArray.IsCompleted);

        Assert.AreEqual(getArray.Result, new List<List<string>>
        {
            ArrayToSend
        });
    }
    
    [UnityTest]
    public IEnumerator TestGetBlockNumber()
    {
        var getBlockNumber = _sample.GetBlockNumber();

        yield return new WaitUntil(() => getBlockNumber.IsCompleted);

        Assert.AreEqual(getBlockNumber.Result.ToString(), "9691998");
    }
    
    [UnityTest]
    public IEnumerator TestGetGasLimit()
    {
        var getGasLimit = _sample.GetGasLimit(Abi, ContractAddress);

        yield return new WaitUntil(() => getGasLimit.IsCompleted);

        Assert.AreEqual(getGasLimit.Result, "9691998");
    }
    
    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = _sample.GetGasPrice();

        yield return new WaitUntil(() => getGasPrice.IsCompleted);

        Assert.AreEqual(getGasPrice.Result, "92");
    }
    
    [UnityTest]
    public IEnumerator TestGetGasNonce()
    {
        var getGasNonce = _sample.GetNonce();

        yield return new WaitUntil(() => getGasNonce.IsCompleted);

        Assert.AreEqual(getGasNonce.Result, "92");
    }
    
    [UnityTest]
    public IEnumerator TestTransactionStatus()
    {
        var getTransactionStatus = _sample.GetTransactionStatus();

        yield return new WaitUntil(() => getTransactionStatus.IsCompleted);

        Assert.AreEqual(getTransactionStatus.Result, new TransactionReceipt
        {
            
        });
    }
    
    [UnityTest]
    public IEnumerator TestMint721()
    {
        var mint721 = _sample.Mint721(Mint721Abi, Mint721Address, MintUri);

        yield return new WaitUntil(() => mint721.IsCompleted);

        Assert.AreEqual(mint721.Result, null);
    }
    
    [UnityTest]
    public IEnumerator TestUseRegisteredContract()
    {
        var useRegisteredContract = _sample.UseRegisteredContract();

        yield return new WaitUntil(() => useRegisteredContract.IsCompleted);

        Assert.AreEqual(useRegisteredContract.Result, "0");
    }
    
    [UnityTest]
    public IEnumerator TestSendArray()
    {
        var sendArray = _sample.SendArray(SendArrayMethodName, SendArrayAbi, SendArrayAddress, ArrayToSend.ToArray());

        yield return new WaitUntil(() => sendArray.IsCompleted);

        Assert.AreEqual(sendArray.Result, "0");
    }
    
    [UnityTest]
    public IEnumerator TestSendTransaction()
    {
        var sendTransaction = _sample.SendTransaction(SendToAddress);

        yield return new WaitUntil(() => sendTransaction.IsCompleted);

        Assert.AreEqual(sendTransaction.Result, "0");
    }
    
    [UnityTest]
    public IEnumerator TestSha3()
    {
        var sha3 = _sample.Sha3("It’s dangerous to go alone, take this!");

        Assert.AreEqual(sha3, "45760485edafabcbb28570e7da5ddda4639abb4794de9af6de1d6669cbf220fc");

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestSignMessage()
    {
        var signMessage = _sample.SignMessage("The right man in the wrong place can make all the difference in the world.");

        yield return new WaitUntil(() => signMessage.IsCompleted);
        
        Assert.AreEqual(signMessage.Result, "");
    }
    
    [UnityTest]
    public IEnumerator TestSignVerify()
    {
        var signVerify = _sample.SignVerify("A man chooses, a slave obeys.");

        yield return new WaitUntil(() => signVerify.IsCompleted);
        
        Assert.AreEqual(signVerify.Result, "");
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc20()
    {
        var transferErc20 = _sample.TransferErc20(TransferErc20ContractAddress, SendToAddress, "1000000000000000");

        yield return new WaitUntil(() => transferErc20.IsCompleted);
        
        Assert.AreEqual(transferErc20.Result, "");
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc721()
    {
        var transferErc721 = _sample.TransferErc721(TransferErc721ContractAddress, SendToAddress, 0);

        yield return new WaitUntil(() => transferErc721.IsCompleted);
        
        Assert.AreEqual(transferErc721.Result, "");
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc1155()
    {
        var transferErc1155 = _sample.TransferErc1155(TransferErc1155ContractAddress, 0, 1, SendToAddress);

        yield return new WaitUntil(() => transferErc1155.IsCompleted);
        
        Assert.AreEqual(transferErc1155.Result, "");
    }
}
