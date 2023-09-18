using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Web3Unity.Scripts.Prefabs;

public class Erc721Tests
{
    private const string balanceOfContractAddress = "0x9123541E259125657F03D7AD2A7D1a8Ec79375BA";
    private const string balanceOfAccount = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";


    private const string ownerOfContractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private const string ownerOfTokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";
    private const string ownerOfExpected = "0x7Ea535884Ba8342bE7aDbef0Fa6822E629162375";

    private const string ownerOfBatchContractAddress = "0x47381c5c948254e6e0E324F1AA54b7B24104D92D";
    private string[] ownerOfBatchTokenIds = { "33", "29" };
    public string multicall = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";

    private string[] ownerOfBatchExpected =
        { "0x5e8caec3a556cbb8a3cb689560ea811bdddc8d90", "0x80cc9a67ead304bbb3b6cfca804773ef51da872c" };

    private const string allContractAddress = "0x2c1867BC3026178A47a677513746DCc6822A137A";

    private const string uriContractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private const string uriTokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";
    private Erc721Sample _logic;

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
            projectConfigScriptableObject = ProjectConfigUtilities.Load("3dc3e125-71c4-4511-a367-e981a6a94371", "5",
                "Ethereum", "Goerli", "Geth", "https://goerli.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f");
        }

        var web3Builder = new Web3Builder(projectConfigScriptableObject).Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseRpcProvider();
        });

        var buildWeb3 = web3Builder.BuildAsync();

        //wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);

        _logic = new Erc721Sample(buildWeb3.Result);
    }

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = _logic.BalanceOf(balanceOfContractAddress, balanceOfAccount);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);

        Assert.AreEqual(2, getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOf()
    {
        var getOwnerOf = _logic.OwnerOf(ownerOfContractAddress, ownerOfTokenId);
        yield return new WaitUntil(() => getOwnerOf.IsCompleted);

        Assert.AreEqual(ownerOfExpected, getOwnerOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOfBatch()
    {
        var getOwnerOfBatch = _logic.OwnerOfBatch(ownerOfBatchContractAddress, ownerOfBatchTokenIds, multicall);
        yield return new WaitUntil(() => getOwnerOfBatch.IsCompleted);

        CollectionAssert.AreEqual(ownerOfBatchExpected, getOwnerOfBatch.Result);
    }

    private const string ExpectedUriResult =
        "https://ipfs.io/ipfs/f01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    [UnityTest]
    public IEnumerator TestUri()
    {
        var uri = _logic.Uri(uriContractAddress, uriTokenId);
        yield return new WaitUntil(() => uri.IsCompleted);
        Assert.AreEqual(ExpectedUriResult, uri.Result);
    }
}