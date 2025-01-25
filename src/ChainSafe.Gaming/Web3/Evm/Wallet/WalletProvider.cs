using System;
using System.Globalization;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Operations;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    ///     Concrete implementation of <see cref="IWalletProvider" />.
    /// </summary>
    public abstract class
        WalletProvider : RpcClientProvider, IWalletProvider,
        IChainSwitchHandler // todo make sure chain id is in sync for wallet and sdk
    {
        private readonly IChainConfig chainConfig;
        private readonly ILogWriter logWriter;
        private readonly IOperatingSystemMediator operatingSystemMediator;
        private readonly IOperationTracker operationTracker;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WalletProvider" /> class.
        /// </summary>
        /// <param name="environment">Injected <see cref="Web3Environment" />.</param>
        /// <param name="chainConfig">Injected <see cref="chainConfig" />.</param>
        /// <param name="operationTracker">Injected <see cref="IOperationTracker" />.</param>
        /// <param name="operatingSystemMediator">Injected <see cref="IOperatingSystemMediator" />.</param>
        protected WalletProvider(
            Web3Environment environment,
            IChainConfig chainConfig,
            IOperationTracker operationTracker,
            IOperatingSystemMediator operatingSystemMediator)
            : base(environment, chainConfig)
        {
            this.operationTracker = operationTracker;
            this.operatingSystemMediator = operatingSystemMediator;
            this.chainConfig = chainConfig;
            logWriter = environment.LogWriter;
        }

        public virtual async Task HandleChainSwitching()
        {
            try
            {
                await SwitchChain(chainConfig);
            }
            catch (Exception ex)
            {
                throw new Web3Exception("Error occured while trying to switch wallet chain.", ex);
            }
        }

        public abstract Task<string> Connect();

        public abstract Task Disconnect();

        public abstract Task<T>
            Request<T>(
                string method,
                params object[] parameters); // todo sync wallet chain id before sending any other request

        public virtual async Task SwitchChainAddIfMissing(IChainConfig config = null)
        {
            try
            {
                var chainId = await GetWalletChainId();
                var chainConfig = config ?? this.chainConfig;

                // If we are already on the correct network, return.
                if (chainId == chainConfig.ChainId)
                {
                    return;
                }
            }
            catch (Exception)
            {
                logWriter.Log("Chain Id isn't present in the wallet. Adding it...");
            }

            var addChainParameter = new
            {
                chainId = GetChainId(chainConfig.ChainId),
                chainName = chainConfig.Chain,
                nativeCurrency = new
                {
                    name = chainConfig.NativeCurrency.Name,
                    symbol = chainConfig.NativeCurrency.Symbol,
                    decimals = chainConfig.NativeCurrency.Decimals,
                },
                rpcUrls = new[] { chainConfig.Rpc },
                blockExplorerUrls = new[] { chainConfig.BlockExplorerUrl },
            };

            try
            {
                if (operatingSystemMediator.IsMobilePlatform)
                {
                    await Task.Run(() => Request<object[]>("wallet_addEthereumChain", addChainParameter));
                }
                else
                {
                    await Request<object[]>("wallet_addEthereumChain", addChainParameter);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(
                        "May not specify default MetaMask chain",
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    if (operatingSystemMediator.IsMobilePlatform)
                    {
                        await Task.Run(SwitchToDefaultMetaMaskChain);
                    }
                    else
                    {
                        await SwitchToDefaultMetaMaskChain();
                    }

                    return;
                }

                logWriter.LogError($"Failed to add or switch to the network: {ex.Message}");
                throw new InvalidOperationException("Failed to add or switch to the network.", ex);
            }
        }

        private async Task SwitchChain(IChainConfig chainId)
        {
            await SwitchChainAddIfMissing(chainId);
        }

        protected virtual async Task<string> GetWalletChainId()
        {
            var rawHexChainId = await Request<string>(
                "eth_chainId");
            rawHexChainId = rawHexChainId.Replace("0x", string.Empty);
            var number = Convert.ToUInt64(rawHexChainId, 16);

            return number.ToString(CultureInfo.InvariantCulture);
        }

        public string GetChainId(string chainId)
        {
            return "0x" + ulong.Parse(chainId).ToString("X");
        }

        /// <summary>
        ///     Switching to the default chain (either mainnet or sepolia) in MetaMask.
        /// </summary>
        /// <exception cref="InvalidOperationException">Switching to the desired chain is not successful.</exception>
        private async Task SwitchToDefaultMetaMaskChain()
        {
            {
                try
                {
                    await Request<object[]>(
                        "wallet_switchEthereumChain",
                        new { chainId = "0x" + ulong.Parse(chainConfig.ChainId).ToString("X") });
                }
                catch (Exception e)
                {
                    logWriter.LogError($"Failed to switch to the network: {e.Message}");
                    throw new InvalidOperationException("Failed to add or switch to the network.", e);
                }
            }
        }
    }
}