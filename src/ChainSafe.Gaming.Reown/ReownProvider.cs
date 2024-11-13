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
using ChainSafe.Gaming.Web3.Core.Operations;
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
        private const string EvmNamespace = "eip155";

        private readonly ILogWriter logWriter;
        private readonly IReownConfig config;
        private readonly IChainConfig chainConfig;
        private readonly IOperatingSystemMediator osMediator;
        private readonly IWalletRegistry walletRegistry;
        private readonly RedirectionHandler redirection;
        private readonly ReownHttpClient reownHttpClient;
        private readonly IAnalyticsClient analyticsClient;
        private readonly Web3Environment environment;
        private readonly IChainConfigSet chainConfigSet;
        private readonly IOperationTracker operationTracker;

        private SessionStruct session;
        private bool connected;
        private bool initialized;
        private ConnectionHandlerConfig connectionHandlerConfig;
        private Dictionary<string, ProposedNamespace> optionalNamespaces;
        private WalletModel sessionWallet;

        public ReownProvider(
            IReownConfig config,
            IChainConfig chainConfig,
            IChainConfigSet chainConfigSet,
            IWalletRegistry walletRegistry,
            RedirectionHandler redirection,
            Web3Environment environment,
            ReownHttpClient reownHttpClient,
            IOperationTracker operationTracker)
            : base(environment, chainConfig, operationTracker)
        {
            this.operationTracker = operationTracker;
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

        public SignClient SignClient { get; private set; }

        public bool StoredSessionAvailable
        {
            get
            {
                if (!SignClient.AddressProvider.HasDefaultSession)
                {
                    return false; // no session stored
                }

                if (string.IsNullOrWhiteSpace(SignClient.AddressProvider.DefaultSession.Topic))
                {
                    return false; // session topic is empty
                }

                if (!SignClient.Session.Keys.Contains(SignClient.AddressProvider.DefaultSession.Topic))
                {
                    return false; // usually happens when session was closed on the wallet side
                }

                return true;
            }
        }

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

            config.Validate();

            ReownLogger.Instance = new ReownLogWriter(logWriter, config);

            using (operationTracker.TrackOperation("Initializing the Reown module..."))
            {
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

                SignClient = await SignClient.Init(signClientOptions);
                await SignClient.AddressProvider.LoadDefaultsAsync();
                
                if (config.OnRelayErrored is not null)
                {
                    SignClient.CoreClient.Relayer.OnErrored += config.OnRelayErrored;
                }

                var optionalNamespace =
                    new ProposedNamespace // todo using optional namespaces like AppKit does, should they be required?
                    {
                        Chains = chainConfigSet.Configs
                            .Select(chainEntry => BuildChainIdForReown(chainEntry.ChainId))
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
                    { EvmNamespace, optionalNamespace },
                };

                initialized = true;
            }
        }

        public ValueTask WillStopAsync()
        {
            SignClient?.Dispose();

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

                var address = GetPlayerAddress();

                if (!AddressExtensions.IsPublicAddress(address))
                {
                    throw new ReownIntegrationException("Public address provided by Reown is not valid.");
                }

                sessionWallet = GetSessionWallet();

                if (sessionWallet is null)
                {
                    ReownLogger.Log("Couldn't identify the wallet used to connect the session. " +
                                         "Redirection is disabled. " +
                                         $"URL from wallet metadata is \"{session.Peer.Metadata.Url}\".");
                }

                connected = true;

                await CheckAndSwitchNetwork();

                return address;
            }
            catch (Exception e)
            {
                SignClient.AddressProvider.DefaultSession = default; // reset saved session
                await SignClient.CoreClient.Storage.Clear();

                throw new ReownIntegrationException("Error occured during Reown connection process.", e);
            }
        }

        private async Task CheckAndSwitchNetwork()
        {
            var chainId = ExtractChainIdFromAddress();
            if (chainId == $"{EvmNamespace}:{chainConfig.ChainId}")
            {
                return;
            }

            const int maxSwitchAttempts = 3;

            for (var i = 0;;)
            {
                var messageToUser = i == 0
                    ? "Switching wallet network..."
                    : $"Switching wallet network (attempt {i + 1})...";

                using (operationTracker.TrackOperation(messageToUser))
                {
                    try
                    {
                        await SwitchChain(chainConfig.ChainId);
                        UpdateSessionChainId();
                        return; // success, exit loop
                    }
                    catch (ReownNetworkException)
                    {
                        if (++i >= maxSwitchAttempts)
                        {
                            throw;
                        }

                        logWriter.Log("Attempted to switch the network, but was rejected. Trying again...");
                    }
                }
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

        private string BuildChainIdForReown(string chainId)
        {
            return $"{EvmNamespace}:{chainId}";
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
                await SignClient.Disconnect(session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
                await SignClient.CoreClient.Storage.Clear();
                connected = false;
            }
            catch (Exception e)
            {
                ReownLogger.LogError($"Error occured during disconnect: {e}");
            }
        }

        private async Task<SessionStruct> ConnectSession()
        {
            ConnectedData connectedData;
            IConnectionHandler connectionHandler;

            using (operationTracker.TrackOperation("Initializing a new Reown session..."))
            {
                var connectOptions = new ConnectOptions { OptionalNamespaces = optionalNamespaces };
                connectedData = await SignClient.Connect(connectOptions);
                connectionHandler = await config.ConnectionHandlerProvider.ProvideHandler();
            }

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

                    HttpHeaders = reownHttpClient.BuildHeaders(),

                    WalletIconEndpoint = $"{ReownHttpClient.Host}/getWalletImage/",

                    RedirectToWallet = !OsManageWalletSelection
                        ? OnRedirectToWallet
                        : null,

                    RedirectOsManaged = OsManageWalletSelection
                        ? () => redirection.RedirectConnectionOsManaged(connectedData.Uri)
                        : null,
                };

                void OnRedirectToWallet(string walletId)
                {
                    SignClient.CoreClient.Storage.SetItem("ChainSafe_RecentLocalWalletId", walletId); // saving wallet id to enable future redirection
                    redirection.RedirectConnection(connectedData.Uri, walletId);
                }

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
            session = SignClient.AddressProvider.DefaultSession;

            if (SessionExpired(session))
            {
                await RenewSession();
            }

            ReownLogger.Log("Wallet connected using stored session.");

            return session;
        }

        private async Task RenewSession()
        {
            using (operationTracker.TrackOperation("Renewing the Reown session..."))
            {
                try
                {
                    var acknowledgement = await SignClient.Extend(session.Topic);
                    TryRedirectToWallet();
                    await acknowledgement.Acknowledged();
                }
                catch (Exception e)
                {
                    throw new ReownIntegrationException("Session renewal failed.", e);
                }
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
                handler => SignClient.CoreClient.Relayer.Publisher.OnPublishedMessage += handler,
                handler => SignClient.CoreClient.Relayer.Publisher.OnPublishedMessage -= handler);

            return await ReownRequest<T>(sessionTopic, method, parameters);

            void OnPublishedMessage(object sender, PublishParams args)
            {
                if (args.Topic != sessionTopic)
                {
                    logWriter.LogError("Session topic is different than args -> " +
                                       $"sessionTopic: {sessionTopic}, args.Topic: {args.Topic}");
                    return;
                }

                TryRedirectToWallet();
            }
        }

        private WalletModel GetSessionWallet()
        {
            var nativeUrl = RemoveSlash(session.Peer.Metadata.Url);

            var wallet = walletRegistry
                .SupportedWallets
                .FirstOrDefault(w => RemoveSlash(w.Homepage) == nativeUrl);

            return wallet;

            string RemoveSlash(string s)
            {
                return s.EndsWith('/')
                    ? s[..s.LastIndexOf('/')]
                    : s;
            }
        }

        private async void TryRedirectToWallet()
        {
            if (sessionWallet is null)
            {
                return; // session wallet couldn't be determined, ignore redirection
            }

            if (!await SignClient.CoreClient.Storage.HasItem("ChainSafe_RecentLocalWalletId"))
            {
                return; // no local wallets connected - ignore redirection
            }

            var recentLocalWalletId = await SignClient.CoreClient.Storage.GetItem<string>("ChainSafe_RecentLocalWalletId");

            if (recentLocalWalletId != sessionWallet.Id)
            {
                ReownLogger.Log("Last clicked local wallet was not used to connect the session. " +
                                "Assuming the wallet was connected remotely. No redirection is going to happen.");
                return;
            }

            redirection.Redirect(sessionWallet); // safe to redirect
        }

        private string GetPlayerAddress()
        {
            return GetFullAddress().Split(":")[2];
        }

        private string ExtractChainIdFromAddress()
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

        private async Task<T> ReownRequest<T>(string topic, string method, params object[] parameters)
        {
            // Helper method to make a request using ReownSignClient.
            async Task<T> MakeRequest<TRequest>(bool sendChainId = true)
            {
                var data = (TRequest)Activator.CreateInstance(typeof(TRequest), parameters);
                try
                {
                    try
                    {
                        return await SignClient.Request<TRequest, T>(
                            topic,
                            data,
                            sendChainId ? BuildChainIdForReown(chainConfig.ChainId) : null);
                    }
                    finally
                    {
#if DEBUG
                        logWriter.Log("SignClient.Request executed successfully.");
#endif
                    }
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
                default:
                    try
                    {
                        // Direct RPC request via http, Reown RPC url.
                        var chain = session.Namespaces.First().Value.Chains[0];

                        // Using Reown Blockchain API: https://docs.reown.com/cloud/blockchain-api
                        var url = $"https://rpc.walletconnect.com/v1?chainId={chain}&projectId={config.ProjectId}";

                        var body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters));
                        var rawResult = await reownHttpClient.PostRaw(url, body, "application/json");
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