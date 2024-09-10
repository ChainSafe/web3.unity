using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    /// Concrete implementation of <see cref="IWalletProvider"/>.
    /// </summary>
    public abstract class WalletProvider : RpcClientProvider, IWalletProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalletProvider"/> class.
        /// </summary>
        /// <param name="environment">Injected <see cref="Web3Environment"/>.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="chainRegistryProvider"/>.</param>
        /// <param name="chainConfig">Injected <see cref="chainConfig"/>.</param>
        protected WalletProvider(Web3Environment environment, ChainRegistryProvider chainRegistryProvider, IChainConfig chainConfig)
            : base(environment, chainRegistryProvider, chainConfig)
        {
        }

        public abstract Task<string> Connect();

        public abstract Task Disconnect();

        public abstract Task<T> Request<T>(string method, params object[] parameters);
    }
}