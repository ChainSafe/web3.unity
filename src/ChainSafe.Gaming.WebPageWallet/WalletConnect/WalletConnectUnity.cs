using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
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

        public WalletConnectConfig Config { get; private set; }

        public async Task Initialize(WalletConnectConfig config)
        {
            Config = config;

            if (Config.Logger != null)
            {
                WCLogger.Logger = Config.Logger;
            }

            Core = new WalletConnectCore(new CoreOptions()
            {
                Name = Config.ProjectName, ProjectId = Config.ProjectId, Storage = BuildStorage(Config.StoragePath), BaseContext = Config.BaseContext,
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

        private IKeyValueStorage BuildStorage(string path)
        {
            path = Path.Combine(path);
            return new FileSystemStorage(path);
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
                    Chains = new string[] { Chain.Goerli.FullChainId }, Events = events, Methods = methods,
                });

            // start connecting
            ConnectedData connectData = await SignClient.Connect(new ConnectOptions
            {
                RequiredNamespaces = requiredNamespaces,
            });

            InvokeConnected(connectData);

            SessionStruct sessionResult = await connectData.Approval;

            InvokeSessionApproved(sessionResult);

            string nativeUrl = sessionResult.Peer.Metadata.Redirect.Native.Replace("//", string.Empty);

            string defaultWalletId = Config.SupportedWallets.FirstOrDefault(t =>
                    t.Value.Mobile.NativeProtocol == nativeUrl || t.Value.Desktop.NativeProtocol == nativeUrl)
                .Key;

            var defaultWallet = Config.SupportedWallets[defaultWalletId];

            if (Config.IsMobilePlatform)
            {
                defaultWallet.OpenDeeplink(connectData);
            }

            return connectData;
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