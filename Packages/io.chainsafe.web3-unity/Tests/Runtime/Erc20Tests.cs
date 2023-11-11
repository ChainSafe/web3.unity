using System.Collections;
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

public class Erc20Tests
{
    // Fields
    #region Contract Calls
    
    private const string Account = "0x55ffe9E30347266f02b9BdAe20aD3a86493289ea";

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
        var getBalanceOf = Erc20.BalanceOf(web3, Contracts.Erc20, Account);
        yield return new WaitUntil(() => getBalanceOf.IsCompleted);

        Assert.AreEqual(new BigInteger(new byte[]
        {
            0
        }), getBalanceOf.Result);
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
    public IEnumerator TestNativeBalanceOf()
    {
        var getNativeBalanceOf = Erc20.NativeBalanceOf(web3, Account);

        yield return new WaitUntil(() => getNativeBalanceOf.IsCompleted);
        
        Assert.AreEqual(new BigInteger(0), getNativeBalanceOf.Result);
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
        var getTotalSupply = Erc20.TotalSupply(web3, Contracts.Erc20);

        yield return new WaitUntil(() => getTotalSupply.IsCompleted);

        Assert.AreEqual(new BigInteger(1000000000000000000), getTotalSupply.Result);
    }
}