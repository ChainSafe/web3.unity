using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Core.Models.Publisher;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectUnity.Core;
using WalletConnectUnity.Modal;
using OriginalWc = WalletConnectUnity.Core.WalletConnect;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public class WalletConnectUnityProvider : WalletProvider, ILifecycleParticipant
    {
        private static bool instanceStarted;
        
        private readonly IWalletConnectUnityConfig config;
        private readonly IChainConfig chainConfig;
        private readonly IHttpClient httpClient;

        public WalletConnectUnityProvider(
            IChainConfig chainConfig, 
            ChainRegistryProvider chainRegistry,
            IHttpClient httpClient)
            : base(chainRegistry)
        {
            this.chainConfig = chainConfig;
            this.httpClient = httpClient;
            config = new WalletConnectUnityConfig();
        }

        public WalletConnectUnityProvider(
            IWalletConnectUnityConfig config, 
            IChainConfig chainConfig,
            ChainRegistryProvider chainRegistry,
            IHttpClient httpClient)
            : this(chainConfig, chainRegistry, httpClient)
        {
            this.config = config;
        }
        
        public async ValueTask WillStartAsync()
        {
            if (instanceStarted)
            {
                throw new Web3Exception(
                    $"One instance of {nameof(WalletConnectUnityProvider)} is already running. This integration of WalletConnect does not support running multiple instances.");
            }

            // if (config.ShouldSpawnModal)
            // {
            //     await SpawnModal();
            // }

            if (!WalletConnectModal.IsReady) 
            {
                throw new Web3Exception(
                    $"{nameof(WalletConnectModal)} is not ready yet. Make sure it's loaded and initialized before launching {nameof(Web3.Web3)}");
            }

            instanceStarted = true;
        }

        public ValueTask WillStopAsync()
        {
            instanceStarted = false;

            return new ValueTask(Task.CompletedTask);
        }
        
        public override async Task<string> Connect()
        {
            if (OriginalWc.Instance.IsConnected) // WalletConnectModal tries resuming the session automatically, so we check if it succeeded right here
            {
                return ReadPublicAddress();
            }

            var tcs = new TaskCompletionSource<byte>();
            var connectionTask = tcs.Task;
            
            WalletConnectModal.Connected += OnConnected;
            WalletConnectModal.ConnectionError += ConnectionError;

            try
            {
                WalletConnectModal.Open(new WalletConnectModalOptions
                {
                    IncludedWalletIds = config.IncludedWalletIds,
                    ExcludedWalletIds = config.ExcludedWalletIds,
                    ConnectOptions = BuildConnectOptions()
                });

                await connectionTask;
            }
            finally
            {
                WalletConnectModal.Connected -= OnConnected;
                WalletConnectModal.ConnectionError -= ConnectionError;
            }

            return ReadPublicAddress();

            void OnConnected(object sender, EventArgs e)
            {
                tcs.SetResult(0);
            }

            void ConnectionError(object sender, EventArgs e)
            {
                tcs.SetException(new Web3Exception("Error occured when tried to connect using WalletConnect."));
            }
        }

        public override Task Disconnect()
        {
            WalletConnectModal.Disconnect();
            return Task.CompletedTask;
        }

        public override async Task<T> Perform<T>(string method, params object[] parameters)
        {
            if (!OriginalWc.Instance.IsConnected)
            {
                throw new WalletConnectException("Can't send requests. No session is connected at the moment.");
            }

            // check if session is expired to renew session

            bool methodRegistered = OriginalWc.Instance.ActiveSession.Namespaces.Any(n => n.Value.Methods.Contains(method));

            if (!methodRegistered)
            {
                throw new WalletConnectException(
                    $"RPC method {method} is not supported. " +
                    $"If you add a new method you have to update {nameof(WalletConnectProvider)} code to reflect those changes. " +
                    "Contact ChainSafe if you think a specific method should be included in the SDK.");
            }

            string chainId = OriginalWc.Instance.ActiveSession.Namespaces.First().Value.Chains[0].Split(':')[1];

            throw new NotImplementedException("Interacting with WalletConnectUnity was not implemented yet.");
            
            // return await WalletConnectRequest<T>(method, chainId, parameters);
        }

        private string FullChainId => $"{ChainConstants.Namespaces.Evm}:{chainConfig.ChainId}";

        private string ReadPublicAddress() => OriginalWc.Instance.ActiveSession.CurrentAddress(FullChainId).Address;

        // private async Task SpawnModal()
        // {
        //     if (!config.ModalPrefab)
        //     {
        //         throw new Web3Exception("No WalletConnectModal was provided in config.");
        //     }
        //
        //     var tcs = new TaskCompletionSource<bool>();
        //     WalletConnectModal.Ready += OnModalReady;
        //     await Object.InstantiateAsync(config.ModalPrefab);
        //     await tcs.Task;
        //
        //     void OnModalReady(object sender, ModalReadyEventArgs modalReadyEventArgs)
        //     {
        //         WalletConnectModal.Ready -= OnModalReady;
        //         tcs.SetResult(true);
        //     }
        // }

        private ConnectOptions BuildConnectOptions()
        {
            var requiredNamespaces = new RequiredNamespaces
            {
                {
                    ChainConstants.Namespaces.Evm,
                    new ProposedNamespace
                    {
                        Chains = new[] { FullChainId },
                        Events = new[] { "chainChanged", "accountsChanged" },
                        Methods = new[]
                        {
                            "eth_sendTransaction",
                            "eth_signTransaction",
                            "eth_sign",
                            "personal_sign",
                            "eth_signTypedData",
                        },
                    }
                },
            };

            return new ConnectOptions
            {
                RequiredNamespaces = requiredNamespaces
            };
        }
        
        // private async Task<T> WalletConnectRequest<T>(string method, string chainId, params object[] parameters)
        // {
        //     // Helper method to make a request using WalletConnectSignClient.
        //     async Task<T> MakeRequest<TRequest>()
        //     {
        //         var data = (TRequest)Activator.CreateInstance(typeof(TRequest), parameters);
        //         return await OriginalWc.Instance.RequestAsync<TRequest, T>(data, chainId);
        //     }
        //
        //     switch (method)
        //     {
        //         case "personal_sign":
        //             return await MakeRequest<EthSignMessage>();
        //         case "eth_signTypedData":
        //             return await MakeRequest<EthSignTypedData>();
        //         case "eth_signTransaction":
        //             return await MakeRequest<EthSignTransaction>();
        //         case "eth_sendTransaction":
        //             return await MakeRequest<EthSendTransaction>();
        //         default:
        //             try
        //             {
        //                 return await Request<T>(method, parameters);
        //             }
        //             catch (Exception e)
        //             {
        //                 throw new WalletConnectException($"{method} RPC method currently not implemented.", e);
        //             }
        //     }
        // }
        //
        // // Direct RPC request via WalletConnect RPC url.
        // private async Task<T> Request<T>(string method, params object[] parameters)
        // {
        //     string chain = OriginalWc.Instance.ActiveSession.Namespaces.First().Value.Chains[0];
        //
        //     // Using WalletConnect Blockchain API: https://docs.walletconnect.com/cloud/blockchain-api
        //     var url = $"https://rpc.walletconnect.com/v1?chainId={chain}&projectId={config.ProjectId}";
        //
        //     string body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters));
        //
        //     var rawResult = await httpClient.PostRaw(url, body, "application/json");
        //
        //     RpcResponseMessage response = JsonConvert.DeserializeObject<RpcResponseMessage>(rawResult.Response);
        //
        //     return response.Result.ToObject<T>();
        // }
    }
}