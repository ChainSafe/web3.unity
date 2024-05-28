using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectUnity.Core;
using WalletConnectUnity.Modal;
using OriginalWc = WalletConnectUnity.Core.WalletConnect;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public class WalletConnectUnityProvider : IWalletConnectProvider, ILifecycleParticipant
    {
        private static bool instanceStarted;
        
        private readonly IWalletConnectUnityConfig config;
        private readonly IChainConfig chainConfig;

        public WalletConnectUnityProvider(IChainConfig chainConfig)
        {
            this.chainConfig = chainConfig;
            this.config = new WalletConnectUnityConfig();
        }

        public WalletConnectUnityProvider(IWalletConnectUnityConfig config, IChainConfig chainConfig)
        {
            this.chainConfig = chainConfig;
            this.config = config;
        }
        
        public async ValueTask WillStartAsync()
        {
            if (instanceStarted)
            {
                throw new Web3Exception(
                    $"One instance of {nameof(WalletConnectUnityProvider)} is already running. This integration of WalletConnect does not support running multiple instances.");
            }

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
        
        public async Task<string> Connect()
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

        public Task Disconnect()
        {
            WalletConnectModal.Disconnect();
            return Task.CompletedTask;
        }

        public Task<string> Request<T>(T data, long? expiry = null) => OriginalWc.Instance.RequestAsync<T, string>(data);

        private string FullChainId => $"{ChainConstants.Namespaces.Evm}:{chainConfig.ChainId}";

        private string ReadPublicAddress() => OriginalWc.Instance.ActiveSession.CurrentAddress(FullChainId).Address;

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
    }
}