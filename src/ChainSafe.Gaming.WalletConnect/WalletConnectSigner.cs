using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
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
    public class WalletConnectSigner : ISigner, ILifecycleParticipant
    {
        private readonly IOperatingSystemMediator operatingSystem;
        private readonly ILogWriter logWriter;
        private readonly WalletConnectConfig config;

        public WalletConnectSigner(WalletConnectConfig config, IOperatingSystemMediator operatingSystem, ILogWriter logWriter)
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

        public string Address { get; private set; }

        public async ValueTask WillStartAsync()
        {
            config.SavedUserAddress?.AssertIsPublicAddress(nameof(config.SavedUserAddress));

            // if testing just don't initialize wallet connect
            if (config.Testing)
            {
                Address = config.SavedUserAddress;

                return;
            }

            // Wallet Connect
            await Initialize();

            Address = config.SavedUserAddress ?? await ConnectToWallet();
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
                Storage = new InMemoryStorage(),
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

        private async Task<ConnectedData> ConnectClient()
        {
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

            // start connecting
            ConnectedData connectData = await SignClient.Connect(new ConnectOptions
            {
                RequiredNamespaces = requiredNamespaces,
            });

            config.InvokeConnected(connectData);

            // open deeplink to redirect to wallet for connection
            if (config.RedirectToWallet)
            {
                if (config.DefaultWallet != null)
                {
                    config.DefaultWallet.OpenDeeplink(connectData, operatingSystem);
                }
                else
                {
                    operatingSystem.OpenUrl(connectData.Uri);
                }
            }

            Session = await connectData.Approval;

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

            return connectData;
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Disconnect());
        }

        public Task<string> GetAddress()
        {
            if (string.IsNullOrEmpty(Address))
            {
                var addressParts = GetFullAddress().Split(":");

                Address = addressParts[2];
            }

            if (!AddressExtensions.IsPublicAddress(Address))
            {
                throw new Web3Exception(
                    $"Public address recovered from signature is not valid. Public address: {Address}");
            }

            Address.AssertNotNull(nameof(Address));

            return Task.FromResult(Address!);
        }

        public Task<TR> Request<T, TR>(T data, long? expiry = null)
        {
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

            return SignClient.Request<T, TR>(topic, data, chainId, expiry);
        }

        public async Task<string> SignTransaction(TransactionRequest transaction)
        {
            if (config.Testing)
            {
                return config.TestResponse;
            }

            if (string.IsNullOrEmpty(transaction.From))
            {
                transaction.From = await GetAddress();
            }

            EthSignTransaction requestData = new EthSignTransaction(new TransactionModel
            {
                From = transaction.From,
                To = transaction.To,
                Gas = transaction.GasLimit?.HexValue,
                GasPrice = transaction.GasPrice?.HexValue,
                Value = transaction.Value?.HexValue,
                Data = transaction.Data ?? "0x",
                Nonce = transaction.Nonce?.HexValue,
            });

            string hash = await Request<EthSignTransaction, string>(requestData);

            // TODO replace validation with regex
            WCLogger.Log($"Transaction executed with hash {hash}");

            return hash;
        }

        public async Task<string> SignMessage(string message)
        {
            if (config.Testing)
            {
                return config.TestResponse;
            }

            var requestData = new EthSignMessage(message, Address);

            string hash =
                await Request<EthSignMessage, string>(requestData);

            var isValid = ValidateResponse(hash);
            if (!isValid)
            {
                throw new Web3Exception("Incorrect response format from signing.");
            }

            // TODO: log event on success
            return hash;

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var requestData = new EthSignTypedData<TStructType>(Address, domain, message);

            string hash =
                await Request<EthSignTypedData<TStructType>, string>(requestData);

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

        // Connect to wallet and return address of connected wallet.
        private async Task<string> ConnectToWallet()
        {
            ConnectedData = await ConnectClient();

            return await GetAddress();
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

        private async Task Disconnect()
        {
            try
            {
                config.SavedUserAddress = null;

                await SignClient.Disconnect(Session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
            }
            catch (Exception e)
            {
                WCLogger.LogError($"error disconnecting: {e}");
            }
        }
    }
}