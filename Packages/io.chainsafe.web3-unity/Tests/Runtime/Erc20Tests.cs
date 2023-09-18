using System.Collections;
using System.Numerics;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Erc20Tests
{
    private const string Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private const string ContractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    private Erc20Sample _logic;

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

        _logic = new Erc20Sample(buildWeb3.Result);
    }

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = _logic.BalanceOf(ContractAddress, Account);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);

        Assert.AreEqual(new BigInteger(new byte[]
        {
            0, 0, 0, 64, 234, 237, 116, 70, 208, 156, 44, 159, 12
        }), getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestDecimals()
    {
        var getDecimals = _logic.Decimals(ContractAddress);
        yield return new WaitUntil(() => getDecimals.IsCompleted);

        Assert.AreEqual(new BigInteger(18), getDecimals.Result);
    }

    [UnityTest]
    public IEnumerator TestName()
    {
        var getName = _logic.Name(ContractAddress);
        yield return new WaitUntil(() => getName.IsCompleted);

        Assert.AreEqual("ChainToken", getName.Result);
    }

    [UnityTest]
    public IEnumerator TestNativeBalanceOf()
    {
        var getNativeBalanceOf = _logic.NativeBalanceOf(Account);

        yield return new WaitUntil(() => getNativeBalanceOf.IsCompleted);


        Assert.AreEqual(new BigInteger(new byte[]
        {
            0, 144, 99, 20, 5, 161, 13, 3
        }), getNativeBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestSymbol()
    {
        var getSymbol = _logic.Symbol(ContractAddress);

        yield return new WaitUntil(() => getSymbol.IsCompleted);

        Assert.AreEqual("CT", getSymbol.Result);
    }

    [UnityTest]
    public IEnumerator TestTotalSupply()
    {
        var getTotalSupply = _logic.TotalSupply(ContractAddress);

        yield return new WaitUntil(() => getTotalSupply.IsCompleted);

        Assert.AreEqual(new BigInteger(new byte[]
        {
            0, 0, 0, 64, 234, 237, 116, 70, 208, 156, 44, 159, 12
        }), getTotalSupply.Result);
    }
}