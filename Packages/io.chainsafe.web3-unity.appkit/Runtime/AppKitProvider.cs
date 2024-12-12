using System;
using System.Collections.Generic;
using System.Globalization;
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
using Reown.Sign.Models;
using UnityEngine;
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
        private AppKitCore _appKitCore;

        private readonly string[] TestnetSuffix = new[]
        {
            "Testnet",
            "Goerli",
            "Sepolia",
        };
        
        public AppKitProvider(ReownHttpClient httpClient, IChainConfigSet chains, IReownConfig reownConfig, ILogWriter logWriter, Web3Environment web3Environment, IChainManager chainManager, IOperationTracker operationTracker) : base(web3Environment, chainManager.Current, operationTracker)
        {
            _httpClient = httpClient;
            _chains = chains;
            _logWriter = logWriter;
            _chainManager = chainManager;
            ReownConfig = reownConfig;
        }

        private async Task Initialize()
        {
            if(_appKitCore != null) 
                return;
            _appKitCore = UnityEngine.Object.Instantiate(Resources.Load<AppKitCore>("Reown AppKit"));
            
            _appKitChains = _chains.Configs
                .Select(IChainConfigToAppKitChain).ToArray();
            
            var appKitConfig = new AppKitConfig()
            {
                projectId = ReownConfig.ProjectId,
                metadata = new Metadata(ReownConfig.Metadata.Name, ReownConfig.Metadata.Description, ReownConfig.Metadata.Url, ReownConfig.Metadata.Icons[0], new RedirectData()
                {
                    Native = ReownConfig.Metadata.Redirect.Native,
                    Universal = ReownConfig.Metadata.Redirect.Universal,
                }),
                supportedChains = _appKitChains,
                enableEmail = false,
                enableOnramp = false,
            };
            
            await W3AppKit.InitializeAsync(appKitConfig);
        }

        public override async Task HandleChainSwitching()
        {
            var chain = _appKitChains.FirstOrDefault(x => x.ChainReference == _chainManager.Current.ChainId);
            await W3AppKit.NetworkController.ChangeActiveChainAsync(chain);
        }
        
        private Chain IChainConfigToAppKitChain(IChainConfig x)
        {
            return new Chain(ChainConstants.Namespaces.Evm, x.ChainId,
                x.Chain, new Currency(x.NativeCurrency.Name, x.NativeCurrency.Symbol,
                    x.NativeCurrency.Decimals), new BlockExplorer(x.Chain + " block explorer", x.BlockExplorerUrl),
                x.Rpc, IsTestnet(x.Chain), "https://chainlist.org/unknown-logo.png", ToCamelCase(x.Chain));
        }
        
        static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var words = input.Split(' ');
            for (int i = 1; i < words.Length; i++)
            {
                words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower());
            }

            return words[0].ToLower() + string.Concat(words[1..]);
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
            return default;
        }

        public bool StoredSessionAvailable => false;

        private readonly TaskCompletionSource<string> _accountConnected = new ();
        
        public override async Task<string> Connect()
        { 
            await Initialize();
            W3AppKit.AccountConnected += AppKitAccountConnected;
            W3AppKit.OpenModal();
            var result = await _accountConnected.Task;
            return result;
        }
        
        private async void AppKitAccountConnected(object sender, Connector.AccountConnectedEventArgs e)
        {
            var account = await e.GetAccount();
            _accountConnected.SetResult(account.Address);
        }
        
        public override async Task Disconnect()
        {
            W3AppKit.AccountConnected -= AppKitAccountConnected;
            await W3AppKit.ConnectorController.DisconnectAsync();
        }

        public override async Task<T> Request<T>(string method, params object[] parameters)
        {
            return await ReownRequest<T>(method, parameters);
        }
        
        private async Task<T> ReownRequest<T>(string method, params object[] parameters)
        {
            // Helper method to make a request using ReownSignClient.
            async Task<T> MakeRequest<TRequest>(bool sendChainId=true)
            {
                var data = (TRequest)Activator.CreateInstance(typeof(TRequest), parameters);
                try
                {
                    return await WagmiInterop.InteropCallAsync<TRequest, T>(method, data);
                }
                catch (KeyNotFoundException e)
                {
                    throw new ReownIntegrationException("Can't execute request. The session was most likely terminated on the wallet side.", e);
                }
            }

            switch (method)
            {
                case "personal_sign":
                    method = "signMessage";
                    return await MakeRequest<EthSignMessage>();
                case "eth_signTypedData":
                    method = "signTypedData";
                    return await MakeRequest<EthSignTypedData>();
                case "eth_sendTransaction":
                    method = "sendTransaction";
                    return await MakeRequest<EthSendTransactionSingle>();
                default:
                    try
                    {
                        // Direct RPC request via http, Reown RPC url.
                        var chain = W3AppKit.NetworkController.ActiveChain.ChainId;

                        // Using Reown Blockchain API: https://docs.reown.com/cloud/blockchain-api
                        var url = $"https://rpc.walletconnect.com/v1?chainId={chain}&projectId={ReownConfig.ProjectId}";

                        var body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters));
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