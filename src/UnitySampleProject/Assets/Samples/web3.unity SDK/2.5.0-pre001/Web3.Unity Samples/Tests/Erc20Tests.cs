using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

public class Erc20Tests
{
    private Erc20Sample logic;

    [OneTimeSetUp]
    public void Setup()
    {
        var web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
            });

        var buildWeb3Task = web3Builder.BuildAsync().AsTask();
        buildWeb3Task.Wait();
        var web3 = buildWeb3Task.Result;

        logic = new Erc20Sample(web3);
    }

    [Test]
    public async Task Erc20SampleTest()
    {
        // Arrange
        var contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

        // Act
        var name = await logic.Name(contractAddress);

        // Assert
        Assert.AreEqual("ChainToken", name);
    }
}
