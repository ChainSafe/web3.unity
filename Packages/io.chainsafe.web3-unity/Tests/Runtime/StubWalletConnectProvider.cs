using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace Tests.Runtime
{
    public class StubWalletConnectProvider : WalletProvider
    {
        private readonly StubWalletConnectProviderConfig config;

        public StubWalletConnectProvider(StubWalletConnectProviderConfig config, ChainRegistryProvider chainRegistryProvider) : base(chainRegistryProvider: chainRegistryProvider)
        {
            this.config = config;
        }

        public override Task<string> Connect()
        {
            return Task.FromResult(config.WalletAddress);
        }

        public override Task Disconnect()
        {
            // empty
            return Task.CompletedTask;
        }

        public override Task<T> Perform<T>(string method, params object[] parameters)
        {
            // box and unbox the stub response.
            return Task.FromResult((T) (config.StubResponse as object));
        }
    }
}