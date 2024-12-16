using System;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Operations;
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
        private readonly IOperationTracker operationTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletProvider"/> class.
        /// </summary>
        /// <param name="environment">Injected <see cref="Web3Environment"/>.</param>
        /// <param name="chainConfig">Injected <see cref="chainConfig"/>.</param>
        /// <param name="operationTracker">Injected <see cref="IOperationTracker"/>.</param>
        protected WalletProvider(Web3Environment environment, IChainConfig chainConfig, IOperationTracker operationTracker)
            : base(environment, chainConfig)
        {
            this.operationTracker = operationTracker;
            this.chainConfig = chainConfig;
            this.logWriter = environment.LogWriter;
        }

        public abstract Task<string> Connect();

        public abstract Task Disconnect();

        public abstract Task<T> Request<T>(string method, params object[] parameters); // todo sync wallet chain id before sending any other request

        public async Task HandleChainSwitching()
        {
            try
            {
                await SwitchChain(chainConfig);
            }
            catch (Exception ex)
            {
                throw new Web3Exception($"Error occured while trying to switch wallet chain.", ex);
            }
        }

        private async Task SwitchChain(IChainConfig chainId)
        {
            await AddNetworkIfNotExistInWallet(chainId);
        }

        private async Task<string> GetWalletChainId()
        {
            var rawHexChainId = await Request<string>(
                "eth_chainId");
            rawHexChainId = rawHexChainId.Replace("0x", string.Empty);
            ulong number = Convert.ToUInt64(rawHexChainId, 16);

            return number.ToString(CultureInfo.InvariantCulture);
        }

        public async Task AddNetworkIfNotExistInWallet(IChainConfig config = null)
        {
            var chainId = await GetWalletChainId();
            var chainConfig = config ?? this.chainConfig;

            // If we are already on the correct network, return.
            if (chainId == chainConfig.ChainId)
            {
                return;
            }

            var addChainParameters = new[]
            {
                new
                {
                    chainId = "0x" + ulong.Parse(chainConfig.ChainId).ToString("X"),
                    chainName = chainConfig.Chain,
                    nativeCurrency = new
                    {
                        name = chainConfig.NativeCurrency.Name,
                        symbol = chainConfig.NativeCurrency.Symbol,
                        decimals = chainConfig.NativeCurrency.Decimals,
                    },
                    rpcUrls = new[] { chainConfig.Rpc },
                    blockExplorerUrls = new[] { chainConfig.BlockExplorerUrl },
                },
            };

            using (operationTracker.TrackOperation($"Adding or Switching the network to: {chainConfig.Chain}..."))
            {
                try
                {
                    await AddChain(addChainParameters);
                }
                catch (Exception e)
                {
                    logWriter.LogError($"Failed to add and switch to the network: {e.Message}");
                    throw new InvalidOperationException("Failed to add or switch to the network.", e);
                }
            }
        }

        protected async Task AddChain(object addChainParameters)
        {
            await Request<object[]>("wallet_addEthereumChain", addChainParameters);
        }
    }
}