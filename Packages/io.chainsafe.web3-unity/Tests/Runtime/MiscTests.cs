using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Web3Unity.Scripts.Prefabs;

public class MiscTests : SampleTestsBase
{
    private UnsortedSample _sample;

    private const string ContractSendMethodName = "addTotal";

    private const int IncreaseAmount = 1;

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
    private const string TransferErc1155ContractAddress = "0xe793e17Ec93bEc809C5Ac6dd0d8b383446E65B78";

    #endregion

    [UnitySetUp]
    public override IEnumerator Setup()
    {
        yield return base.Setup();

        _sample = new UnsortedSample(web3Result);
    }

    [UnityTest]
    public IEnumerator TestContractSend()
    {
        config.TestResponse = "0x9de3bb69db4bd93babef923f5da1f53cdb287d9ebab9b4177ba2fb25e6a09225";
        
        object[] args =
        {
            IncreaseAmount
        };
        
        var sendContract = _sample.ContractSend(ContractSendMethodName, Abi, ContractAddress, args);

        yield return new WaitUntil(() => sendContract.IsCompleted);

        if (sendContract.Exception != null) throw sendContract.Exception;
        
        Assert.IsTrue(sendContract.IsCompletedSuccessfully);

        Assert.AreEqual(sendContract.Result, string.Empty);
    }

    [UnityTest]
    public IEnumerator TestGetArray()
    {
        string contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
        string abi = "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        string method = "getStore";
        var getArray = _sample.GetArray(contractAddress, abi, method);

        yield return new WaitUntil(() => getArray.IsCompleted);

        //convert toLower to make comparing easier
        var result = getArray.Result.ConvertAll(a => a.ConvertAll(b => b.ToLower()));

        Assert.AreEqual(result, new List<List<string>>
        {
            ArrayToSend.ConvertAll(a => a.ToLower())
        });
    }

