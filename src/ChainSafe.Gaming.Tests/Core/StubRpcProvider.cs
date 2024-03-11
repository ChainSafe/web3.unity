using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Evm.Providers;

namespace ChainSafe.Gaming.Tests.Core
{
    public class StubRpcProvider : IRpcProvider
    {
        public Network LastKnownNetwork { get; } = null;

        public Task<Network> RefreshNetwork()
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Perform<T>(string method, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}