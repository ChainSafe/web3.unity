using System;
using System.IO;
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
using WalletConnectSharp.Core.Models.Relay;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectCustomProvider : IWalletConnectCustomProvider, ILifecycleParticipant
    {
        private readonly IOperatingSystemMediator operatingSystem;
        private readonly ILogWriter logWriter;
        private readonly WalletConnectConfig config;

        public WalletConnectCustomProvider(WalletConnectConfig config, IOperatingSystemMediator operatingSystem, ILogWriter logWriter)
        {
            this.operatingSystem = operatingSystem;
            this.config = config;
            this.logWriter = logWriter;
        }

        // static to not destroy client session on logout/TerminateAsync, just disconnect instead
        public static WalletConnectSignClient SignClient { get; private set; }

        public WalletConnectCore Core { get; private set; }

        public SessionStruct Session { get; private set; }

        public ConnectedData ConnectedData { get; private set; }

        private bool SessionExpired => Session.Expiry != null && Clock.IsExpired((long)Session.Expiry);

        public ValueTask WillStartAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        private async Task Initialize()
        {
            if (SignClient != null)
            {
                Core = (WalletConnectCore)SignClient.Core;
            }

            if (Core != null && Core.Initialized)
            {
                WCLogger.Log("Core already initialized");

                return;
            }

            WCLogger.Logger = new WCLogWriter(logWriter);

            Core = new WalletConnectCore(new CoreOptions()
            {
                Name = config.ProjectName,
                ProjectId = config.ProjectId,
                Storage = BuildStorage(),
                BaseContext = config.BaseContext,
            });

            await Core.Start();

            SignClient = await WalletConnectSignClient.Init(new SignClientOptions()
            {
                BaseContext = config.BaseContext,
                Core = Core,
                Metadata = config.Metadata,
                Name = config.ProjectName,
                ProjectId = config.ProjectId,
                Storage = Core.Storage,
            });
        }

        // Connect to wallet and return address of connected wallet.
        public async Task<string> Connect()
        {
            await Initialize();

            RequiredNamespaces requiredNamespaces = new RequiredNamespaces();

            var methods = new string[]
            {
                "eth_sendTransaction", "eth_signTransaction", "eth_sign", "personal_sign", "eth_signTypedData",
            };

            var events = new string[] { "chainChanged", "accountsChanged" };

            requiredNamespaces.Add(
                ChainModel.EvmNamespace,
                new ProposedNamespace
                {
                    Chains = new string[]
                    {
                        config.Chain.FullChainId,
                    },
                    Events = events,
                    Methods = methods,
                });

            var connectOptions = new ConnectOptions
            {
                RequiredNamespaces = requiredNamespaces,
            };

            // if there's a saved session pair and continue with that session/connect automatically
            bool autoConnect = !string.IsNullOrEmpty(config.SavedSessionTopic);

            if (autoConnect)
            {
                try
                {
                    // try and restore session
                    Session = SignClient.Find(requiredNamespaces).First(s => s.Topic == config.SavedSessionTopic);

                    connectOptions.PairingTopic = Session.PairingTopic;
                }
                catch (Exception)
                {
                    throw new Web3Exception($"Failed to restore session topic {config.SavedSessionTopic}");
                }
            }

            // start connecting
            ConnectedData = await SignClient.Connect(connectOptions);

            config.InvokeConnected(ConnectedData);

            // open deeplink to redirect to wallet for connection
            if (config.RedirectToWallet)
            {
                if (autoConnect)
                {
                    // try and open wallet for session renewal
                    config.DefaultWallet?.OpenWallet(operatingSystem);
                }
                else if (config.DefaultWallet != null)
                {
                    try
                    {
                        config.DefaultWallet.OpenDeeplink(ConnectedData, operatingSystem);
                    }
                    catch (Exception e)
                    {
                        throw new Web3Exception(
                            $"Failed Redirecting to {config.DefaultWallet.Name} Wallet, Failed with Exception : {e}");
                    }
                }
                else
                {
                    operatingSystem.OpenUrl(ConnectedData.Uri);
                }
            }

            if (autoConnect)
            {
                ConnectedData.Approval = Task.FromResult(Session);
            }

            Session = await ConnectedData.Approval;

            WCLogger.Log($"Wallet Connect session {Session.Topic} approved");

            if (SessionExpired)
            {
                await TryRenewSession();
            }

            config.InvokeSessionApproved(Session);

            // get default wallet
            if (config.RedirectToWallet && config.DefaultWallet == null)
            {
                string nativeUrl = Session.Peer.Metadata.Redirect.Native.Replace("//", string.Empty);

                int index = nativeUrl.IndexOf(':');

                if (index != -1)
                {
                    nativeUrl = $"{nativeUrl.Substring(0, index)}:";
                }

                WCLogger.Log($"Wallet Native Url {nativeUrl}");

                var defaultWallet = config.SupportedWallets.Values.FirstOrDefault(w =>
                    w.Mobile.NativeProtocol == nativeUrl || w.Desktop.NativeProtocol == nativeUrl);

                if (defaultWallet != null)
                {
                    config.DefaultWallet = defaultWallet;

                    WCLogger.Log("Default Wallet Set");
                }
                else
                {
                    WCLogger.Log("Default Wallet Not Found in Supported Wallets");
                }
            }

            // get address
            var addressParts = GetFullAddress().Split(":");

            string address = addressParts[2];

            if (!AddressExtensions.IsPublicAddress(address))
            {
                throw new Web3Exception(
                    $"Public address recovered from signature is not valid. Public address: {address}");
            }

            return address;
        }

        public ValueTask WillStopAsync()
        {
            if (!config.KeepSessionAlive)
            {
                // disconnect on terminate
                return new ValueTask(Disconnect());
            }

            return new ValueTask(Task.CompletedTask);
        }

        public async Task<string> Request<T>(T data, long? expiry = null)
        {
            // if testing skip making request
            if (config.Testing)
            {
                return config.TestResponse;
            }

            if (SessionExpired)
            {
                if (config.KeepSessionAlive)
                {
                    await TryRenewSession();
                }
                else
                {
                    throw new Web3Exception($"Failed to perform {typeof(T)} Request, Session expired, Please Reconnect");
                }
            }

            string topic = Session.Topic;

            var addressParts = GetFullAddress().Split(":");

            string chainId = string.Join(':', addressParts.Take(2));

            string method = RpcMethodAttribute.MethodForType<T>();

            // if it's a registered method try and open wallet
            if (Session.Namespaces.Any(n => n.Value.Methods.Contains(method)))
            {
                Core.Relayer.Events.ListenForOnce<object>(
                    RelayerEvents.Publish,
                    (_, _) =>
                    {
                        // if default wallet exists and redirect is true redirect user to default wallet
                        if (config.RedirectToWallet && config.DefaultWallet != null)
                        {
                            WCLogger.Log("Opening Default Wallet...");

                            config.DefaultWallet.OpenWallet(operatingSystem);
                        }
                        else
                        {
                            WCLogger.Log("No Default Wallet to Open");
                        }
                    });
            }

            return await SignClient.Request<T, string>(topic, data, chainId, expiry);
        }

        private async Task TryRenewSession()
        {
            WCLogger.Log("Attempting to renew Session...");

            try
            {
                var acknowledgement = await SignClient.Extend(Session.Topic);

                await acknowledgement.Acknowledged();

                // try to open default wallet to approve session renewal
                if (config.RedirectToWallet)
                {
                    config.DefaultWallet?.OpenWallet(operatingSystem);
                }
            }
            catch (Exception e)
            {
                throw new Web3Exception($"Auto Renew Session Failed with Exception : {e}");
            }
        }

        private string GetFullAddress()
        {
            var defaultChain = Session.Namespaces.Keys.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(defaultChain))
            {
                throw new Web3Exception("can't get full address, default chain not found");
            }

            var defaultNamespace = Session.Namespaces[defaultChain];

            if (defaultNamespace.Accounts.Length == 0)
            {
                throw new Web3Exception("can't get full address, no connected accounts");
            }

            return defaultNamespace.Accounts[0];
        }

        private FileSystemStorage BuildStorage()
        {
            var path = Path.Combine(config.StoragePath, "walletconnect.json");

            WCLogger.Log($"Wallet Connect Storage set to {path}");

            return new FileSystemStorage(path);
        }

        public async Task Disconnect()
        {
            WCLogger.Log("Disconnecting Wallet Connect session...");

            try
            {
                await SignClient.Disconnect(Session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));

                await Core.Storage.Clear();
            }
            catch (Exception e)
            {
                WCLogger.LogError($"error disconnecting: {e}");
            }
        }
    }
}