    [UnityTest]
    public IEnumerator TestGetBlockNumber()
    {
        var getBlockNumber = _sample.GetBlockNumber();

        yield return new WaitUntil(() => getBlockNumber.IsCompleted);

        if (getBlockNumber.Exception != null) throw getBlockNumber.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getBlockNumber.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasLimit()
    {
        var getGasLimit = _sample.GetGasLimit(Abi, ContractAddress, "addTotal");

        yield return new WaitUntil(() => getGasLimit.IsCompleted);

        if (getGasLimit.Exception != null) throw getGasLimit.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasLimit.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = _sample.GetGasPrice();

        yield return new WaitUntil(() => getGasPrice.IsCompleted);

        if (getGasPrice.Exception != null) throw getGasPrice.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasPrice.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasNonce()
    {
        config.TestResponse = "0x527fcd7356738389d29a96342b5fba92ab1348b744409d5bf4ce0ca2fbc2f25e";

        var getGasNonce = _sample.GetNonce();

        yield return new WaitUntil(() => getGasNonce.IsCompleted);

        if (getGasNonce.Exception != null) throw getGasNonce.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasNonce.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestTransactionStatus()
    {
        config.TestResponse = "0x1e989dbcc43e078b19ea8ea201af195e74397b494b7acd4afcca67e65e5c3339";

        var getTransactionStatus = _sample.GetTransactionStatus();

        yield return new WaitUntil(() => getTransactionStatus.IsCompleted);

        if (getTransactionStatus.Exception != null) throw getTransactionStatus.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getTransactionStatus.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestMint721()
    {
        config.TestResponse = "0xd3027fbfd9d5ddb5ea0ef75f5b128581d9268ad67728d150657f915c8910f9f0";

        var mint721 = _sample.Mint721(Mint721Abi, Mint721Address, MintUri);

        yield return new WaitUntil(() => mint721.IsCompleted);

        if (mint721.Exception != null) throw mint721.Exception;
        
        Assert.IsTrue(mint721.IsCompletedSuccessfully);

        Assert.AreEqual(mint721.Result, string.Empty);
    }

    [UnityTest]
    public IEnumerator TestUseRegisteredContract()
    {
        var useRegisteredContract = _sample.UseRegisteredContract("shiba", "balanceOf");

        yield return new WaitUntil(() => useRegisteredContract.IsCompleted);

        if (useRegisteredContract.Exception != null) throw useRegisteredContract.Exception;
        
        Assert.IsTrue(useRegisteredContract.IsCompletedSuccessfully);

        Assert.AreEqual(useRegisteredContract.Result, new BigInteger(0));
    }

    [UnityTest]
    public IEnumerator TestSendArray()
    {
        config.TestResponse = "0x6a33280f3b2b907da613b18b09f863cd835f1977a4131001ace5602899fc98c7";

        var sendArray = _sample.SendArray(SendArrayMethodName, SendArrayAbi, SendArrayAddress, ArrayToSend.ToArray());

        yield return new WaitUntil(() => sendArray.IsCompleted);

        if (sendArray.Exception != null) throw sendArray.Exception;
        
        Assert.IsTrue(sendArray.IsCompletedSuccessfully);

        Assert.AreEqual(sendArray.Result, string.Empty);
    }

    [UnityTest]
    public IEnumerator TestSendTransaction()
    {
        config.TestResponse = "0xa60bef1df91bedcd2f3f79e6609716ef245fd1202d66c6e35694b43529bf2e71";

        var sendTransaction = _sample.SendTransaction(SendToAddress);

        yield return new WaitUntil(() => sendTransaction.IsCompleted);

        if (sendTransaction.Exception != null) throw sendTransaction.Exception;
        
        Assert.IsTrue(sendTransaction.IsCompletedSuccessfully);

        Assert.AreEqual(sendTransaction.Result, config.TestResponse);
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
        config.TestResponse =
            "0x87dfaa646f476ca53ba8b6e8d122839571e52866be0984ec0497617ad3e988b7401c6b816858df27625166cb98a688f99ba92fa593da3c86c78b19c78c1f51cc1c";

        var signMessage = _sample.SignMessage("The right man in the wrong place can make all the difference in the world.");

        yield return new WaitUntil(() => signMessage.IsCompleted);

        if (signMessage.Exception != null) throw signMessage.Exception;
        
        Assert.IsTrue(signMessage.IsCompletedSuccessfully);

        Assert.AreEqual(signMessage.Result, config.TestResponse);
    }

    [UnityTest]
    public IEnumerator TestSignVerify()
    {
        config.TestResponse =
            "0x5c996d43c2e804a0d0de7f8b07cc660bbae638aa7ea137df6156621abe5e1fbb1727ebb06f7e0067537cb0f942825fa15ead9dea6d74e4d17fa6e69007cb59561c";

        var signVerify = _sample.SignVerify("A man chooses, a slave obeys.");

        yield return new WaitUntil(() => signVerify.IsCompleted);

        if (signVerify.Exception != null) throw signVerify.Exception;
        
        Assert.IsTrue(signVerify.IsCompletedSuccessfully);

        Assert.AreEqual(signVerify.Result, true);
    }

    [UnityTest]
    public IEnumerator TestTransferErc20()
    {
        config.TestResponse = "0xba90b6fb8cbee5fd0ad423cc74bb4a365bb88b260601933aac86b947945c5465";

        var transferErc20 = _sample.TransferErc20(TransferErc20ContractAddress, SendToAddress, "1000000000000000");

        yield return new WaitUntil(() => transferErc20.IsCompleted);

        if (transferErc20.Exception != null) throw transferErc20.Exception;
        
        Assert.IsTrue(transferErc20.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc20.Result, new object[] { false });
    }

    [UnityTest]
    public IEnumerator TestTransferErc721()
    {
        config.TestResponse = "0x0e292ae8c5ab005d87581f32fd791e1b18b0cfa944d6877b41edbdb740ee8586";

        var transferErc721 = _sample.TransferErc721(TransferErc721ContractAddress, SendToAddress, 0);

        yield return new WaitUntil(() => transferErc721.IsCompleted);

        if (transferErc721.Exception != null) throw transferErc721.Exception;
        
        Assert.IsTrue(transferErc721.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc721.Result, string.Empty);
    }

    [UnityTest]
    public IEnumerator TestTransferErc1155()
    {
        config.TestResponse = "0xb018a043ac0affe05159a53daa8656dbbad61c839eaf89622d7813226f222876";

        var transferErc1155 = _sample.TransferErc1155(TransferErc1155ContractAddress, 101, 1, SendToAddress);

        yield return new WaitUntil(() => transferErc1155.IsCompleted);

        if (transferErc1155.Exception != null) throw transferErc1155.Exception;
        
        yield return new WaitUntil(() => transferErc1155.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc1155.Result, string.Empty);
    }
}
