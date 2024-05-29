using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Web3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using Scripts.EVM.Token;
using Tests.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

public class EvmCustomResponseTests
{
    private const int IncreaseAmount = 2;
    private const string SendArrayMethod = "setStore";
    private const string ContractSendMethod = "addTotal";

    private const int Mint20Amount = 1;
    private const string Mint721Uri = "QmfUHuFj3YL2JMZkyXNtGRV8e9aLJgQ6gcSrqbfjWFvbqQ";


    private const int Mint1155Id = 1;
    private const int Mint1155Amount = 1;

    private const int Transfer721Id = 5;
    private const int Transfer1155Id = 1;
    private const int Transfer1155Amount = 1;

    private const string SendToAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    private BigInteger TransferErc20Amount = 1;

    private static readonly List<string> ArrayToSend = new()
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    private Web3 web3;

    [UnityTearDown]
    public virtual IEnumerator TearDown()
    {
        var terminateWeb3Task = web3.TerminateAsync();

        // Wait until for async task to finish
        yield return new WaitUntil(() => terminateWeb3Task.IsCompleted);
    }

    [UnityTest]
    public IEnumerator TestContractSend()
    {
        yield return BuildWeb3WithTestResponse("0x910f89945e46576ebd63419853c7d10d41a50590c59b7c55bd591bf41d43601e");
        object[] args =
        {
            IncreaseAmount
        };
        var sendContract = Evm.ContractSend(web3, ContractSendMethod, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        yield return new WaitUntil(() => sendContract.IsCompleted);
        if (sendContract.Exception != null) throw sendContract.Exception;
        Assert.IsTrue(sendContract.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, sendContract.Result);
    }

    [UnityTest]
    public IEnumerator TestSendArray()
    {
        yield return BuildWeb3WithTestResponse("0x3446b949c3d214fba7e61c9cf127eac6cd0b2983564cf76be618099879b6f1e1");
        var sendArray = Evm.SendArray(web3, SendArrayMethod, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, ArrayToSend.ToArray());
        yield return new WaitUntil(() => sendArray.IsCompleted);
        if (sendArray.Exception != null) throw sendArray.Exception;
        Assert.IsTrue(sendArray.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, sendArray.Result);
    }

    [UnityTest]
    public IEnumerator TestGetGasNonce()
    {
        yield return BuildWeb3WithTestResponse("0x04b6d6b467d4d07dd0d20c49516afedd1c4a17d4f13bc89b6e6c520890cea832");
        var getGasNonce = Evm.GetNonce(web3);
        yield return new WaitUntil(() => getGasNonce.IsCompleted);
        if (getGasNonce.Exception != null) throw getGasNonce.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasNonce.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestTransactionStatus()
    {
        yield return BuildWeb3WithTestResponse("0xaba88d9a1977c8d78ddfb3d973798eb061bd495189d7cbfa832f895896417cd1");
        var getTransactionStatus = Evm.GetTransactionStatus(web3);
        yield return new WaitUntil(() => getTransactionStatus.IsCompleted);
        if (getTransactionStatus.Exception != null) throw getTransactionStatus.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getTransactionStatus.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestSendTransaction()
    {
        const string testResponse = "0x3446b949c3d214fba7e61c9cf127eac6cd0b2983564cf76be618099879b6f1e1";
        yield return BuildWeb3WithTestResponse(testResponse);
        var sendTransaction = Evm.SendTransaction(web3, SendToAddress);
        yield return new WaitUntil(() => sendTransaction.IsCompleted);
        if (sendTransaction.Exception != null) throw sendTransaction.Exception;
        Assert.IsTrue(sendTransaction.IsCompletedSuccessfully);
        Assert.AreEqual(testResponse, sendTransaction.Result);
    }

    [UnityTest]
    public IEnumerator TestSignMessage()
    {
        const string testResponse = "0xda2880dde86b7d870ac9ddfa690010bed8a679350415f8e9cce02d87bac91651081c40455538bce7a18792e3f2ea56457183e8dba8568f3a92785ab0d743ce0b1b";
        yield return BuildWeb3WithTestResponse(testResponse);
        var signMessage = Evm.SignMessage(web3, "The right man in the wrong place can make all the difference in the world.");
        yield return new WaitUntil(() => signMessage.IsCompleted);
        if (signMessage.Exception != null) throw signMessage.Exception;
        Assert.IsTrue(signMessage.IsCompletedSuccessfully);
        Assert.AreEqual(testResponse, signMessage.Result);
    }

    [UnityTest]
    public IEnumerator TestSignVerify()
    {
        yield return BuildWeb3WithTestResponse(
            "0xda2880dde86b7d870ac9ddfa690010bed8a679350415f8e9cce02d87bac91651081c40455538bce7a18792e3f2ea56457183e8dba8568f3a92785ab0d743ce0b1b");
        var signVerify = Evm.SignVerify(web3, "A man chooses, a slave obeys.");
        yield return new WaitUntil(() => signVerify.IsCompleted);
        if (signVerify.Exception != null) throw signVerify.Exception;
        Assert.IsTrue(signVerify.IsCompletedSuccessfully);
        Assert.AreEqual(true, signVerify.Result);
    }

    [UnityTest]
    public IEnumerator TestMintErc20()
    {
        yield return BuildWeb3WithTestResponse("0xf6133ad76359ffaf67853a5eb138a94ed11f29d350b907420a92c685c6df5303");
        var mint20 = web3.Erc20.Mint(ChainSafeContracts.Erc20, Mint20Amount, SendToAddress);
        yield return new WaitUntil(() => mint20.IsCompleted);
        if (mint20.Exception != null) throw mint20.Exception;
        Assert.IsTrue(mint20.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, mint20.Result);
    }

    [UnityTest]
    public IEnumerator TestMintErc721()
    {
        yield return BuildWeb3WithTestResponse("0x09f1c615d638ae0b3a8c4a5555b46170c42dba214f04412400f3ff639657a223");
        var mint721 = web3.Erc721.Mint(ChainSafeContracts.Erc721, Mint721Uri);
        yield return new WaitUntil(() => mint721.IsCompleted);
        if (mint721.Exception != null) throw mint721.Exception;
        Assert.IsTrue(mint721.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, mint721.Result);
    }

    [UnityTest]
    public IEnumerator TestMintErc1155()
    {
        yield return BuildWeb3WithTestResponse("0xa04294541b934b48ada4073b07ba01492d8ad676aa2db6f93249cec0820a1dca");
        var mint1155 = web3.Erc1155.Mint(ChainSafeContracts.Erc1155, Mint1155Id, Mint1155Amount);
        yield return new WaitUntil(() => mint1155.IsCompleted);
        if (mint1155.Exception != null) throw mint1155.Exception;
        Assert.IsTrue(mint1155.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, mint1155.Result);
    }

    [UnityTest]
    public IEnumerator TestTransferErc20()
    {
        yield return BuildWeb3WithTestResponse("0x87d8826e895247b4106596040c5133a18ecbf76077c5433091a5f18c355a120b");
        var transferErc20 = web3.Erc20.Transfer(ChainSafeContracts.Erc20, SendToAddress, TransferErc20Amount);
        yield return new WaitUntil(() => transferErc20.IsCompleted);
        if (transferErc20.Exception != null) throw transferErc20.Exception;
        Assert.IsTrue(transferErc20.IsCompletedSuccessfully);
        Assert.AreEqual(new object[] { false }, transferErc20.Result);
    }

    [UnityTest]
    public IEnumerator TestTransferErc721()
    {
        yield return BuildWeb3WithTestResponse("0xba034c4150f2a5fd50926551a8e95028d51dcc91e3c3b566bbd316968bc29375");
        var transferErc721 = web3.Erc721.Transfer(ChainSafeContracts.Erc721, SendToAddress, Transfer721Id);
        yield return new WaitUntil(() => transferErc721.IsCompleted);
        if (transferErc721.Exception != null) throw transferErc721.Exception;
        Assert.IsTrue(transferErc721.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, transferErc721.Result);
    }

    [UnityTest]
    public IEnumerator TestTransferErc1155()
    {
        yield return BuildWeb3WithTestResponse("0x390b47d378e9a6de830e2cc6d624de0920efc44d7b40fb61f75d983545c987fc");
        var transferErc1155 = web3.Erc1155.Transfer(ChainSafeContracts.Erc1155, Transfer1155Id, Transfer1155Amount, SendToAddress);
        yield return new WaitUntil(() => transferErc1155.IsCompleted);
        if (transferErc1155.Exception != null) throw transferErc1155.Exception;
        yield return new WaitUntil(() => transferErc1155.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, transferErc1155.Result);
    }

    private IEnumerator BuildWeb3WithTestResponse(string testResponse)
    {
        var buildWeb3Task = SampleTestsBase.BuildTestWeb3(services =>
        {
            services.Replace(ServiceDescriptor.Singleton(new StubWalletConnectProviderConfig
            { StubResponse = testResponse }));
        });

        // Wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3Task.IsCompleted);

        // Assign result to web3
        web3 = buildWeb3Task.Result;
    }
}