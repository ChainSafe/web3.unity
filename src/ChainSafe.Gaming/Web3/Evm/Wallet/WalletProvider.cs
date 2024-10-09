using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    /// Concrete implementation of <see cref="IWalletProvider"/>.
    /// </summary>
    public abstract class WalletProvider : RpcClientProvider, IWalletProvider, IChainSwitchHandler // todo make sure chain id is in sync for wallet and sdk
    {
        private readonly ILogWriter logWriter;
        private readonly IChainConfig chainConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletProvider"/> class.
        /// </summary>
        /// <param name="environment">Injected <see cref="Web3Environment"/>.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="chainRegistryProvider"/>.</param>
        /// <param name="chainConfig">Injected <see cref="chainConfig"/>.</param>
        protected WalletProvider(Web3Environment environment, IChainConfig chainConfig)
            : base(environment, chainConfig)
        {
            this.chainConfig = chainConfig;
            this.logWriter = environment.LogWriter;
        }

        public abstract Task<string> Connect();

        public abstract Task Disconnect();

        public abstract Task<T> Request<T>(string method, params object[] parameters); // todo sync wallet chain id before sending any other request

        public Task HandleChainSwitching()
        {
            return SwitchChain(chainConfig.ChainId);
        }

        protected async Task SwitchChain(string chainId)
        {
            var str = $"{BigInteger.Parse(chainId):X}";
            str = str.TrimStart('0');
            var networkSwitchParams = new
            {
                chainId = $"0x{str}", // Convert the Chain ID to hex format
            };

            try
            {
                await Request<string>("wallet_switchEthereumChain", networkSwitchParams);
            }
            catch (Exception ex)
            {
                throw new Web3Exception($"Error occured while trying to switch wallet chain.", ex);
            }
        }
    }
}