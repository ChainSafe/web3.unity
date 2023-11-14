using System.Collections;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class Erc721Tests
{
    #region Fields
    
    #region Balances

    private const string balanceOfAccount = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private static int balanceOfExpected = 3;

    #endregion

    #region OwnerOf

    private const string ownerOfTokenId = "2";
    private const string ownerOfExpected = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private static string[] ownerOfBatchTokenIds = { "4", "5" };
    private static string[] ownerOfBatchExpected = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44" };

    #endregion

    #region Uri

    private const string uriTokenId = "1";
    private const string ExpectedUriResult =
        "https://ipfs.chainsafe.io/ipfs/QmfUHuFj3YL2JMZkyXNtGRV8e9aLJgQ6gcSrqbfjWFvbqQ";
    private const string NftTextureContractAddress = "0x0288B4F1389ED7b3d3f9C7B73d4408235c0CBbc6";

    #endregion
    
    #endregion
    
    private Web3 web3;

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
        // Wait for some time to initialize
        yield return new WaitForSeconds(5f);
    
        // Set project config, fallback is for github as it doesn't load
        var projectConfigScriptableObject = ProjectConfigUtilities.Load();
        if (projectConfigScriptableObject == null)
        {
            projectConfigScriptableObject = ProjectConfigUtilities.Load("3dc3e125-71c4-4511-a367-e981a6a94371", "11155111",
                "Ethereum", "Sepolia", "Seth", "https://sepolia.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f");
        }
        
        // Create web3builder & assign services
        var web3Builder = new Web3Builder(projectConfigScriptableObject)
                .Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseRpcProvider();
            services.UseMultiCall();
        });
    
        var buildWeb3 = web3Builder.LaunchAsync();
    
        // Wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);
        
        // Assign result to web3
        web3 = buildWeb3.Result;
    }

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = Erc721.BalanceOf(web3, Contracts.Erc721, balanceOfAccount);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);
        Assert.AreEqual(balanceOfExpected, getBalanceOf.Result);
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
        var getOwnerOfBatch = Erc721.OwnerOfBatch(web3, Contracts.Erc721, ownerOfBatchTokenIds);
        yield return new WaitUntil(() => getOwnerOfBatch.IsCompleted);
        CollectionAssert.AreEqual(ownerOfBatchExpected, getOwnerOfBatch.Result);
    }

    [UnityTest]
    public IEnumerator TestUri()
    {
        var uri = Erc721.Uri(web3, Contracts.Erc721, uriTokenId);
        yield return new WaitUntil(() => uri.IsCompleted);
        Assert.AreEqual(ExpectedUriResult, uri.Result);
    }
}