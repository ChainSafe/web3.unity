using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class Erc1155Tests
{
    #region Fields

    #region Balances

    private static string[] _accounts = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a" };
    private static string[] _tokenIds = { "1", "2" };

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
        var getBalanceOf = Erc1155.BalanceOf(web3, Contracts.Erc1155, _accounts[0], _tokenIds[0]);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);
        Assert.AreEqual(new BigInteger(2), getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestBalanceOfBatch()
    {
        var getBalanceOf = Erc1155.BalanceOfBatch(web3, Contracts.Erc1155, _accounts, _tokenIds);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);
        CollectionAssert.AreEqual(new List<BigInteger> { 2, 3 }, getBalanceOf.Result);
    }

    private const string ExpectedUriResult =
        "https://ipfs.chainsafe.io/ipfs/QmfUHuFj3YL2JMZkyXNtGRV8e9aLJgQ6gcSrqbfjWFvbqQ";

    [UnityTest]
    public IEnumerator TestUri()
    {
        var uri = Erc1155.Uri(web3, Contracts.Erc1155, _tokenIds[0]);
        yield return new WaitUntil(() => uri.IsCompleted);
        Assert.AreEqual(ExpectedUriResult, uri.Result);
    }

    //Something is really off with the indexer, since some values seem to be null (on SampleMain scene),
    //So I can't run this test until the root cause is fixed (or the All button is removed :D )
    /*[UnityTest]
    public IEnumerator TestIndexer()
    {
        var allNfts = _logic.All(IndexerChain, IndexerNetwork, IndexerAccount, IndexerContract, IndexerTake,
            IndexerSkip); 
        
        yield return new WaitUntil(() => allNfts.IsCompleted);
        /*var output = string.Join(",\n", allNfts.Result.Where(x => x != null).Select(x => x.ToString()));
        Debug.Log(output);#1#
        
    }*/

    [UnityTest]
    public IEnumerator TestImportNFTTexture()
    {
        //only way to compare this, unfortunately.
        #region Bytes Of The Texture

        byte[] bytesOfTheTexture = {
            255, 216, 255, 224, 0, 16, 74, 70, 73, 70, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 255, 219, 0, 131, 16, 3, 32, 2, 38,
            2, 88, 2, 188, 2, 88, 1, 244, 3, 32, 2, 188, 2, 138, 2, 188, 3, 132, 3, 82, 3, 32, 3, 182, 4, 176, 7, 208,
            5, 20, 4, 176, 4, 76, 4, 76, 4, 176, 9, 146, 6, 214, 7, 58, 5, 170, 7, 208, 11, 84, 9, 246, 11, 234, 11,
            184, 11, 34, 9, 246, 10, 240, 10, 190, 12, 128, 14, 16, 17, 248, 15, 60, 12, 128, 13, 72, 16, 254, 13, 122,
            10, 190, 10, 240, 15, 160, 21, 74, 15, 210, 16, 254, 18, 142, 19, 36, 20, 30, 20, 80, 20, 30, 12, 28, 15,
            10, 22, 18, 23, 162, 21, 224, 19, 136, 23, 112, 17, 248, 19, 186, 20, 30, 19, 86, 255, 219, 0, 131, 17, 3,
            82, 3, 132, 3, 132, 4, 176, 4, 26, 4, 176, 9, 46, 5, 20, 5, 20, 9, 46, 19, 86, 12, 228, 10, 240, 12, 228,
            19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19,
            86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86,
            19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19,
            86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 19, 86, 255, 193, 0, 17, 8, 0, 156, 0,
            156, 3, 1, 34, 0, 2, 17, 1, 3, 17, 1, 255, 196, 0, 31, 0, 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 255, 196, 0, 181, 16, 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 125,
            1, 2, 3, 0, 4, 17, 5, 18, 33, 49, 65, 6, 19, 81, 97, 7, 34, 113, 20, 50, 129, 145, 161, 8, 35, 66, 177, 193,
            21, 82, 209, 240, 36, 51, 98, 114, 130, 9, 10, 22, 23, 24, 25, 26, 37, 38, 39, 40, 41, 42, 52, 53, 54, 55,
            56, 57, 58, 67, 68, 69, 70, 71, 72, 73, 74, 83, 84, 85, 86, 87, 88, 89, 90, 99, 100, 101, 102, 103, 104,
            105, 106, 115, 116, 117, 118, 119, 120, 121, 122, 131, 132, 133, 134, 135, 136, 137, 138, 146, 147, 148,
            149, 150, 151, 152, 153, 154, 162, 163, 164, 165, 166, 167, 168, 169, 170, 178, 179, 180, 181, 182, 183,
            184, 185, 186, 194, 195, 196, 197, 198, 199, 200, 201, 202, 210, 211, 212, 213, 214, 215, 216, 217, 218,
            225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 255,
            196, 0, 31, 1, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 255,
            196, 0, 181, 17, 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 119, 0, 1, 2, 3, 17, 4, 5, 33, 49, 6, 18, 65,
            81, 7, 97, 113, 19, 34, 50, 129, 8, 20, 66, 145, 161, 177, 193, 9, 35, 51, 82, 240, 21, 98, 114, 209, 10,
            22, 36, 52, 225, 37, 241, 23, 24, 25, 26, 38, 39, 40, 41, 42, 53, 54, 55, 56, 57, 58, 67, 68, 69, 70, 71,
            72, 73, 74, 83, 84, 85, 86, 87, 88, 89, 90, 99, 100, 101, 102, 103, 104, 105, 106, 115, 116, 117, 118, 119,
            120, 121, 122, 130, 131, 132, 133, 134, 135, 136, 137, 138, 146, 147, 148, 149, 150, 151, 152, 153, 154,
            162, 163, 164, 165, 166, 167, 168, 169, 170, 178, 179, 180, 181, 182, 183, 184, 185, 186, 194, 195, 196,
            197, 198, 199, 200, 201, 202, 210, 211, 212, 213, 214, 215, 216, 217, 218, 226, 227, 228, 229, 230, 231,
            232, 233, 234, 242, 243, 244, 245, 246, 247, 248, 249, 250, 255, 218, 0, 12, 3, 1, 0, 2, 17, 3, 17, 0, 63,
            0, 74, 40, 162, 128, 10, 40, 162, 128, 10, 49, 74, 5, 45, 0, 38, 40, 197, 25, 164, 205, 0, 58, 138, 109, 20,
            0, 234, 41, 180, 102, 128, 23, 20, 152, 165, 205, 25, 160, 4, 162, 157, 138, 105, 20, 0, 81, 69, 20, 0, 81,
            69, 20, 0, 81, 69, 20, 0, 83, 128, 160, 82, 19, 64, 1, 52, 148, 81, 64, 5, 20, 81, 64, 5, 20, 81, 64, 5, 20,
            81, 64, 5, 20, 81, 64, 10, 13, 45, 54, 128, 113, 64, 10, 69, 37, 58, 154, 69, 0, 20, 81, 69, 0, 20, 160, 82,
            10, 117, 0, 33, 52, 148, 81, 64, 5, 20, 82, 129, 64, 9, 74, 5, 45, 20, 128, 49, 77, 34, 150, 150, 128, 27,
            69, 41, 20, 148, 192, 40, 162, 138, 0, 40, 162, 138, 0, 1, 167, 83, 105, 69, 0, 37, 20, 166, 146, 128, 20,
            80, 104, 29, 41, 15, 90, 0, 40, 162, 138, 0, 119, 74, 40, 61, 40, 164, 1, 72, 77, 33, 52, 83, 1, 65, 165,
            164, 2, 150, 144, 5, 20, 82, 119, 160, 0, 138, 74, 117, 29, 105, 128, 218, 41, 72, 164, 160, 2, 129, 69, 20,
            0, 234, 109, 40, 233, 72, 122, 208, 3, 169, 180, 234, 109, 0, 20, 81, 69, 0, 56, 244, 162, 131, 210, 138,
            64, 54, 138, 40, 166, 3, 168, 20, 82, 125, 105, 0, 19, 73, 154, 118, 41, 164, 83, 1, 65, 165, 166, 210, 131,
            64, 11, 72, 104, 205, 33, 57, 160, 2, 138, 40, 160, 5, 20, 99, 52, 10, 90, 0, 41, 180, 234, 105, 235, 64, 5,
            20, 81, 64, 14, 164, 160, 26, 90, 64, 33, 20, 148, 234, 40, 1, 1, 165, 164, 34, 147, 165, 48, 29, 69, 32,
            52, 180, 128, 66, 41, 49, 78, 162, 128, 27, 69, 41, 20, 148, 192, 40, 162, 138, 0, 81, 75, 72, 58, 80, 104,
            0, 29, 40, 52, 130, 148, 208, 2, 81, 69, 20, 0, 81, 154, 40, 160, 5, 6, 150, 155, 64, 52, 0, 234, 58, 209,
            154, 41, 0, 132, 82, 10, 117, 20, 192, 40, 162, 138, 64, 20, 134, 140, 210, 19, 154, 96, 20, 81, 74, 40, 1,
            105, 167, 173, 41, 233, 73, 64, 5, 56, 83, 104, 7, 20, 0, 164, 82, 83, 169, 164, 98, 128, 10, 40, 162, 128,
            10, 40, 162, 128, 10, 80, 105, 40, 160, 7, 117, 162, 155, 70, 104, 1, 217, 166, 147, 69, 20, 0, 81, 69, 20,
            0, 83, 169, 0, 160, 154, 0, 67, 69, 20, 80, 1, 69, 20, 80, 2, 131, 75, 77, 160, 26, 0, 8, 162, 157, 73, 138,
            0, 74, 40, 197, 20, 0, 81, 69, 20, 0, 81, 69, 20, 0, 81, 69, 46, 40, 1, 41, 64, 165, 164, 38, 128, 2, 113,
            73, 69, 20, 0, 81, 69, 20, 0, 81, 69, 20, 0, 81, 69, 20, 0, 82, 230, 146, 138, 0, 117, 38, 41, 40, 205, 0,
            46, 40, 197, 3, 154, 90, 0, 76, 81, 138, 90, 76, 208, 2, 210, 102, 147, 52, 80, 1, 154, 40, 162, 128, 10,
            40, 162, 128, 10, 40, 162, 128, 63, 255, 217
        };

        #endregion
        var texture = Erc1155.ImportNftTexture1155(web3, Contracts.Erc1155, "1");
        yield return new WaitUntil(() => texture.IsCompleted);
        CollectionAssert.AreEqual(bytesOfTheTexture, texture.Result.EncodeToJPG(1));
    }
}
