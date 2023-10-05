using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Common.Model.Errors;
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
    public class WalletConnectWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private static readonly TimeSpan MinClipboardCheckPeriod = TimeSpan.FromMilliseconds(10);

        private readonly IChainConfig chainConfig;
        private readonly WebPageWalletConfig configuration;
        private readonly IOperatingSystemMediator operatingSystem;
        private readonly IRpcProvider provider;
        private readonly ILogWriter logWriter;

        public WalletConnectWallet(IRpcProvider provider, WebPageWalletConfig configuration, IOperatingSystemMediator operatingSystem, IChainConfig chainConfig, ILogWriter logWriter)
        {
            this.provider = provider;
            this.operatingSystem = operatingSystem;
            this.chainConfig = chainConfig;
            this.configuration = configuration;
            this.logWriter = logWriter;
        }

        public delegate string ConnectMessageBuildDelegate(DateTime expirationTime);

        public delegate void Connected(ConnectedData connectedData);

        public delegate void SessionApproved(SessionStruct session);

        public static event Connected OnConnected;

        public static event SessionApproved OnSessionApproved;

        public static bool Testing { get; set; } = false;

        public static string TestResponse { get; set; } = string.Empty;

        // static to not destroy client session on logout/TerminateAsync, just disconnect instead
        public static WalletConnectSignClient SignClient { get; private set; }

        public WalletConnectCore Core { get; private set; }

        public SessionStruct Session => SignClient.Session.Get(SignClient.Session.Keys[0]);

        public WalletConnectConfig Config { get; private set; }

        public string Address { get; private set; }

        private static void InvokeConnected(ConnectedData connectedData)
        {
            OnConnected?.Invoke(connectedData);
        }

        private static void InvokeSessionApproved(SessionStruct session)
        {
            OnSessionApproved?.Invoke(session);
        }

        public async ValueTask WillStartAsync()
        {
            configuration.SavedUserAddress?.AssertIsPublicAddress(nameof(configuration.SavedUserAddress));

            // if testing just don't initialize wallet connect
            if (Testing)
            {
                Address = configuration.SavedUserAddress;

                return;
            }

            // Wallet Connect
            await Initialize(configuration.WalletConnectConfig);

            Address = configuration.SavedUserAddress ?? await GetAccountVerifyUserOwns();
        }

        public async Task Initialize(WalletConnectConfig config)
        {
            Config = config;

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
                Name = Config.ProjectName,
                ProjectId = Config.ProjectId,
                Storage = new InMemoryStorage(),
                BaseContext = Config.BaseContext,
            });

            await Core.Start();

            SignClient = await WalletConnectSignClient.Init(new SignClientOptions()
            {
                BaseContext = Config.BaseContext,
                Core = Core,
                Metadata = Config.Metadata,
                Name = Config.ProjectName,
                ProjectId = Config.ProjectId,
                Storage = Core.Storage,
            });
        }

        public async Task<ConnectedData> ConnectClient()
        {
            RequiredNamespaces requiredNamespaces = new RequiredNamespaces();

            var methods = new string[]
            {
                "eth_sendTransaction", "eth_signTransaction", "eth_sign", "personal_sign", "eth_signTypedData",
            };

            var events = new string[] { "chainChanged", "accountsChanged" };

            requiredNamespaces.Add(
                Chain.EvmNamespace,
                new ProposedNamespace
                {
                    Chains = new string[]
                    {
                        Config.Chain.FullChainId,
                    },
                    Events = events,
                    Methods = methods,
                });

            // start connecting
            ConnectedData connectData = await SignClient.Connect(new ConnectOptions
            {
                RequiredNamespaces = requiredNamespaces,
            });

            InvokeConnected(connectData);

            SessionStruct sessionResult = await connectData.Approval;

            InvokeSessionApproved(sessionResult);

            // get default wallet
            if (Application.isMobilePlatform && Config.DefaultWallet == null)
            {
                string nativeUrl = sessionResult.Peer.Metadata.Redirect.Native.Replace("//", string.Empty);

                int index = nativeUrl.IndexOf(':');

                if (index != -1)
                {
                    nativeUrl = $"{nativeUrl.Substring(0, index)}:";
                }

                Debug.Log($"Wallet Native Url {nativeUrl}");

                var defaultWallet = Config.SupportedWallets.Values.FirstOrDefault(w =>
                    w.Mobile.NativeProtocol == nativeUrl || w.Desktop.NativeProtocol == nativeUrl);

                if (defaultWallet != null)
                {
                    Config.DefaultWallet = defaultWallet;

                    WCLogger.Log("Default Wallet Set");
                }
                else
                {
                    WCLogger.Log("Default Wallet Not Found in Supported Wallets");
                }
            }

            return connectData;
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Disconnect());
        }

        public Task<string> GetAddress()
        {
            Address.AssertNotNull(nameof(Address));
            return Task.FromResult(Address!);
        }

        private Task<TR> Request<T, TR>(string topic, T data, string chainId = null, long? expiry = null)
        {
            string method = RpcMethodAttribute.MethodForType<T>();

            // if it's a registered method try and open wallet
            if (Session.Namespaces.Any(n => n.Value.Methods.Contains(method)))
            {
                Core.Relayer.Events.ListenForOnce<object>(
                    RelayerEvents.Publish,
                    (_, _) =>
                    {
                        if (Config.DefaultWallet != null)
                        {
                            WCLogger.Log("Opening Default Wallet...");

                            Config.DefaultWallet.OpenWallet();
                        }
                        else
                        {
                            WCLogger.Log("No Default Wallet to Open");
                        }
                    });
            }

            return SignClient.Request<T, TR>(topic, data, chainId, expiry);
        }

        public async Task<string> SignMessage(string message)
        {
            if (Testing)
            {
                return TestResponse;
            }

            // var pageUrl = BuildUrl();

            // Wallet connect
            SessionStruct session = Session;

            var (address, chainId) = GetCurrentAddress();

            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            var request = new EthSignMessage(message, address);

            string hash =
                await Request<EthSignMessage, string>(session.Topic, request, chainId);

            var isValid = ValidateResponse(hash);
            if (!isValid)
            {
                throw new Web3Exception("Incorrect response format extracted from clipboard.");
            }

            // TODO: log event on success
            return hash;

            // string BuildUrl()
            // {
            //     return $"{configuration.ServiceUrl}" +
            //            "?action=sign" +
            //            $"&message={Uri.EscapeDataString(message)}";
            // }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            // Wallet connect
            SessionStruct session = Session;

            var (address, chainId) = GetCurrentAddress();

            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            var request = new EthSignTypedData<TStructType>(address, domain, message);

            string hash =
                await Request<EthSignTypedData<TStructType>, string>(session.Topic, request, chainId);

            var isValid = ValidateResponse(hash);
            if (!isValid)
            {
                throw new Web3Exception("Incorrect response format extracted from clipboard.");
            }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }

            return hash;
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // TODO: log event on success (see example near end of file)
            return await provider.GetTransaction(hash);

            string BuildUrl()
            {
                var sb = new StringBuilder()
                    .Append(configuration.ServiceUrl)
                    .Append("?action=send");

                if (transaction.ChainId != null)
                {
                    sb.Append("&chainId=").Append(transaction.ChainId);
                }
                else
                {
                    sb.Append("&chainId=").Append(chainConfig.ChainId);
                }

                if (transaction.Value != null)
                {
                    sb.Append("&value=").Append(transaction.Value);
                }
                else
                {
                    sb.Append("&value=").Append(0);
                }

                AppendStringIfNotNullOrEmtpy("to", transaction.To);
                AppendStringIfNotNullOrEmtpy("data", transaction.Data);
                AppendIfNotNull("gasLimit", transaction.GasLimit);
                AppendIfNotNull("gasPrice", transaction.GasPrice);

                return sb.ToString();

                void AppendIfNotNull(string name, object value)
                {
                    if (value != null)
                    {
                        sb!.Append('&').Append(name).Append('=').Append(value);
                    }
                }

                void AppendStringIfNotNullOrEmtpy(string name, string value)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        sb!.Append('&').Append(name).Append('=').Append(value);
                    }
                }
            }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 66;
            }
        }

        private (string, string) GetCurrentAddress()
        {
            var currentSession = Session;

            var defaultChain = currentSession.Namespaces.Keys.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(defaultChain))
            {
                return (null, null);
            }

            var defaultNamespace = currentSession.Namespaces[defaultChain];

            if (defaultNamespace.Accounts.Length == 0)
            {
                return (null, null);
            }

            var fullAddress = defaultNamespace.Accounts[0];
            var addressParts = fullAddress.Split(":");

            var address = addressParts[2];
            var chainId = string.Join(':', addressParts.Take(2));

            return (address, chainId);
        }

        // TODO: extract hash from deeplink instead of clipboard
        private async Task<string> OpenPageWaitResponse(string pageUrl, Func<string, bool> validator)
        {
            string response;

            if (Testing)
            {
                response = TestResponse;
            }
            else
            {
                operatingSystem.OpenUrl(pageUrl);
                operatingSystem.ClipboardContent = string.Empty;

                var updateDelay = GetUpdatePeriodSafe();
                while (string.IsNullOrEmpty(operatingSystem.ClipboardContent))
                {
                    await Task.Delay(updateDelay);
                }

                response = operatingSystem.ClipboardContent!;
            }

            var validResponse = validator(response);
            if (!validResponse)
            {
                throw new Web3Exception("Incorrect response format extracted from clipboard.");
            }

            int GetUpdatePeriodSafe()
            {
                return (int)Math.Max(MinClipboardCheckPeriod.TotalMilliseconds, configuration.ClipboardCheckPeriod.TotalMilliseconds);
            }

            return response;
        }

        private async Task<string> GetAccountVerifyUserOwns()
        {
            // sign current time
            var expirationTime = DateTime.Now + configuration.ConnectRequestExpiresAfter;
            await ConnectClient();

            var (address, _) = GetCurrentAddress();

            if (!AddressExtensions.IsPublicAddress(address))
            {
                throw new Web3Exception(
                    $"Public address recovered from signature is not valid. Public address: {address}");
            }

            if (DateTime.Now > expirationTime)
            {
                throw new Web3Exception("Signature has already expired. Try again.");
            }

            return address;
        }

        private async Task Disconnect()
        {
            try
            {
                configuration.SavedUserAddress = null;

                await SignClient.Disconnect(Session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
            }
            catch (Exception e)
            {
                WCLogger.LogError($"error disconnecting: {e}");
            }
        }
    }
}