using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Evm.NetCore;

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
