#if UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Methods;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Operations;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Reown.AppKit.Unity;
using Reown.AppKit.Unity.WebGl.Wagmi;
using UnityEngine;
using NativeCurrency = Reown.AppKit.Unity.WebGl.Wagmi.NativeCurrency;
using Object = UnityEngine.Object;
using W3AppKit = global::Reown.AppKit.Unity.AppKit;

namespace ChainSafe.Gaming.Reown.AppKit
{
    public class AppKitProvider : WalletProvider, ILifecycleParticipant, IConnectionHelper
    {
        private IReownConfig ReownConfig { get; set; }

        private readonly ReownHttpClient _httpClient;
        private readonly IChainConfigSet _chains;
        private readonly ILogWriter _logWriter;
        private readonly IChainManager _chainManager;
        private Chain[] _appKitChains;

        private readonly string[] TestnetSuffix = new[]
        {
            "Testnet",
            "Goerli",
            "Sepolia"
        };

        public AppKitProvider(ReownHttpClient httpClient, IChainConfigSet chains, IReownConfig reownConfig,
            ILogWriter logWriter, Web3Environment web3Environment, IChainManager chainManager,
            IOperationTracker operationTracker) : base(web3Environment, chainManager.Current, operationTracker)
        {
            _httpClient = httpClient;
            _chains = chains;
            _logWriter = logWriter;
            _chainManager = chainManager;
            ReownConfig = reownConfig;
        }

        private async Task Initialize()
        {
            if (W3AppKit.Instance != null)
                return;
            
            Object.Instantiate(Resources.Load<AppKitCore>("Reown AppKit"));

            _appKitChains = _chains.Configs
                .Select(x =>
                {
                    if (!ReownConfig.ChainIdViemNameMap.TryGetValue(x.ChainId, out var viemName))
                        throw new ReownIntegrationException(
                            $"Viem Name is not set for chain: {x.Chain} {x.ChainId}. Make sure to populate the viem name for this chain id in Reown Connection Provider.");
                    return IChainConfigToAppKitChain(x);
                }).ToArray();

            var appKitConfig = new AppKitConfig()
            {
                projectId = ReownConfig.ProjectId,
                metadata = new Metadata(ReownConfig.Metadata.Name, ReownConfig.Metadata.Description,
                    ReownConfig.Metadata.Url, ReownConfig.Metadata.Icons[0], new RedirectData()
                    {
                        Native = ReownConfig.Metadata.Redirect.Native,
                        Universal = ReownConfig.Metadata.Redirect.Universal
                    }),
                supportedChains = _appKitChains,
                enableEmail = false,
                enableOnramp = false
            };

            await W3AppKit.InitializeAsync(appKitConfig);
        }

        private Chain IChainConfigToAppKitChain(IChainConfig x)
        {
            return new Chain(ChainConstants.Namespaces.Evm, x.ChainId,
                x.Chain, new Currency(x.NativeCurrency.Name, x.NativeCurrency.Symbol,
                    x.NativeCurrency.Decimals), new BlockExplorer(x.Chain + " block explorer", x.BlockExplorerUrl),
                x.Rpc, IsTestnet(x.Chain), "https://chainlist.org/unknown-logo.png", ReownConfig.ChainIdViemNameMap[x.ChainId]);
        }

        private bool IsTestnet(string argChain)
        {
            return TestnetSuffix.Any(x => argChain.Contains(x, StringComparison.InvariantCultureIgnoreCase));
        }

        public async ValueTask WillStartAsync()
        {
            await Initialize();
        }

        public ValueTask WillStopAsync()
        {
            W3AppKit.AccountConnected -= AppKitAccountConnected;
            W3AppKit.ModalController.OpenStateChanged -= OpenStateChanged;
            return new ValueTask();
        }

        public bool StoredSessionAvailable => false;

        private readonly TaskCompletionSource<string> _accountConnected = new();

        public override async Task<string> Connect()
        {
            await Initialize();
            W3AppKit.AccountConnected += AppKitAccountConnected;
            W3AppKit.ModalController.OpenStateChanged += OpenStateChanged;
            W3AppKit.OpenModal();
            var result = await _accountConnected.Task;
            return result;
        }

        private async void OpenStateChanged(object sender, ModalOpenStateChangedEventArgs e)
        {
            await Task.Delay(200);
            if(!e.IsOpen && _accountConnected.Task.IsCompleted == false)
                _accountConnected.SetException(new TaskCanceledException());
        }

        private async void AppKitAccountConnected(object sender, Connector.AccountConnectedEventArgs e)
        {
            var account = await e.GetAccount();
            _accountConnected.SetResult(account.Address);
        }

        public override async Task Disconnect()
        {
            await W3AppKit.ConnectorController.DisconnectAsync();
        }

        public override async Task HandleChainSwitching()
        {
            var currentChain = _chainManager.Current;
            await WagmiInterop.SwitchChainAsync(int.Parse(currentChain.ChainId), new AddEthereumChainParameter()
            {
                chainId = $"{ChainConstants.Namespaces.Evm}:{currentChain.ChainId}",
                blockExplorerUrls = new [] { currentChain.BlockExplorerUrl },
                chainName = currentChain.Chain,
                nativeCurrency = new NativeCurrency()
                {
                    decimals = currentChain.NativeCurrency.Decimals,
                    name = currentChain.NativeCurrency.Name,
                    symbol = currentChain.NativeCurrency.Symbol
                },
                rpcUrls = new [] { currentChain.Rpc },
                iconUrls = new [] { "https://chainlist.org/unknown-logo.png"}
            });
        }

        public override async Task<T> Request<T>(string method, params object[] parameters)
        {
            return await ReownRequest<T>(method, parameters);
        }

        private async Task<T> ReownRequest<T>(string method, params object[] parameters)
        {
            // Helper method to make a request using ReownSignClient.
            async Task<T> MakeRequest<TRequest>(bool sendChainId = true)
            {
                var data = (TRequest)Activator.CreateInstance(typeof(TRequest), parameters);
                try
                {
                    return await WagmiInterop.InteropCallAsync<TRequest, T>(method, data);
                }
                catch (KeyNotFoundException e)
                {
                    throw new ReownIntegrationException(
                        "Can't execute request. The session was most likely terminated on the wallet side.", e);
                }
            }

            switch (method)
            {
                case "personal_sign":
                    method = "signMessage";
                    return await MakeRequest<EthSignMessageAppKit>();
                case "eth_signTypedData":
                    method = "signTypedData";
                    return await MakeRequest<EthSignTypedData>();
                case "eth_sendTransaction":
                    method = "sendTransaction";
                    return await MakeRequest<EthSendTransactionSingle>();
                case "wallet_addEthereumChain" :
                    return default;
                default:
                    try
                    {
                        // Direct RPC request via http, Reown RPC url.
                        var chain = W3AppKit.NetworkController.ActiveChain.ChainId;

                        // Using Reown Blockchain API: https://docs.reown.com/cloud/blockchain-api
                        var url = $"https://rpc.walletconnect.com/v1?chainId={chain}&projectId={ReownConfig.ProjectId}";

                        var body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method,
                            parameters));
                        var rawResult = await _httpClient.PostRaw(url, body, "application/json");
                        var response = JsonConvert.DeserializeObject<RpcResponseMessage>(rawResult.Response);

                        return response.Result.ToObject<T>();
                    }
                    catch (Exception e)
                    {
                        throw new ReownIntegrationException($"{method} RPC method currently not implemented.", e);
                    }
            }
        }
    }
}
#endif