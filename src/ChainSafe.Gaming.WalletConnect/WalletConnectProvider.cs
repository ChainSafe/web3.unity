using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.WalletConnect.Connection;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.WalletConnect.Storage;
using ChainSafe.Gaming.WalletConnect.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Common.Model.Errors;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Models.Publisher;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using BigInteger = System.Numerics.BigInteger;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Default implementation of <see cref="IWalletProvider"/>.
    /// </summary>
    public class WalletConnectProvider : WalletProvider, ILifecycleParticipant, IConnectionHelper
    {
        private readonly ILogWriter logWriter;
        private readonly IWalletConnectConfig config;
        private readonly DataStorage storage;
        private readonly IChainConfig chainConfig;
        private readonly IOperatingSystemMediator osMediator;
        private readonly IWalletRegistry walletRegistry;
        private readonly RedirectionHandler redirection;
        private readonly IHttpClient httpClient;
        private readonly IAnalyticsClient analyticsClient;

        private WalletConnectCore core;
        private WalletConnectSignClient signClient;
        private RequiredNamespaces requiredNamespaces;
        private LocalData localData;
        private SessionStruct session;
        private bool connected;
        private bool initialized;
        private ConnectionHandlerConfig connectionHandlerConfig;

        public WalletConnectProvider(
            IWalletConnectConfig config,
            DataStorage storage,
            IChainConfig chainConfig,
            IWalletRegistry walletRegistry,
            RedirectionHandler redirection,
            Web3Environment environment)
            : base(environment, chainConfig)
        {
            analyticsClient = environment.AnalyticsClient;
            this.redirection = redirection;
            this.walletRegistry = walletRegistry;
            osMediator = environment.OperatingSystem;
            this.chainConfig = chainConfig;
            this.storage = storage;
            this.config = config;
            logWriter = environment.LogWriter;
            httpClient = environment.HttpClient;
        }

        public bool StoredSessionAvailable => localData.SessionTopic != null;

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
                EventName = "Wallet Connect Initialized",
                PackageName = "io.chainsafe.web3-unity",
            });

            ValidateConfig();

            WCLogger.Logger = new WCLogWriter(logWriter, config);

            localData = !config.ForceNewSession
                ? await storage.LoadLocalData() ?? new LocalData()
                : new LocalData();

            core = new WalletConnectCore(new CoreOptions
            {
                Name = config.ProjectName,
                ProjectId = config.ProjectId,
                Storage = storage.BuildStorage(sessionStored: !string.IsNullOrEmpty(localData.SessionTopic)),
                BaseContext = config.BaseContext,
                ConnectionBuilder = config.ConnectionBuilder,
            });

            await core.Start();

            signClient = await WalletConnectSignClient.Init(new SignClientOptions
            {
                BaseContext = config.BaseContext,
                Core = core,
                Metadata = config.Metadata,
                Name = config.ProjectName,
                ProjectId = config.ProjectId,
                Storage = core.Storage,
            });

            requiredNamespaces = new RequiredNamespaces
            {
                {
                    ChainModel.EvmNamespace,
                    new ProposedNamespace
                    {
                        Chains = new[] { $"{ChainModel.EvmNamespace}:{chainConfig.ChainId}", },
                        Events = new[] { "chainChanged", "accountsChanged" },
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
                    }
                },
            };

            initialized = true;
        }

        private void ValidateConfig()
        {
            if (string.IsNullOrWhiteSpace(config.ProjectId))
            {
                throw new WalletConnectException("ProjectId was not set.");
            }

            if (string.IsNullOrWhiteSpace(config.StoragePath))
            {
                throw new WalletConnectException("Storage folder path was not set.");
            }
        }

        public ValueTask WillStopAsync()
        {
            signClient?.Dispose();
            core?.Dispose();

            return new ValueTask(Task.CompletedTask);
        }

        public override async Task<string> Connect()
        {
            if (connected)
            {
                throw new WalletConnectException(
                    $"Tried connecting {nameof(WalletConnectProvider)}, but it's already been connected.");
            }

            if (!initialized)
            {
                await Initialize();
            }

            try
            {
                var sessionStored = !string.IsNullOrEmpty(localData.SessionTopic);

                session = !sessionStored
                    ? await ConnectSession()
                    : await RestoreSession();

                localData.SessionTopic = session.Topic;

                var sessionLocalWallet = GetSessionLocalWallet();
                if (sessionLocalWallet != null)
                {
                    localData.ConnectedLocalWalletName = sessionLocalWallet.Name;
                    WCLogger.Log($"\"{sessionLocalWallet.Name}\" set as locally connected wallet for the current session.");
                }
                else
                {
                    localData.ConnectedLocalWalletName = null;
                    WCLogger.Log("Remote wallet connected.");
                }

                if (config.RememberSession)
                {
                    await storage.SaveLocalData(localData);
                }
                else
                {
                    storage.ClearLocalData();
                }

                var address = GetPlayerAddress();

                if (!AddressExtensions.IsPublicAddress(address))
                {
                    throw new Web3Exception("Public address provided by WalletConnect is not valid.");
                }

                connected = true;

                await CheckAndSwitchNetwork();

                return address;
            }
            catch (Exception e)
            {
                storage.ClearLocalData();
                throw new WalletConnectException("Error occured during WalletConnect connection process.", e);
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

            WCLogger.Log("Disconnecting Wallet Connect session...");

            try
            {
                storage.ClearLocalData();
                localData = new LocalData();
                await signClient.Disconnect(session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
                await core.Storage.Clear();
                connected = false;
            }
            catch (Exception e)
            {
                WCLogger.LogError($"Error occured during disconnect: {e}");
            }
        }

        private async Task<SessionStruct> ConnectSession()
        {
            if (config.ConnectionHandlerProvider == null)
            {
                throw new WalletConnectException($"Can not connect to a new session. No {nameof(IConnectionHandlerProvider)} was set in config.");
            }

            var connectOptions = new ConnectOptions { RequiredNamespaces = requiredNamespaces };
            var connectedData = await signClient.Connect(connectOptions);
            var connectionHandler = await config.ConnectionHandlerProvider.ProvideHandler();

            Task dialogTask;
            try
            {
                connectionHandlerConfig = new ConnectionHandlerConfig
                {
                    ConnectRemoteWalletUri = connectedData.Uri,
                    DelegateLocalWalletSelectionToOs = OsManageWalletSelection,
                    WalletLocationOption = config.WalletLocationOption,

                    LocalWalletOptions = !OsManageWalletSelection
                        ? walletRegistry.EnumerateSupportedWallets(osMediator.Platform)
                            .ToList() // todo notify devs that some wallets don't work on Desktop
                        : null,

                    RedirectToWallet = !OsManageWalletSelection
                        ? walletName => redirection.RedirectConnection(connectedData.Uri, walletName)
                        : null,

                    RedirectOsManaged = OsManageWalletSelection
                        ? () => redirection.RedirectConnectionOsManaged(connectedData.Uri)
                        : null,
                };
                dialogTask = connectionHandler.ConnectUserWallet(connectionHandlerConfig);

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

            WCLogger.Log("Wallet connected using new session.");

            return newSession;
        }

        private async Task<SessionStruct> RestoreSession()
        {
            var storedSession = signClient.Find(requiredNamespaces).First(s => s.Topic == localData.SessionTopic);

            if (SessionExpired(storedSession))
            {
                await RenewSession();
            }

            WCLogger.Log("Wallet connected using stored session.");

            return storedSession;
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
                throw new WalletConnectException("Auto-renew session failed.", e);
            }

            WCLogger.Log("Renewed session successfully.");
        }

        public override async Task<T> Request<T>(string method, params object[] parameters)
        {
            if (!connected)
            {
                throw new WalletConnectException("Can't send requests. No session is connected at the moment.");
            }

            if (SessionExpired(session))
            {
                if (config.AutoRenewSession)
                {
                    await RenewSession();
                }
                else
                {
                    throw new WalletConnectException(
                        $"Failed to perform {typeof(T)} request - session expired. Please reconnect.");
                }
            }

            var sessionTopic = session.Topic;

            EventUtils.ListenOnce<PublishParams>(
                OnPublishedMessage,
                handler => core.Relayer.Publisher.OnPublishedMessage += handler,
                handler => core.Relayer.Publisher.OnPublishedMessage -= handler);

            var chainId = GetChainId();

            return await WalletConnectRequest<T>(sessionTopic, method, chainId, parameters);

            void OnPublishedMessage(object sender, PublishParams args)
            {
                if (args.Topic != sessionTopic)
                {
                    logWriter.LogError("Session topic is different than args -> " +
                                       $"sessionTopic: {sessionTopic}, args.Topic: {args.Topic}");
                    return;
                }

                if (localData.ConnectedLocalWalletName != null)
                {
                    redirection.Redirect(localData.ConnectedLocalWalletName);
                }
            }
        }

        private WalletModel GetSessionLocalWallet()
        {
            var nativeUrl = RemoveSlash(session.Peer.Metadata.Url);

            var sessionWallet = walletRegistry
                .EnumerateSupportedWallets(osMediator.Platform)
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
            if (localData.ConnectedLocalWalletName == null)
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

        private async Task<T> WalletConnectRequest<T>(string topic, string method, string chainId, params object[] parameters)
        {
            // Helper method to make a request using WalletConnectSignClient.
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
                        // Direct RPC request via http, WalletConnect RPC url.
                        string chain = session.Namespaces.First().Value.Chains[0];

                        // Using WalletConnect Blockchain API: https://docs.walletconnect.com/cloud/blockchain-api
                        var url = $"https://rpc.walletconnect.com/v1?chainId={chain}&projectId={config.ProjectId}";

                        string body = JsonConvert.SerializeObject(new RpcRequestMessage(Guid.NewGuid().ToString(), method, parameters));

                        var rawResult = await httpClient.PostRaw(url, body, "application/json");

                        RpcResponseMessage response = JsonConvert.DeserializeObject<RpcResponseMessage>(rawResult.Response);

                        return response.Result.ToObject<T>();
                    }
                    catch (Exception e)
                    {
                        throw new WalletConnectException($"{method} RPC method currently not implemented.", e);
                    }
            }
        }
    }
}