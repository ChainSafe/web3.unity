using System.Collections;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

public class Erc20Tests
{
    private Erc20Sample _logic;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //wait for some time to initialize
        yield return new WaitForSeconds(5f);

        var config = ProjectConfigUtilities.Load("7e99206b-e098-4666-bfbf-4991a1800a33", "5", "ethereum", "goerli",
            string.Empty, "https://goerli.infura.io/v3/06f3f78b0f324d9c8cde54f90cd4fb5b");

        Web3Builder web3Builder = new Web3Builder(config).Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseRpcProvider();
        });

        ValueTask<Web3> buildWeb3 = web3Builder.BuildAsync();

        //wait until for async task to finish
        yield return new WaitUntil(() => buildWeb3.IsCompleted);

        _logic = new Erc20Sample(buildWeb3.Result);
    }

    [UnityTest]
    public IEnumerator Erc20SampleTest()
    {
        var contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

        Task<string> getName = _logic.Name(contractAddress);

        //wait until for async task to finish
        yield return new WaitUntil(() => getName.IsCompleted);

        Assert.AreEqual("ChainToken", getName.Result);
    }
}