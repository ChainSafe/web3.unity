using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Connection;
using ChainSafe.Gaming.Reown.Methods;
using ChainSafe.Gaming.Reown.Models;
using ChainSafe.Gaming.Reown.Storage;
using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Reown.Core.Common.Logging;
using Reown.Core.Common.Model.Errors;
using Reown.Core.Common.Utils;
using Reown.Core.Crypto;
using Reown.Core.Models.Publisher;
using Reown.Core.Network.Models;
using Reown.Sign;
using Reown.Sign.Models;
using Reown.Sign.Models.Engine;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// Reown implementation of <see cref="IWalletProvider"/>.
    /// </summary>
    public class ReownProvider : WalletProvider, ILifecycleParticipant, IConnectionHelper
    {
        private readonly ILogWriter logWriter;
        private readonly IReownConfig config;
        private readonly IChainConfig chainConfig;
        private readonly IOperatingSystemMediator osMediator;
        private readonly IWalletRegistry walletRegistry;
        private readonly RedirectionHandler redirection;
        private readonly IHttpClient reownHttpClient;
        private readonly IAnalyticsClient analyticsClient;
        private readonly Web3Environment environment;
        private readonly IChainConfigSet chainConfigSet;

        private SignClient signClient;
        private SessionStruct session;
        private bool connected;
        private bool initialized;
        private ConnectionHandlerConfig connectionHandlerConfig;
        private Dictionary<string, ProposedNamespace> optionalNamespaces;
        private WalletModel connectedLocalWallet;

        public ReownProvider(
            IReownConfig config,
            IChainConfig chainConfig,
            IChainConfigSet chainConfigSet,
            IWalletRegistry walletRegistry,
            RedirectionHandler redirection,
            Web3Environment environment,
            ReownHttpClient reownHttpClient)
            : base(environment, chainConfig)
        {
            this.chainConfigSet = chainConfigSet;
            this.environment = environment;
            analyticsClient = environment.AnalyticsClient;
            this.redirection = redirection;
            this.walletRegistry = walletRegistry;
            osMediator = environment.OperatingSystem;
            this.chainConfig = chainConfig;
            this.config = config;
            logWriter = environment.LogWriter;
            this.reownHttpClient = reownHttpClient;
        }

        public bool StoredSessionAvailable => signClient.AddressProvider.HasDefaultSession
                                              && !string.IsNullOrWhiteSpace(signClient.AddressProvider.DefaultSession.Topic);

        private bool OsManageWalletSelection => osMediator.Platform == Platform.Android;

        private static bool SessionExpired(SessionStruct s) => s.Expiry != null && Clock.IsExpired((long)s.Expiry);

        public async ValueTask WillStartAsync()
        {
            await Initialize();
        }

        private async Task Initialize()
        {
            if (initialized)
            {
                return;
            }

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = "Reown Initialized",
                PackageName = "io.chainsafe.web3-unity",
            });

            ValidateConfig();

            ReownLogger.Instance = new ReownLogWriter(logWriter, config);

            var storage = await ReownStorageFactory.Build(environment);
            var signClientOptions = new SignClientOptions
            {
                ProjectId = config.ProjectId,
                Name = config.ProjectName,
                Metadata = config.Metadata,
                BaseContext = config.BaseContext,
                Storage = storage,
                KeyChain = new KeyChain(storage),
                ConnectionBuilder = config.ConnectionBuilder,
                RelayUrlBuilder = config.RelayUrlBuilder,
            };

            signClient = await SignClient.Init(signClientOptions);
            await signClient.AddressProvider.LoadDefaultsAsync();

            var optionalNamespace = new ProposedNamespace // todo using optional namespaces like AppKit does, should they be required?
            {
                Chains = chainConfigSet.Configs
                    .Select(chainEntry => chainEntry.ChainId)
                    .ToArray(),
                Methods = new[]
                {
                    "eth_sign",
                    "personal_sign",
                    "eth_signTypedData",
                    "eth_signTransaction",
                    "eth_sendTransaction",
                    "eth_getTransactionByHash",
                    "wallet_switchEthereumChain",
                    "eth_blockNumber",
                },
                Events = new[]
                {
                    "chainChanged",
                    "accountsChanged",
                },
            };

            optionalNamespaces = new Dictionary<string, ProposedNamespace>
            {
                { ChainModel.EvmNamespace, optionalNamespace },
            };

            initialized = true;
        }

        private void ValidateConfig()
        {
            if (string.IsNullOrWhiteSpace(config.ProjectId))
            {
                throw new ReownIntegrationException("ProjectId was not set.");
            }

            if (config.ConnectionHandlerProvider == null)
            {
                throw new ReownIntegrationException($"No {nameof(IConnectionHandlerProvider)} was provided in the config.");
            }

            if (string.IsNullOrWhiteSpace(config.Metadata.Url))
            {
                throw new ReownIntegrationException("Your domain URL should be provided in Metadata, otherwise wallets are going to reject the connection.");
            }
        }

        public ValueTask WillStopAsync()
        {
            signClient?.Dispose();

            return new ValueTask(Task.CompletedTask);
        }

        public override async Task<string> Connect()
        {
            if (connected)
            {
                throw new ReownIntegrationException(
                    $"Tried connecting with {nameof(ReownProvider)}, but was already connected.");
            }

            if (!initialized)
            {
                await Initialize();
            }

            try
            {
                session = !config.RememberSession || !StoredSessionAvailable
                    ? await ConnectSession()
                    : await RestoreSession();

                connectedLocalWallet = GetSessionLocalWallet();

                ReownLogger.Log(connectedLocalWallet != null
                    ? $"Local wallet connected. \"{connectedLocalWallet.Name}\" set as locally connected wallet for the current session."
                    : "Remote wallet connected.");

                var address = GetPlayerAddress();

                if (!AddressExtensions.IsPublicAddress(address))
                {
                    throw new ReownIntegrationException("Public address provided by Reown is not valid.");
                }

                connected = true;

                await CheckAndSwitchNetwork();

                return address;
            }
            catch (Exception e)
            {
                signClient.AddressProvider.DefaultSession = default; // reset saved session
                throw new ReownIntegrationException("Error occured during Reown connection process.", e);
            }
        }

        private async Task CheckAndSwitchNetwork()
        {
            var chainId = GetChainId();
            if (chainId != $"{ChainModel.EvmNamespace}:{chainConfig.ChainId}")
            {
                await SwitchChain(chainConfig.ChainId);
                UpdateSessionChainId();
            }
        }

        private void UpdateSessionChainId()
        {
            var defaultChain = session.Namespaces.Keys.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(defaultChain))
            {
                var defaultNamespace = session.Namespaces[defaultChain];
                var chains = ConvertArrayToListAndRemoveFirst(defaultNamespace.Chains);
                defaultNamespace.Chains = chains.ToArray();
                var accounts = ConvertArrayToListAndRemoveFirst(defaultNamespace.Accounts);
                defaultNamespace.Accounts = accounts.ToArray();
            }
            else
            {
                throw new Web3Exception("Can't update session chain ID. Default chain not found.");
            }
        }

        private List<T> ConvertArrayToListAndRemoveFirst<T>(T[] array)
        {
            var list = array.ToList();
            list.RemoveAt(0);
            return list;
        }

        public override async Task Disconnect()
        {
            if (!connected)
            {
                return;
            }

            ReownLogger.Log("Disconnecting Reown session...");

            try
            {
                await signClient.Disconnect(session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
                await signClient.CoreClient.Storage.Clear();
                connected = false;
            }
            catch (Exception e)
            {
                ReownLogger.LogError($"Error occured during disconnect: {e}");
            }
        }

        private async Task<SessionStruct> ConnectSession()
        {
            var connectOptions = new ConnectOptions { OptionalNamespaces = optionalNamespaces };
            var connectedData = await signClient.Connect(connectOptions);
            var connectionHandler = await config.ConnectionHandlerProvider.ProvideHandler();

            try
            {
                connectionHandlerConfig = new ConnectionHandlerConfig
                {
                    ConnectRemoteWalletUri = connectedData.Uri,
                    DelegateLocalWalletSelectionToOs = OsManageWalletSelection,
                    WalletLocationOption = config.WalletLocationOption,

                    LocalWalletOptions = !OsManageWalletSelection
                        ? walletRegistry.SupportedWallets.ToList()
                        : null,

                    RedirectToWallet = !OsManageWalletSelection
                        ? walletId => redirection.RedirectConnection(connectedData.Uri, walletId)
                        : null,

                    RedirectOsManaged = OsManageWalletSelection
                        ? () => redirection.RedirectConnectionOsManaged(connectedData.Uri)
                        : null,
                };
                var dialogTask = connectionHandler.ConnectUserWallet(connectionHandlerConfig);

                // awaiting handler task to catch exceptions, actually awaiting only for approval
                var combinedTasks = await Task.WhenAny(dialogTask, connectedData.Approval);

                if (combinedTasks.IsFaulted)
                {
                    await combinedTasks; // this will throw the exception
                }
            }
            finally
            {
                try
                {
                    connectionHandler.Terminate();
                }
                catch
                {
                    // ignored
                }
            }

            var newSession = await connectedData.Approval;

            ReownLogger.Log("Wallet connected using new session.");

            return newSession;
        }

        private async Task<SessionStruct> RestoreSession()
        {
            session = signClient.AddressProvider.DefaultSession;

            if (SessionExpired(session))
            {
                await RenewSession();
            }

            ReownLogger.Log("Wallet connected using stored session.");

            return session;
        }

        private async Task RenewSession()
        {
            try
            {
                var acknowledgement = await signClient.Extend(session.Topic);
                TryRedirectToWallet();
                await acknowledgement.Acknowledged();
            }
            catch (Exception e)
            {
                throw new ReownIntegrationException("Session extension failed.", e);
            }

            ReownLogger.Log("Renewed session successfully.");
        }

        public override async Task<T> Request<T>(string method, params object[] parameters)
        {
            if (!connected)
            {
                throw new ReownIntegrationException("Can't send requests. No session is connected at the moment.");
            }

            if (SessionExpired(session))
            {
                if (config.AutoRenewSession)
                {
                    await RenewSession();
                }
                else
                {
                    throw new ReownIntegrationException(
                        $"Failed to perform {typeof(T)} request - session expired. Please reconnect.");
                }
            }

            var sessionTopic = session.Topic;

            EventUtils.ListenOnce<PublishParams>(
                OnPublishedMessage,
                handler => signClient.CoreClient.Relayer.Publisher.OnPublishedMessage += handler,
                handler => signClient.CoreClient.Relayer.Publisher.OnPublishedMessage -= handler);

            var chainId = GetChainId();

            return await ReownRequest<T>(sessionTopic, method, chainId, parameters);

            void OnPublishedMessage(object sender, PublishParams args)
            {
                if (args.Topic != sessionTopic)
                {
                    logWriter.LogError("Session topic is different than args -> " +
                                       $"sessionTopic: {sessionTopic}, args.Topic: {args.Topic}");
                    return;
                }

                if (connectedLocalWallet != null)
                {
                    redirection.Redirect(connectedLocalWallet.Id);
                }
            }
        }

        private WalletModel GetSessionLocalWallet()
        {
            var nativeUrl = RemoveSlash(session.Peer.Metadata.Url);

            var sessionWallet = walletRegistry
                .SupportedWallets
                .FirstOrDefault(w => RemoveSlash(w.Homepage) == nativeUrl);

            return sessionWallet;

            string RemoveSlash(string s)
            {
                return s.EndsWith('/')
                    ? s[..s.LastIndexOf('/')]
                    : s;
            }
        }

        private void TryRedirectToWallet()
        {
            if (connectedLocalWallet == null)
            {
                return;
            }

            var sessionLocalWallet = GetSessionLocalWallet();
            redirection.Redirect(sessionLocalWallet);
        }

        private string GetPlayerAddress()
        {
            return GetFullAddress().Split(":")[2];
        }

        private string GetChainId()
        {
            return string.Join(":", GetFullAddress().Split(":").Take(2));
        }

        private string GetFullAddress()
        {
            var defaultChain = session.Namespaces.Keys.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(defaultChain))
            {
                throw new Web3Exception("Can't get full address. Default chain not found.");
            }

            var defaultNamespace = session.Namespaces[defaultChain];

            if (defaultNamespace.Accounts.Length == 0)
            {
                throw new Web3Exception("Can't get full address. No connected accounts.");
            }

            return defaultNamespace.Accounts[0];
        }

        private async Task<T> ReownRequest<T>(string topic, string method, string chainId, params object[] parameters)
        {
            // Helper method to make a request using ReownSignClient.
            async Task<T> MakeRequest<TRequest>()
            {
                var data = (TRequest)Activator.CreateInstance(typeof(TRequest), parameters);
                return await signClient.Request<TRequest, T>(topic, data, chainId);
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
                    return await MakeRequest<WalletSwitchEthereumChain>();
                default:
                    try
                    {
                        // Direct RPC request via http, Reown RPC url.
                        string chain = session.Namespaces.First().Value.Chains[0];

                        // Using Reown Blockchain API: https://docs.Reown.com/cloud/blockchain-api
                        var url = $"https://rpc.walletconnect.com/v1?chainId={chain}&projectId={config.ProjectId}";

                        string body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters));

                        var rawResult = await reownHttpClient.PostRaw(url, body, "application/json");

                        RpcResponseMessage response = JsonConvert.DeserializeObject<RpcResponseMessage>(rawResult.Response);

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