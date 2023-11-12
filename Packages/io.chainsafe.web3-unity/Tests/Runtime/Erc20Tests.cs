using System.Collections;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts;
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

public class Erc20Tests
{
    // Fields
    #region Contract Calls
    
    private const string Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private const string ContractAddress = "0x5213E57f38238C46560a0f1686CCaFf54263cE44";
    // Mint ERC20 function adjusts the total supply so we have a duplicate here that doesn't change for the test to pass
    private const string TotalSupplyAddress = "0xd1A103234d1D65E0E817A523d679B114cf86521A";

    #endregion

    private Web3 web3;
    private WalletConnectConfig config;

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

        var web3Builder = new Web3Builder(projectConfigScriptableObject).Configure(services =>
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
        var getBalanceOf = Erc20.BalanceOf(web3, ContractAddress, Account);
		yield return new WaitUntil(() => getBalanceOf.IsCompleted);
		Assert.AreEqual(new BigInteger(1000000000000000000), getBalanceOf.Result);
        //Assert.AreEqual(new BigInteger(new byte[]
        //{
        //    0, 0, 0, 64, 234, 237, 116, 70, 208, 156, 44, 159, 12
        //}), getBalanceOf.Result);
    }
    
    [UnityTest]
    public IEnumerator TestNativeBalanceOf()
    {
        var getNativeBalanceOf = Erc20.NativeBalanceOf(web3, Account);
        yield return new WaitUntil(() => getNativeBalanceOf.IsCompleted);
        Assert.AreEqual(new BigInteger(500000000000000000), getNativeBalanceOf.Result);
        //Assert.AreEqual(new BigInteger(new byte[]
        //{
        //    0, 144, 99, 20, 5, 161, 13, 3
        //}), getNativeBalanceOf.Result);
    }

    [UnityTest]
    public IEnumerator TestDecimals()
    {
        var getDecimals = Erc20.Decimals(web3, ContractAddress);
        yield return new WaitUntil(() => getDecimals.IsCompleted);
        Assert.AreEqual(new BigInteger(18), getDecimals.Result);
    }

    [UnityTest]
    public IEnumerator TestName()
    {
        var getName = Erc20.Name(web3, ContractAddress);
        yield return new WaitUntil(() => getName.IsCompleted);
        Assert.AreEqual("CsTestErc20", getName.Result);
    }

    [UnityTest]
    public IEnumerator TestSymbol()
    {
        var getSymbol = Erc20.Symbol(web3, ContractAddress);
        yield return new WaitUntil(() => getSymbol.IsCompleted);
        Assert.AreEqual("CST", getSymbol.Result);
    }

    [UnityTest]
    public IEnumerator TestTotalSupply()
    {
        var getTotalSupply = Erc20.TotalSupply(web3, TotalSupplyAddress);
        yield return new WaitUntil(() => getTotalSupply.IsCompleted);
		Assert.AreEqual(new BigInteger(1000000000000000000), getTotalSupply.Result);
        //Assert.AreEqual(new BigInteger(new byte[]
        //{
        //    0, 0, 0, 64, 234, 237, 116, 70, 208, 156, 44, 159, 12
        //}), getTotalSupply.Result);
    }
}