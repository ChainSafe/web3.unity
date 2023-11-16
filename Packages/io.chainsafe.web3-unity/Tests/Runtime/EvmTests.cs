using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Web3;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class EvmTests : SampleTestsBase
{
    #region Fields
    
    #region ContractCalls

    private const string ContractSendMethod = "addTotal";
    private const int IncreaseAmount = 2;
    private const string ContractCallMethod = "myTotal";
    private const string CallAmount = "1";

    #endregion

    #region Array

	private const string GetArrayMethod = "getStore";
    private const string SendArrayMethod = "setStore";

    private static List<string> ArrayToSend = new List<string>()
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region MintErc20
    
    private const int Mint20Amount = 1;

    #endregion
    
    #region Mint721

    private const string Mint721Uri = "QmfUHuFj3YL2JMZkyXNtGRV8e9aLJgQ6gcSrqbfjWFvbqQ";

    #endregion
    
    #region Mint1155
    
    private const int Mint1155Id = 1;
    private const int Mint1155Amount = 1;

    #endregion
    
    #region Transfer
    
    private BigInteger TransferErc20Amount = 1;
    
    private const int Transfer721Id = 5;
    private const int Transfer1155Id = 1;
    private const int Transfer1155Amount = 1;
    
    private const string SendToAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    #endregion
    
    #region ECDSA

    private string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    private string ecdsaMessage = "This is a test message";
    private string transactionHash = "0x123456789";
    private string chainId ="11155111";

    #endregion

    #endregion
    
    private Web3 web3;
    
    [UnitySetUp]
    public override IEnumerator Setup()
    {
        yield return base.Setup();
        web3 = web3Result;
    }
    
    [UnityTest]
    public IEnumerator TestContractCall()
    {
        object[] args = { config.TestWalletAddress };
        var callContract = Evm.ContractCall(web3, ContractCallMethod, ABI.ArrayTotal, Contracts.ArrayTotal, args);
        yield return new WaitUntil(() => callContract.IsCompleted);
        if (callContract.Exception != null) throw callContract.Exception;
        Assert.IsTrue(callContract.IsCompletedSuccessfully);
        Assert.AreEqual(CallAmount, callContract.Result[0].ToString());
    }

    [UnityTest]
    public IEnumerator TestContractSend()
    {
        config.TestResponse = "0x324080652dfe1463f0fcbde18961a6e7eee87f2231133523e96dc52ce8239d3f";
        object[] args =
        {
            IncreaseAmount
        };
        var sendContract = Evm.ContractSend(web3, ContractSendMethod, ABI.ArrayTotal, Contracts.ArrayTotal, args);
        yield return new WaitUntil(() => sendContract.IsCompleted);
        if (sendContract.Exception != null) throw sendContract.Exception;
        Assert.IsTrue(sendContract.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, sendContract.Result);
    }

	[UnityTest]
    public IEnumerator TestGetArray()
    {
        var getArray = Evm.GetArray(web3, Contracts.ArrayTotal, ABI.ArrayTotal, GetArrayMethod);
        yield return new WaitUntil(() => getArray.IsCompleted);
        // Convert toLower to make comparing easier
        var result = getArray.Result.ConvertAll(a => a.ConvertAll(b => b.ToLower()));
        Assert.AreEqual(result, new List<List<string>>
        {
            ArrayToSend.ConvertAll(a => a.ToLower())
        });
    }
    
    [UnityTest]
    public IEnumerator TestSendArray()
    {
        config.TestResponse = "0x3446b949c3d214fba7e61c9cf127eac6cd0b2983564cf76be618099879b6f1e1";
        var sendArray = Evm.SendArray(web3, SendArrayMethod, ABI.ArrayTotal, Contracts.ArrayTotal, ArrayToSend.ToArray());
        yield return new WaitUntil(() => sendArray.IsCompleted);
        if (sendArray.Exception != null) throw sendArray.Exception;
        Assert.IsTrue(sendArray.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, sendArray.Result);
    }

    [UnityTest]
    public IEnumerator TestGetBlockNumber()
    {
        var getBlockNumber = Evm.GetBlockNumber(web3);
        yield return new WaitUntil(() => getBlockNumber.IsCompleted);
        if (getBlockNumber.Exception != null) throw getBlockNumber.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getBlockNumber.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasLimit()
    {
        object[] args =
        {
           IncreaseAmount
        };
        var getGasLimit = Evm.GetGasLimit(web3, ABI.ArrayTotal, Contracts.ArrayTotal, ContractSendMethod, args);
        yield return new WaitUntil(() => getGasLimit.IsCompleted);
        if (getGasLimit.Exception != null) throw getGasLimit.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasLimit.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = Evm.GetGasPrice(web3);
        yield return new WaitUntil(() => getGasPrice.IsCompleted);
        if (getGasPrice.Exception != null) throw getGasPrice.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasPrice.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasNonce()
    {
        config.TestResponse = "0xaba88d9a1977c8d78ddfb3d973798eb061bd495189d7cbfa832f895896417cd1";
        var getGasNonce = Evm.GetNonce(web3);
        yield return new WaitUntil(() => getGasNonce.IsCompleted);
        if (getGasNonce.Exception != null) throw getGasNonce.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasNonce.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestTransactionStatus()
    {
        config.TestResponse = "0xaba88d9a1977c8d78ddfb3d973798eb061bd495189d7cbfa832f895896417cd1";
        var getTransactionStatus = Evm.GetTransactionStatus(web3);
        yield return new WaitUntil(() => getTransactionStatus.IsCompleted);
        if (getTransactionStatus.Exception != null) throw getTransactionStatus.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getTransactionStatus.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestUseRegisteredContract()
    {
        var useRegisteredContract = Evm.UseRegisteredContract(web3, "CsTestErc20", EthMethod.BalanceOf);
        yield return new WaitUntil(() => useRegisteredContract.IsCompleted);
        if (useRegisteredContract.Exception != null) throw useRegisteredContract.Exception;
        Assert.IsTrue(useRegisteredContract.IsCompletedSuccessfully);
        Assert.AreEqual(new BigInteger(999999999999999), useRegisteredContract.Result);
    }

    [UnityTest]
    public IEnumerator TestSendTransaction()
    {
        config.TestResponse = "0x3446b949c3d214fba7e61c9cf127eac6cd0b2983564cf76be618099879b6f1e1";
        var sendTransaction = Evm.SendTransaction(web3, SendToAddress);
        yield return new WaitUntil(() => sendTransaction.IsCompleted);
        if (sendTransaction.Exception != null) throw sendTransaction.Exception;
        Assert.IsTrue(sendTransaction.IsCompletedSuccessfully);
        Assert.AreEqual(config.TestResponse, sendTransaction.Result);
    }

    [UnityTest]
    public IEnumerator TestSha3()
    {
        var sha3 = Evm.Sha3("It’s dangerous to go alone, take this!");
        Assert.AreEqual("45760485edafabcbb28570e7da5ddda4639abb4794de9af6de1d6669cbf220fc", sha3);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSignMessage()
    {
        config.TestResponse =
            "0xda2880dde86b7d870ac9ddfa690010bed8a679350415f8e9cce02d87bac91651081c40455538bce7a18792e3f2ea56457183e8dba8568f3a92785ab0d743ce0b1b";
        var signMessage = Evm.SignMessage(web3,"The right man in the wrong place can make all the difference in the world.");
        yield return new WaitUntil(() => signMessage.IsCompleted);
        if (signMessage.Exception != null) throw signMessage.Exception;
        Assert.IsTrue(signMessage.IsCompletedSuccessfully);
        Assert.AreEqual(config.TestResponse, signMessage.Result);
    }

    [UnityTest]
    public IEnumerator TestSignVerify()
    {
        config.TestResponse =
            "0xda2880dde86b7d870ac9ddfa690010bed8a679350415f8e9cce02d87bac91651081c40455538bce7a18792e3f2ea56457183e8dba8568f3a92785ab0d743ce0b1b";
        var signVerify = Evm.SignVerify(web3, "A man chooses, a slave obeys.");
        yield return new WaitUntil(() => signVerify.IsCompleted);
        if (signVerify.Exception != null) throw signVerify.Exception;
        Assert.IsTrue(signVerify.IsCompletedSuccessfully);
        Assert.AreEqual(true, signVerify.Result);
    }
    
    [UnityTest]
    public IEnumerator TestECDSASignTx()
    {
        var signTxECDSA = Evm.EcdsaSignTransaction(ecdsaKey, transactionHash, chainId);
        Assert.AreEqual("0xf8cd1c5dfa0706767c7ec81b91bed9a05b65b9c5b47769206a594377c5969a9a47fe21f7328b5865815f97b7ddd09bd7b899879aac6ed7c01eda5380975a54dc01546d72", signTxECDSA);
        return null;
    }
    
    [UnityTest]
    public IEnumerator TestECDSASign()
    {
        var signECDSA = Evm.EcdsaSignMessage(ecdsaKey, ecdsaMessage);
        Assert.AreEqual("0x90bd386a185bdc5cbb13b9bba442e35036ac8e92792e74e385abbf7d9546be8720879c2075bf59124d9114b4d432173a4d6c4b118d278c3ffb51a140a625c66c1b", signECDSA);
        return null;
    }
    
    [UnityTest]
    public IEnumerator TestECDSAAddress()
    {
        var address = Evm.EcdsaGetAddress(ecdsaKey);
        Assert.AreEqual("0x428066dd8A212104Bc9240dCe3cdeA3D3A0f7979", address);
        return null;
    }
    
    [UnityTest]
    public IEnumerator TestCustomBalanceOfErc20()
    {
        var getCustomBalanceOf = Erc20.CustomTokenBalance(web3, ABI.Erc20, Contracts.Erc20);
        yield return new WaitUntil(() => getCustomBalanceOf.IsCompleted);
        Assert.AreEqual(new BigInteger(999999999999999), getCustomBalanceOf.Result);
    }
    
    [UnityTest]
    public IEnumerator TestMintErc20()
    {
        config.TestResponse = "0xf6133ad76359ffaf67853a5eb138a94ed11f29d350b907420a92c685c6df5303";
        var mint20 = Erc20.MintErc20(web3, Contracts.Erc20, SendToAddress, Mint20Amount);
        yield return new WaitUntil(() => mint20.IsCompleted);
        if (mint20.Exception != null) throw mint20.Exception;
        Assert.IsTrue(mint20.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, mint20.Result);
    }
    
    [UnityTest]
    public IEnumerator TestMintErc721()
    {
        config.TestResponse = "0x09f1c615d638ae0b3a8c4a5555b46170c42dba214f04412400f3ff639657a223";
        var mint721 = Erc721.MintErc721(web3, ABI.Erc721, Contracts.Erc721, Mint721Uri);
        yield return new WaitUntil(() => mint721.IsCompleted);
        if (mint721.Exception != null) throw mint721.Exception;
        Assert.IsTrue(mint721.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, mint721.Result);
    }
    
    [UnityTest]
    public IEnumerator TestMintErc1155()
    {
        config.TestResponse = "0xa04294541b934b48ada4073b07ba01492d8ad676aa2db6f93249cec0820a1dca";
        var mint1155 = Erc1155.MintErc1155(web3, ABI.Erc1155, Contracts.Erc1155, Mint1155Id, Mint1155Amount);
        yield return new WaitUntil(() => mint1155.IsCompleted);
        if (mint1155.Exception != null) throw mint1155.Exception;
        Assert.IsTrue(mint1155.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, mint1155.Result);
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc20()
    {
        config.TestResponse = "0x87d8826e895247b4106596040c5133a18ecbf76077c5433091a5f18c355a120b";
        var transferErc20 = Erc20.TransferErc20(web3, Contracts.Erc20, SendToAddress, TransferErc20Amount);
        yield return new WaitUntil(() => transferErc20.IsCompleted);
        if (transferErc20.Exception != null) throw transferErc20.Exception;
        Assert.IsTrue(transferErc20.IsCompletedSuccessfully);
        Assert.AreEqual(new object[] { false }, transferErc20.Result);
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc721()
    {
        config.TestResponse = "0x5c02df6b55bbc5f958d3a5e4b21f0db20cd2284fabe1f7278f7984d3e05de81d";
        var transferErc721 = Erc721.TransferErc721(web3, Contracts.Erc721, SendToAddress, Transfer721Id);
        yield return new WaitUntil(() => transferErc721.IsCompleted);
        if (transferErc721.Exception != null) throw transferErc721.Exception;
        Assert.IsTrue(transferErc721.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, transferErc721.Result);
    }
    
    [UnityTest]
    public IEnumerator TestTransferErc1155()
    {
        config.TestResponse = "0x390b47d378e9a6de830e2cc6d624de0920efc44d7b40fb61f75d983545c987fc";
        var transferErc1155 = Erc1155.TransferErc1155(web3, Contracts.Erc1155, Transfer1155Id, Transfer1155Amount, SendToAddress);
        yield return new WaitUntil(() => transferErc1155.IsCompleted);
        if (transferErc1155.Exception != null) throw transferErc1155.Exception;
        yield return new WaitUntil(() => transferErc1155.IsCompletedSuccessfully);
        Assert.AreEqual(string.Empty, transferErc1155.Result);
    }
}
