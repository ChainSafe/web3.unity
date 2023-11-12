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
	//public const string Erc721 = "0xF7B58448FEFdC634ccFE1624Dc9356C55b8d126c";
    #region Balances

    private const string balanceOfContractAddress = "0xE83EE42471Ad261Cf3b9221006925d8F1FC01ee9";
    private const string balanceOfAccount = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region OwnerOf

    private const string ownerOfContractAddress = "0xE83EE42471Ad261Cf3b9221006925d8F1FC01ee9";
    private const string ownerOfTokenId = "2";
    private const string ownerOfExpected = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private const string ownerOfBatchContractAddress = "0xE83EE42471Ad261Cf3b9221006925d8F1FC01ee9";
    private string[] ownerOfBatchTokenIds = { "1", "2" };
	public string multicall = "";
    //public string multicall = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";
    private string[] ownerOfBatchExpected = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2" };

    #endregion
    
    #region AllNfts
    
    private const string allContractAddress = "0x2c1867BC3026178A47a677513746DCc6822A137A";
    
    #endregion

    #region Uri
    
    private const string uriContractAddress = "0xE83EE42471Ad261Cf3b9221006925d8F1FC01ee9";
    private const string uriTokenId = "1";

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
        var getBalanceOf = Erc721.BalanceOf(web3, balanceOfContractAddress, balanceOfAccount);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);

        Assert.AreEqual(3, getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOf()
    {
        var getOwnerOf = Erc721.OwnerOf(web3, ownerOfContractAddress, ownerOfTokenId);
        yield return new WaitUntil(() => getOwnerOf.IsCompleted);

        Assert.AreEqual(ownerOfExpected, getOwnerOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOfBatch()
    {
        var getOwnerOfBatch = Erc721.OwnerOfBatch(web3, ownerOfBatchContractAddress, ownerOfBatchTokenIds, multicall);
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