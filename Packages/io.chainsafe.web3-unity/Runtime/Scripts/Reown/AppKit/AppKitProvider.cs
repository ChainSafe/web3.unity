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
using Reown.AppKit.Unity.Utils;
using Reown.Sign.Unity.Utils;
using UnityEngine;
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
        private bool initialized;

        private readonly string[] TestnetSuffix = new[]
        {
            "Testnet",
            "Goerli",
            "Sepolia",
        };
        
        public AppKitProvider(ReownHttpClient httpClient, IChainConfigSet chains, IReownConfig reownConfig, ILogWriter logWriter, Web3Environment web3Environment, IChainConfig chainConfig, IOperationTracker operationTracker) : base(web3Environment, chainConfig, operationTracker)
        {
            _httpClient = httpClient;
            _chains = chains;
            _logWriter = logWriter;
            ReownConfig = reownConfig;
        }

        private async Task Initialize()
        {
            if(initialized)
                return;
           
            var appKitConfig = new AppKitConfig()
            {
                projectId = ReownConfig.ProjectId,
                metadata = new Metadata(ReownConfig.Metadata.Name, ReownConfig.Metadata.Description, ReownConfig.Metadata.Url, ReownConfig.Metadata.Icons[0], new RedirectData()
                {
                    Native = ReownConfig.Metadata.Redirect.Native,
                    Universal = ReownConfig.Metadata.Redirect.Universal,
                }),
                supportedChains = ConvertIChainConfigToAppKitChain(),
                enableEmail = false,
                enableOnramp = false,
            };
            await W3AppKit.InitializeAsync(appKitConfig);
            initialized = true;
            
        }

        private Chain[] ConvertIChainConfigToAppKitChain()
        {
            var allChains = _chains.Configs
                .Select(x => new Chain(ChainConstants.Namespaces.Evm, x.ChainId, 
                    x.Chain, new Currency(x.NativeCurrency.Name, x.NativeCurrency.Symbol, 
                        x.NativeCurrency.Decimals), new BlockExplorer(x.Chain + " block explorer", x.BlockExplorerUrl),
                    x.Rpc, IsTestnet(x.Chain), "https://chainlist.org/unknown-logo.png", x.Chain.ToCamelCase())).ToArray();

            return allChains;
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
        
        private const string EvmNamespace = "eip155";

        
        private string BuildChainIdForReown(string chainId)
        {
            return $"{EvmNamespace}:{chainId}";
        }
        
        private async Task<T> ReownRequest<T>(string method, params object[] parameters)
        {
            var session = W3AppKit.Instance.SignClient.Session.Values[0];
            var currentChain = W3AppKit.NetworkController.ActiveChain;
            // Helper method to make a request using ReownSignClient.
            async Task<T> MakeRequest<TRequest>(bool sendChainId = true)
            {
                var data = (TRequest)Activator.CreateInstance(typeof(TRequest), parameters);
                try
                {
                    return await W3AppKit.Instance.SignClient.Request<TRequest, T>(
                        session.Topic,
                        data,
                        sendChainId ? BuildChainIdForReown(currentChain.ChainId) : null);
                }
                catch (KeyNotFoundException e)
                {
                    throw new ReownIntegrationException("Can't execute request. The session was most likely terminated on the wallet side.", e);
                }
            }

            switch (method)
            {
                case "personal_sign":
                    return await MakeRequest<EthSignMessage>();
                case "eth_signTypedData":
                    return await MakeRequest<EthSignTypedData>();
                case "eth_signTransaction":
                    return await MakeRequest<EthSignTransaction>();
                case "eth_sendTransaction":
                    return await MakeRequest<EthSendTransaction>();
                case "wallet_switchEthereumChain":
                    return await MakeRequest<WalletSwitchEthereumChain>(false);
                case "wallet_addEthereumChain":
                    return await MakeRequest<WalletAddEthereumChain>(false);
                default:
                    try
                    {
                        // Direct RPC request via http, Reown RPC url.
                        var chain = session.Namespaces.First().Value.Chains[0];

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