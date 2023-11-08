using System.Collections;
using System.Numerics;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class Erc20Tests : SampleTestsBase
{
    // Fields
    #region Contract Calls
    
    private const string Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private const string ContractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

    #endregion

    #region Transfer
    
    private const string TransferErc20ContractAddress = "0xc778417e063141139fce010982780140aa0cd5ab";
    private const string SendToAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    #endregion
    
    private Erc20 erc20;

    [UnitySetUp]
    public override IEnumerator Setup()
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
            config = new WalletConnectConfig
            {
                // set wallet to testing
                Testing = true,
                TestWalletAddress = "0x55ffe9E30347266f02b9BdAe20aD3a86493289ea",
            };
        });

        var buildWeb3 = web3Builder.BuildAsync();

        //wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);

        erc20 = new Erc20(buildWeb3.Result);
    }

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = erc20.BalanceOf(ContractAddress, Account);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);

        Assert.AreEqual(new BigInteger(new byte[]
        {
            0, 0, 0, 64, 234, 237, 116, 70, 208, 156, 44, 159, 12
        }), getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestDecimals()
    {
        var getDecimals = erc20.Decimals(ContractAddress);
        yield return new WaitUntil(() => getDecimals.IsCompleted);

        Assert.AreEqual(new BigInteger(18), getDecimals.Result);
    }

    [UnityTest]
    public IEnumerator TestName()
    {
        var getName = erc20.Name(ContractAddress);
        yield return new WaitUntil(() => getName.IsCompleted);

        Assert.AreEqual("ChainToken", getName.Result);
    }

    [UnityTest]
    public IEnumerator TestNativeBalanceOf()
    {
        var getNativeBalanceOf = erc20.NativeBalanceOf(Account);

        yield return new WaitUntil(() => getNativeBalanceOf.IsCompleted);


        Assert.AreEqual(new BigInteger(new byte[]
        {
            0, 144, 99, 20, 5, 161, 13, 3
        }), getNativeBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestSymbol()
    {
        var getSymbol = erc20.Symbol(ContractAddress);

        yield return new WaitUntil(() => getSymbol.IsCompleted);

        Assert.AreEqual("CT", getSymbol.Result);
    }

    [UnityTest]
    public IEnumerator TestTotalSupply()
    {
        var getTotalSupply = erc20.TotalSupply(ContractAddress);

        yield return new WaitUntil(() => getTotalSupply.IsCompleted);

        Assert.AreEqual(new BigInteger(new byte[]
        {
            0, 0, 0, 64, 234, 237, 116, 70, 208, 156, 44, 159, 12
        }), getTotalSupply.Result);
    }

    [UnityTest]
    public IEnumerator TestTransferErc20()
    {
        config.TestResponse = "0xba90b6fb8cbee5fd0ad423cc74bb4a365bb88b260601933aac86b947945c5465";

        var transferErc20 = erc20.TransferErc20(TransferErc20ContractAddress, SendToAddress, "1000000000000000");

        yield return new WaitUntil(() => transferErc20.IsCompleted);

        if (transferErc20.Exception != null) throw transferErc20.Exception;
        
        Assert.IsTrue(transferErc20.IsCompletedSuccessfully);

        Assert.AreEqual(transferErc20.Result, new object[] { false });
    }
}