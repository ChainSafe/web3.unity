using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class MiscTests : SampleTestsBase
{
    #region ContractCalls

    private const string ContractSendMethodName = "addTotal";

    private const int IncreaseAmount = 1;

    private const string Abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";

    private const string ContractAddress = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";

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

    #endregion

    private Evm evm;
    
    [UnitySetUp]
    public override IEnumerator Setup()
    {
        yield return base.Setup();

        evm = new Evm(web3Result);
    }

    [UnityTest]
    public IEnumerator TestContractSend()
    {
        config.TestResponse = "0x9de3bb69db4bd93babef923f5da1f53cdb287d9ebab9b4177ba2fb25e6a09225";
        
        object[] args =
        {
            IncreaseAmount
        };
        
        var sendContract = evm.ContractSend(ContractSendMethodName, Abi, ContractAddress, args);

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
        var getArray = evm.GetArray(contractAddress, abi, method);

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
        var getBlockNumber = evm.GetBlockNumber();

        yield return new WaitUntil(() => getBlockNumber.IsCompleted);

        if (getBlockNumber.Exception != null) throw getBlockNumber.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getBlockNumber.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasLimit()
    {
        var getGasLimit = evm.GetGasLimit(Abi, ContractAddress, "addTotal");

        yield return new WaitUntil(() => getGasLimit.IsCompleted);

        if (getGasLimit.Exception != null) throw getGasLimit.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasLimit.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = evm.GetGasPrice();

        yield return new WaitUntil(() => getGasPrice.IsCompleted);

        if (getGasPrice.Exception != null) throw getGasPrice.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasPrice.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasNonce()
    {
        config.TestResponse = "0x527fcd7356738389d29a96342b5fba92ab1348b744409d5bf4ce0ca2fbc2f25e";

        var getGasNonce = evm.GetNonce();

        yield return new WaitUntil(() => getGasNonce.IsCompleted);

        if (getGasNonce.Exception != null) throw getGasNonce.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasNonce.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestTransactionStatus()
    {
        config.TestResponse = "0x1e989dbcc43e078b19ea8ea201af195e74397b494b7acd4afcca67e65e5c3339";

        var getTransactionStatus = evm.GetTransactionStatus();

        yield return new WaitUntil(() => getTransactionStatus.IsCompleted);

        if (getTransactionStatus.Exception != null) throw getTransactionStatus.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getTransactionStatus.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestUseRegisteredContract()
    {
        var useRegisteredContract = evm.UseRegisteredContract("shiba", "balanceOf");

        yield return new WaitUntil(() => useRegisteredContract.IsCompleted);

        if (useRegisteredContract.Exception != null) throw useRegisteredContract.Exception;
        
        Assert.IsTrue(useRegisteredContract.IsCompletedSuccessfully);

        Assert.AreEqual(useRegisteredContract.Result, new BigInteger(0));
    }

    [UnityTest]
    public IEnumerator TestSendArray()
    {
        config.TestResponse = "0x6a33280f3b2b907da613b18b09f863cd835f1977a4131001ace5602899fc98c7";

        var sendArray = evm.SendArray(SendArrayMethodName, SendArrayAbi, SendArrayAddress, ArrayToSend.ToArray());

        yield return new WaitUntil(() => sendArray.IsCompleted);

        if (sendArray.Exception != null) throw sendArray.Exception;
        
        Assert.IsTrue(sendArray.IsCompletedSuccessfully);

        Assert.AreEqual(sendArray.Result, string.Empty);
    }

    [UnityTest]
    public IEnumerator TestSendTransaction()
    {
        config.TestResponse = "0xa60bef1df91bedcd2f3f79e6609716ef245fd1202d66c6e35694b43529bf2e71";

        var sendTransaction = evm.SendTransaction(SendToAddress);

        yield return new WaitUntil(() => sendTransaction.IsCompleted);

        if (sendTransaction.Exception != null) throw sendTransaction.Exception;
        
        Assert.IsTrue(sendTransaction.IsCompletedSuccessfully);

        Assert.AreEqual(sendTransaction.Result, config.TestResponse);
    }

    [UnityTest]
    public IEnumerator TestSha3()
    {
        var sha3 = evm.Sha3("It’s dangerous to go alone, take this!");

        Assert.AreEqual(sha3, "45760485edafabcbb28570e7da5ddda4639abb4794de9af6de1d6669cbf220fc");

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSignMessage()
    {
        config.TestResponse =
            "0x87dfaa646f476ca53ba8b6e8d122839571e52866be0984ec0497617ad3e988b7401c6b816858df27625166cb98a688f99ba92fa593da3c86c78b19c78c1f51cc1c";

        var signMessage = evm.SignMessage("The right man in the wrong place can make all the difference in the world.");

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

        var signVerify = evm.SignVerify("A man chooses, a slave obeys.");

        yield return new WaitUntil(() => signVerify.IsCompleted);

        if (signVerify.Exception != null) throw signVerify.Exception;
        
        Assert.IsTrue(signVerify.IsCompletedSuccessfully);

        Assert.AreEqual(signVerify.Result, true);
    }
}
