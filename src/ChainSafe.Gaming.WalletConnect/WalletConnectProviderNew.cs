using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Environment;
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

namespace ChainSafe.Gaming.WalletConnect
{
    // todo testing?
    public class WalletConnectProviderNew : ILifecycleParticipant, IWalletConnectProviderNew
    {
        private readonly ILogWriter logWriter;
        private readonly IWalletConnectConfigNew config;
        private readonly IDataStorage storage;
        private readonly IChainConfig chainConfig;
        private readonly IOperatingSystemMediator osMediator;
        private readonly IWalletRegistry walletRegistry;
        private readonly RedirectionHandler redirection;

        private WalletConnectCore core;
        private WalletConnectSignClient signClient;
        private RequiredNamespaces requiredNamespaces;
        private LocalData localData;
        private SessionStruct session;
        private bool connected;

        public WalletConnectProviderNew(
            IWalletConnectConfigNew config,
            IDataStorage storage,
            ILogWriter logWriter,
            IChainConfig chainConfig,
            IOperatingSystemMediator osMediator,
            IWalletRegistry walletRegistry,
            RedirectionHandler redirection)
        {
            this.redirection = redirection;
            this.walletRegistry = walletRegistry;
            this.osMediator = osMediator;
            this.chainConfig = chainConfig;
            this.storage = storage;
            this.config = config;
            this.logWriter = logWriter;
        }

        public bool CanAutoLogin => localData.SessionTopic != null;

        private bool OsManageWalletSelection => osMediator.Platform == Platform.Android;

        private static bool SessionExpired(SessionStruct s) => s.Expiry != null && Clock.IsExpired((long)s.Expiry);

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            // todo assert required config fields

            WCLogger.Logger = new WCLogWriter(logWriter);

