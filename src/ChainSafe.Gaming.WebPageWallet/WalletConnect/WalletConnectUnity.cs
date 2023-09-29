using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Common.Model.Errors;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;
using WalletConnectSharp.Storage.Interfaces;

namespace ChainSafe.Gaming.Wallets.WalletConnect
{
    public class WalletConnectUnity
    {
        public delegate void Connected(ConnectedData connectedData);

        public delegate void SessionApproved(SessionStruct session);

        public event Connected OnConnected;

        public event SessionApproved OnSessionApproved;

        public WalletConnectCore Core { get; private set; }

        public WalletConnectSignClient SignClient { get; private set; }

        public SessionStruct Session => SignClient.Session.Get(SignClient.Session.Keys[0]);

        public WalletConnectConfig Config { get; private set; }

        public async Task Initialize(WalletConnectConfig config)
        {
            if (Core != null && Core.Initialized)
            {
                WCLogger.Log("Core already initialized");

                return;
            }

            Config = config;

            if (Config.Logger != null)
            {
                WCLogger.Logger = Config.Logger;
            }

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

            if (Config.IsMobilePlatform)
            {
                // this doesn't work for all wallets, hence the try catch
                try
                {
                    string nativeUrl = sessionResult.Peer.Metadata.Redirect.Native.Replace("//", string.Empty);

                    string defaultWalletId = Config.SupportedWallets.FirstOrDefault(t =>
                            t.Value.Mobile.NativeProtocol == nativeUrl || t.Value.Desktop.NativeProtocol == nativeUrl)
                        .Key;

                    if (Config.SupportedWallets.TryGetValue(defaultWalletId, out Wallet wallet))
                    {
                        wallet.OpenDeeplink(connectData);
                    }
                }
                catch (Exception e)
                {
                    WCLogger.Log($"Can't open deepLink for wallet {e}");
                }
            }

            return connectData;
        }

        public async Task Disconnect()
        {
            await SignClient.Disconnect(Session.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
        }

        private void InvokeConnected(ConnectedData connectedData)
        {
            OnConnected?.Invoke(connectedData);
        }

        private void InvokeSessionApproved(SessionStruct session)
        {
            OnSessionApproved?.Invoke(session);
        }
    }
}