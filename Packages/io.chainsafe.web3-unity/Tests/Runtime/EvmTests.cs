using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using ChainSafe.Gaming.Web3;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class EvmTests : SampleTestsBase
{
    #region ContractCalls
    
    private const string ContractSendMethodName = "addTotal";
    private const string ContractCallMethodName = "myTotal";
    private const int IncreaseAmount = 1;

    #endregion

    #region Array
    
    private const string GetArrayMethodName = "getStore";
    private const string SendArrayMethodName = "setStore";
    private static readonly List<string> ArrayToSend = new List<string>()
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region MintErc20
    
    private const string Mint20ToAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private const int Mint20Amount = 1;

    #endregion
    
    #region Mint721
    
    private const string Mint721Uri = "ipfs://QmNn5EaGR26kU7aAMH7LhkNsAGcmcyJgun3Wia4MftVicW/1.json";

    #endregion
    
    #region Mint1155
    
    private const int Mint1155Id = 1;
    private const int Mint1155Amount = 1;

    #endregion
    
    #region Transfer
    
    private const string SendToAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    #endregion

    private Web3 web3;
    
    [UnitySetUp]
    public override IEnumerator Setup()
    {
        yield return base.Setup();

        web3 = web3Result;
    }

    [UnityTest]
    public IEnumerator TestContractSend()
    {
        object[] args = { IncreaseAmount };
        
        var sendContract = Evm.ContractSend(web3, ContractSendMethodName, ABI.ArrayAndTotal, Contracts.ArrayAndTotal, args);

        yield return new WaitUntil(() => sendContract.IsCompleted);

        if (sendContract.Exception != null) throw sendContract.Exception;
        
        Assert.IsTrue(sendContract.IsCompletedSuccessfully);
        Debug.Log("TX" + sendContract.Result);

        Assert.AreEqual(sendContract.Result, string.Empty);
    }
    
    [UnityTest]
    public IEnumerator TestContractCall()
    {
        object[] args = { config.TestWalletAddress };
        
        var callContract = Evm.ContractCall(web3, ContractCallMethodName, ABI.ArrayAndTotal, Contracts.ArrayAndTotal, args);

        yield return new WaitUntil(() => callContract.IsCompleted);

        if (callContract.Exception != null) throw callContract.Exception;
        
        Assert.IsTrue(callContract.IsCompletedSuccessfully);

        Assert.AreEqual(callContract.Result[0].ToString(), "1");
    }
    
    [UnityTest]
    public IEnumerator TestSendArray()
    {
        var sendArray = Evm.SendArray(web3, SendArrayMethodName, ABI.ArrayAndTotal, Contracts.ArrayAndTotal, ArrayToSend.ToArray());

        yield return new WaitUntil(() => sendArray.IsCompleted);

        if (sendArray.Exception != null) throw sendArray.Exception;
        
        Assert.IsTrue(sendArray.IsCompletedSuccessfully);

        Assert.AreEqual(sendArray.Result, string.Empty);
    }

    [UnityTest]
    public IEnumerator TestGetArray()
    {
        var getArray = Evm.GetArray(web3, GetArrayMethodName, ABI.ArrayAndTotal, Contracts.ArrayAndTotal);

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
        var getBlockNumber = Evm.GetBlockNumber(web3);

        yield return new WaitUntil(() => getBlockNumber.IsCompleted);

        if (getBlockNumber.Exception != null) throw getBlockNumber.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getBlockNumber.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasLimit()
    {
        var getGasLimit = Evm.GetGasLimit(web3, ABI.ArrayAndTotal, ABI.ArrayAndTotal, ContractSendMethodName);

        yield return new WaitUntil(() => getGasLimit.IsCompleted);

        if (getGasLimit.Exception != null) throw getGasLimit.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasLimit.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = Evm.GetGasPrice(web3);

        yield return new WaitUntil(() => getGasPrice.IsCompleted);

        if (getGasPrice.Exception != null) throw getGasPrice.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasPrice.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasNonce()
    {
        config.TestResponse = "0x527fcd7356738389d29a96342b5fba92ab1348b744409d5bf4ce0ca2fbc2f25e";

        var getGasNonce = Evm.GetNonce(web3);

        yield return new WaitUntil(() => getGasNonce.IsCompleted);

        if (getGasNonce.Exception != null) throw getGasNonce.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getGasNonce.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestTransactionStatus()
    {
        config.TestResponse = "0x1e989dbcc43e078b19ea8ea201af195e74397b494b7acd4afcca67e65e5c3339";

        var getTransactionStatus = Evm.GetTransactionStatus(web3);

        yield return new WaitUntil(() => getTransactionStatus.IsCompleted);

        if (getTransactionStatus.Exception != null) throw getTransactionStatus.Exception;
        
        //just assert successful completion because result is always changing
        Assert.IsTrue(getTransactionStatus.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestUseRegisteredContract()
    {
        var useRegisteredContract = Evm.UseRegisteredContract(web3, "CsTestErc20", "balanceOf");

        yield return new WaitUntil(() => useRegisteredContract.IsCompleted);

        if (useRegisteredContract.Exception != null) throw useRegisteredContract.Exception;
        
        Assert.IsTrue(useRegisteredContract.IsCompletedSuccessfully);

        Assert.AreEqual(useRegisteredContract.Result, new BigInteger(1000000000000000000));
    }

    [UnityTest]
    public IEnumerator TestSendTransaction()
    {
        config.TestResponse = "0xa60bef1df91bedcd2f3f79e6609716ef245fd1202d66c6e35694b43529bf2e71";

        var sendTransaction = Evm.SendTransaction(web3, SendToAddress);

        yield return new WaitUntil(() => sendTransaction.IsCompleted);

        if (sendTransaction.Exception != null) throw sendTransaction.Exception;
        
        Assert.IsTrue(sendTransaction.IsCompletedSuccessfully);

        Assert.AreEqual(sendTransaction.Result, config.TestResponse);
    }

    [UnityTest]
    public IEnumerator TestSha3()
    {
        var sha3 = Evm.Sha3("It’s dangerous to go alone, take this!");

        Assert.AreEqual(sha3, "45760485edafabcbb28570e7da5ddda4639abb4794de9af6de1d6669cbf220fc");

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSignMessage()
    {
        config.TestResponse =
            "0x87dfaa646f476ca53ba8b6e8d122839571e52866be0984ec0497617ad3e988b7401c6b816858df27625166cb98a688f99ba92fa593da3c86c78b19c78c1f51cc1c";

        var signMessage = Evm.SignMessage(web3,"The right man in the wrong place can make all the difference in the world.");

        yield return new WaitUntil(() => signMessage.IsCompleted);

        if (signMessage.Exception != null) throw signMessage.Exception;
        
        Assert.IsTrue(signMessage.IsCompletedSuccessfully);

        Assert.AreEqual(signMessage.Result, config.TestResponse);
    }

    [UnityTest]
    public IEnumerator TestSignVerify()
    {
        var signVerify = Evm.SignVerify(web3, "A man chooses, a slave obeys.");

        yield return new WaitUntil(() => signVerify.IsCompleted);

        if (signVerify.Exception != null) throw signVerify.Exception;
        
        Assert.IsTrue(signVerify.IsCompletedSuccessfully);

        Assert.AreEqual(signVerify.Result, true);
    }
    
    [UnityTest]
    public IEnumerator TestMintErc20()
    {
        config.TestResponse = "0xd3027fbfd9d5ddb5ea0ef75f5b128581d9268ad67728d150657f915c8910f9f0";

        var mint20 = Erc20.MintErc20(web3, Contracts.Erc20, Mint20ToAccount, Mint20Amount);

        yield return new WaitUntil(() => mint20.IsCompleted);

        if (mint20.Exception != null) throw mint20.Exception;
        
        Assert.IsTrue(mint20.IsCompletedSuccessfully);

        Assert.AreEqual(mint20.Result, string.Empty);
    }
    
    [UnityTest]
    public IEnumerator TestMintErc721()
    {
        config.TestResponse = "0xd3027fbfd9d5ddb5ea0ef75f5b128581d9268ad67728d150657f915c8910f9f0";

        var mint721 = Erc721.MintErc721(web3, ABI.Erc721, Contracts.Erc721);

        yield return new WaitUntil(() => mint721.IsCompleted);

        if (mint721.Exception != null) throw mint721.Exception;
        
        Assert.IsTrue(mint721.IsCompletedSuccessfully);

        Assert.AreEqual(mint721.Result, string.Empty);
    }
    
    [UnityTest]
    public IEnumerator TestMintErc1155()
    {
        config.TestResponse = "0xd3027fbfd9d5ddb5ea0ef75f5b128581d9268ad67728d150657f915c8910f9f0";

        var mint1155 = Erc1155.MintErc1155(web3, ABI.Erc1155, Contracts.Erc1155, Mint1155Id, Mint1155Amount);

        yield return new WaitUntil(() => mint1155.IsCompleted);

        if (mint1155.Exception != null) throw mint1155.Exception;
        
        Assert.IsTrue(mint1155.IsCompletedSuccessfully);

        Assert.AreEqual(mint1155.Result, string.Empty);
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc20()
    {
        config.TestResponse = "0xba90b6fb8cbee5fd0ad423cc74bb4a365bb88b260601933aac86b947945c5465";

        var transferErc20 = Erc20.TransferErc20(web3, Contracts.Erc20, SendToAddress, 1000000000000000);

        yield return new WaitUntil(() => transferErc20.IsCompleted);

        if (transferErc20.Exception != null) throw transferErc20.Exception;
        
        Assert.IsTrue(transferErc20.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc20.Result, new object[] { false });
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc721()
    {
        config.TestResponse = "0x0e292ae8c5ab005d87581f32fd791e1b18b0cfa944d6877b41edbdb740ee8586";

        var transferErc721 = Erc721.TransferErc721(web3, Contracts.Erc721, SendToAddress, 0);

        yield return new WaitUntil(() => transferErc721.IsCompleted);

        if (transferErc721.Exception != null) throw transferErc721.Exception;
        
        Assert.IsTrue(transferErc721.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc721.Result, string.Empty);
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc1155()
    {
        config.TestResponse = "0xb018a043ac0affe05159a53daa8656dbbad61c839eaf89622d7813226f222876";

        var transferErc1155 = Erc1155.TransferErc1155(web3, Contracts.Erc1155, 101, 1, SendToAddress);

        yield return new WaitUntil(() => transferErc1155.IsCompleted);

        if (transferErc1155.Exception != null) throw transferErc1155.Exception;
        
        yield return new WaitUntil(() => transferErc1155.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc1155.Result, string.Empty);
    }
}