            localData = await storage.LoadLocalData() ?? new LocalData();

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
                            "eth_sendTransaction",
                            "eth_signTransaction",
                            "eth_sign",
                            "personal_sign",
                            "eth_signTypedData",
                        },
                    }
                },
            };
        }

        async ValueTask ILifecycleParticipant.WillStopAsync() => new ValueTask(Task.CompletedTask);

        public async Task<string> Connect()
        {
            if (connected)
            {
                throw new Web3Exception(
                    $"Tried connecting {nameof(WalletConnectProviderNew)}, but it's already been connected.");
            }

            var connectOptions = new ConnectOptions { RequiredNamespaces = requiredNamespaces };
            var sessionStored = !string.IsNullOrEmpty(localData.SessionTopic);

            session = !sessionStored
                ? await ConnectNewSession(connectOptions)
                : await ConnectStoredSession(connectOptions);

            localData.SessionTopic = session.PairingTopic;

            var connectedLocally = session.Peer.Metadata.Redirect == null;
            if (connectedLocally)
            {
                var sessionLocalWallet = GetSessionLocalWallet();
                localData.ConnectedLocalWalletName = sessionLocalWallet.Name;
                WCLogger.Log($"\"{sessionLocalWallet.Name}\" set as locally connected wallet for current session.");
            }
            else
            {
                localData.ConnectedLocalWalletName = null;
            }

            if (config.RememberSession)
            {
                await storage.SaveLocalData(localData);
            }
            else
            {
                // Clear stored data
                storage.ClearLocalData();
            }

            var address = GetPlayerAddress();

            if (!AddressExtensions.IsPublicAddress(address))
            {
                throw new Web3Exception("Public address provided by WalletConnect is not valid.");
            }

            connected = true;

            return address;
        }

        public async Task Disconnect()
        {
            if (!connected)
            {
                return;
            }

            WCLogger.Log("Disconnecting Wallet Connect session...");

            try
            {
                if (signClient != null)
                {
                    await signClient.Disconnect(session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
                }

                if (core != null)
                {
                    await core.Storage.Clear();
                }

                signClient?.Dispose();
                core?.Dispose();
            }
            catch (Exception e)
            {
                WCLogger.LogError($"Error occured during disconnect: {e}");
            }
        }

        private async Task<SessionStruct> ConnectNewSession(ConnectOptions connectOptions)
        {
            var connectedData = await signClient.Connect(connectOptions);
            var connectionDialog = await config.ConnectionDialogProvider.ProvideDialog();

            try
            {
                var osManageWalletSelection = OsManageWalletSelection;
                var dialogTask = connectionDialog.ShowAndConnectUserWallet(new ConnectionDialogConfig
                {
                    ConnectRemoteWalletUri = connectedData.Uri,
                    DelegateLocalWalletSelectionToOs = osManageWalletSelection,
                    LocalWalletOptions = !osManageWalletSelection
                        ? walletRegistry.EnumerateSupportedWallets(osMediator.Platform).ToList() // todo notify devs that some wallets don't work on Desktop
                        : null,
                    WalletLocationOptions = config.WalletLocationOptions,
                    RedirectToWallet = walletName => redirection.RedirectConnection(connectedData.Uri, walletName),
                    RedirectOsManaged = () => redirection.RedirectConnectionOsManaged(connectedData.Uri),
                });

                // awaiting dialog task to catch exceptions, actually awaiting only for approval
                await Task.WhenAny(dialogTask, connectedData.Approval);
            }
            finally
            {
                connectionDialog.CloseDialog();
            }

            var newSession = await connectedData.Approval;

            WCLogger.Log("Wallet connected using new session.");

            return newSession;
        }

        private async Task<SessionStruct> ConnectStoredSession(ConnectOptions connectOptions)
        {
            var storedSession = signClient.Find(requiredNamespaces).First(s => s.Topic == localData.SessionTopic);
            connectOptions.PairingTopic = storedSession.PairingTopic;
            await signClient.Connect(connectOptions); // connect using existing session

            // var connectedData = await signClient.Connect(connectOptions);
            // OpenLocalWalletIfRegistered(); // todo do we need to approve anything in wallet in this case?
            // await connectedData.Approval;

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
                TryRedirectToWallet(); // todo swapped this with 'await acknowledgement.Acknowledged()', test if it works in build
                await acknowledgement.Acknowledged();
            }
            catch (Exception e)
            {
                throw new Web3Exception("Auto-renew session failed.", e);
            }

            WCLogger.Log("Renewed session successfully.");
        }

        public async Task<string> Request<T>(T data, long? expiry = null)
        {
            if (SessionExpired(session))
            {
                if (config.AutoRenewSession)
                {
                    await RenewSession();
                }
                else
                {
                    throw new Web3Exception(
                        $"Failed to perform {typeof(T)} request - session expired. Please reconnect.");
                }
            }

            var method = RpcMethodAttribute.MethodForType<T>();
            var methodRegistered = session.Namespaces.Any(n => n.Value.Methods.Contains(method));

            if (!methodRegistered)
            {
                throw new Web3Exception(
                    "The method provided is not supported. " +
                    $"If you add a new method you have to update {nameof(WalletConnectProviderNew)} code to reflect those changes. " +
                    "Contact ChainSafe if you think a specific method should be included in the SDK.");
            }

            var sessionTopic = session.Topic;

            EventUtils.ListenOnce<PublishParams>(
                OnPublishedMessage,
                handler => core.Relayer.Publisher.OnPublishedMessage += handler,
                handler => core.Relayer.Publisher.OnPublishedMessage -= handler);

            var chainId = GetChainId();

            return await signClient.Request<T, string>(sessionTopic, data, chainId, expiry);

            void OnPublishedMessage(object sender, PublishParams args)
            {
                if (args.Topic != sessionTopic)
                {
                    return;
                }

                if (localData.ConnectedLocalWalletName != null)
                {
                    redirection.Redirect(localData.ConnectedLocalWalletName);
                }
            }
        }

        private WalletConnectWalletModel GetSessionLocalWallet()
        {
            var nativeUrl = RemoveSlash(session.Peer.Metadata.Url);
            // var dividerIndex = nativeUrl.IndexOf(':');
            // if (dividerIndex != -1)
            // {
            //     nativeUrl = $"{nativeUrl[..dividerIndex]}:";
            // }

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
    }
}