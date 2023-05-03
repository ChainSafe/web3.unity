using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.NetCore;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    [SetUpFixture]
    public class Fixture
    {
        [OneTimeSetUp]
        public void SetupEnvironment()
        {
            NetCoreRpcEnvironment.InitializeRpcEnvironment("http://localhost");
        }
    }
}
