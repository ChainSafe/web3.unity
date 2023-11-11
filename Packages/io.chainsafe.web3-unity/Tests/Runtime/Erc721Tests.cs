using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;
using Web3Unity.Scripts.Prefabs;

public class Erc721Tests
{
    // Fields
    #region Balances
    
    private const string balanceOfAccount = "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44";

    #endregion

    #region OwnerOf

    private const string ownerOfContractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private BigInteger ownerOfTokenId = 1;
    private BigInteger ownerOfExpected = 1;
    private string[] ownerOfBatchTokenIds = { "1", "2" };
    public string multicall = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";
    private string[] ownerOfBatchExpected = { "0x5e8caec3a556cbb8a3cb689560ea811bdddc8d90", "0x80cc9a67ead304bbb3b6cfca804773ef51da872c" };

    #endregion
    
    #region AllNfts
    
    private const string allContractAddress = "0x2c1867BC3026178A47a677513746DCc6822A137A";
    
    #endregion

    #region Uri
    
    private const string uriContractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private const string uriTokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion
    
    private WalletConnectConfig config;
    private Web3 web3;

    private const string NftTextureContractAddress = "0x0288B4F1389ED7b3d3f9C7B73d4408235c0CBbc6";

    #region Indexer Test Parameters

    private const string IndexerChain = "ethereum";
    private const string IndexerNetwork = "goerli"; // mainnet goerli
    private const string IndexerAccount = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
    private const string IndexerContract = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private const int IndexerTake = 500;
    private const int IndexerSkip = 0;

    #endregion

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //wait for some time to initialize
        yield return new WaitForSeconds(5f);
    
        //For whatever reason, in github this won't load
        var projectConfigScriptableObject = ProjectConfigUtilities.Load();
        if (projectConfigScriptableObject == null)
        {
            projectConfigScriptableObject = ProjectConfigUtilities.Load("3dc3e125-71c4-4511-a367-e981a6a94371", "11155111",
                "Ethereum", "Sepolia", "Seth", "https://sepolia.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f");
        }
    
        var web3Builder = new Web3Builder(projectConfigScriptableObject)
                .Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseRpcProvider();
        });
    
        var buildWeb3 = web3Builder.LaunchAsync();
    
        //wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);
    
        web3 = buildWeb3.Result;
    }

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = Erc721.BalanceOf(web3, Contracts.Erc721, balanceOfAccount);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);

        Assert.AreEqual("1", getBalanceOf.Result.ToString());
    }

    [UnityTest]
    public IEnumerator TestOwnerOf()
    {
        var getOwnerOf = Erc721.OwnerOf(web3, Contracts.Erc721, ownerOfTokenId);
        yield return new WaitUntil(() => getOwnerOf.IsCompleted);

        Assert.AreEqual(ownerOfExpected, getOwnerOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOfBatch()
    {
        var getOwnerOfBatch = Erc721.OwnerOfBatch(web3, Contracts.Erc721, ownerOfBatchTokenIds, multicall);
        yield return new WaitUntil(() => getOwnerOfBatch.IsCompleted);

        CollectionAssert.AreEqual(ownerOfBatchExpected, getOwnerOfBatch.Result);
    }

    private const string ExpectedUriResult =
        "https://ipfs.chainsafe.io/ipfs/QmfUHuFj3YL2JMZkyXNtGRV8e9aLJgQ6gcSrqbfjWFvbqQ";

    [UnityTest]
    public IEnumerator TestUri()
    {
        var uri = Erc721.Uri(web3, uriContractAddress, uriTokenId);
        yield return new WaitUntil(() => uri.IsCompleted);
        Assert.AreEqual(ExpectedUriResult, uri.Result);
    }
}