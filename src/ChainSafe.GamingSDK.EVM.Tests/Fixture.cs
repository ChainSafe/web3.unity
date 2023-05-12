using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
