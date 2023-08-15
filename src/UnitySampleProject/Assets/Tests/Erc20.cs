using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

public class Erc20
{
    [OneTimeSetUp]
    public async Task Setup()
    {
        var web3 = await new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseJsonRpcProvider();
            })
            .BuildAsync();
        
        // todo can't use Erc20Sample here (have to put it in a separate assembly and reference it) 
    }
    
    [Test]
    public void Erc20SimplePasses()
    {
    }
}
