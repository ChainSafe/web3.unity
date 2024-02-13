using System.Collections;
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

public class Erc20Tests : SampleTestsBase
{
    #region Fields

    #region Contract Calls

    private const string Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    // Mint ERC20 function adjusts the total supply so we have a duplicate contract that doesn't change for the test to pass
    private const string TotalSupplyAddress = "0xd1A103234d1D65E0E817A523d679B114cf86521A";

    #endregion

    #endregion

    [UnityTest]
    public IEnumerator TestBalanceOf()
    {
        var getBalanceOf = Erc20.BalanceOf(web3, Contracts.Erc20, Account);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);
        Assert.AreEqual(new BigInteger(1000000000000000000), getBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestNativeBalanceOf()
    {
        var getNativeBalanceOf = Erc20.NativeBalanceOf(web3, Account);
        yield return new WaitUntil(() => getNativeBalanceOf.IsCompleted);
        Assert.AreEqual(new BigInteger(500000000000000000), getNativeBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestDecimals()
    {
        var getDecimals = Erc20.Decimals(web3, Contracts.Erc20);
        yield return new WaitUntil(() => getDecimals.IsCompleted);
        Assert.AreEqual(new BigInteger(18), getDecimals.Result);
    }

    [UnityTest]
    public IEnumerator TestName()
    {
        var getName = Erc20.Name(web3, Contracts.Erc20);
        yield return new WaitUntil(() => getName.IsCompleted);
        Assert.AreEqual("CsTestErc20", getName.Result);
    }

    [UnityTest]
    public IEnumerator TestSymbol()
    {
        var getSymbol = Erc20.Symbol(web3, Contracts.Erc20);
        yield return new WaitUntil(() => getSymbol.IsCompleted);
        Assert.AreEqual("CST", getSymbol.Result);
    }

    [UnityTest]
    public IEnumerator TestTotalSupply()
    {
        var getTotalSupply = Erc20.TotalSupply(web3, TotalSupplyAddress);
        yield return new WaitUntil(() => getTotalSupply.IsCompleted);
        Assert.AreEqual(new BigInteger(1000000000000000000), getTotalSupply.Result);
    }
}