using System;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    public class TestHelper
    {
        public static void VerifyGanacheConnection(BaseProvider provider)
        {
            try
            {
                var network = provider.DetectNetwork().Result;
            }
            catch
            {
                Assert.Ignore($"Ignoring this test because Ganache is not set properly on http://127.0.0.1:7545");
            }
        }
    }
}