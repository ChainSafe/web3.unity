using System.Collections;
using System.Linq;
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

public class Erc721Tests : SampleTestsBase
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

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = web3.Erc721.GetBalanceOf(ChainSafeContracts.Erc721, balanceOfAccount);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);
        Assert.AreEqual(balanceOfExpected, getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOf()
    {
        var getOwnerOf = web3.Erc721.GetOwnerOf(ChainSafeContracts.Erc721, ownerOfTokenId);
        yield return new WaitUntil(() => getOwnerOf.IsCompleted);
        Assert.AreEqual(ownerOfExpected, getOwnerOf.Result);
    }

    [UnityTest]
    public IEnumerator TestOwnerOfBatch()
    {
        var getOwnerOfBatch = web3.Erc721.GetOwnerOfBatch(ChainSafeContracts.Erc721, ownerOfBatchTokenIds);
        yield return new WaitUntil(() => getOwnerOfBatch.IsCompleted);
        CollectionAssert.AreEqual(ownerOfBatchExpected, getOwnerOfBatch.Result.Select(x => x.Owner));
    }

    [UnityTest]
    public IEnumerator TestUri()
    {
        var uri = web3.Erc721.GetUri(ChainSafeContracts.Erc721, uriTokenId);
        yield return new WaitUntil(() => uri.IsCompleted);
        Assert.AreEqual(ExpectedUriResult, uri.Result);
    }
}