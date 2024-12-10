using System;
using System.Threading.Tasks;
using Reown.AppKit.Unity.Utils;
using Reown.Sign.Models;
using Reown.Sign.Unity;
using UnityEngine;

namespace Reown.AppKit.Unity
{
    public class AppKitCore : AppKit
    {
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("[AppKit] Instance already exists. Destroying...");
                Destroy(gameObject);
            }
        }

        protected override async Task InitializeAsyncCore()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            await CreateSignClient();
#endif

            ModalController = CreateModalController();
            AccountController = new AccountController();
            ConnectorController = new ConnectorController();
            ApiController = new ApiController();
            BlockchainApiController = new BlockchainApiController();
            NotificationController = new NotificationController();
            NetworkController = new NetworkControllerCore();
            EventsController = new EventsController();
            SiweController = new SiweController();

#if UNITY_WEBGL && !UNITY_EDITOR
            Evm = new WagmiEvmService();
#else
            Evm = new NethereumEvmService();
#endif

            await Task.WhenAll(
                BlockchainApiController.InitializeAsync(SignClient),
                ConnectorController.InitializeAsync(Config, SignClient),
                ModalController.InitializeAsync(),
                EventsController.InitializeAsync(Config, ApiController),
                NetworkController.InitializeAsync(ConnectorController, Config.supportedChains),
                AccountController.InitializeAsync(ConnectorController, NetworkController, BlockchainApiController)
            );

            await Evm.InitializeAsync(SignClient);

            ConnectorController.AccountConnected += AccountConnectedHandler;
            ConnectorController.AccountDisconnected += AccountDisconnectedHandler;
            
            EventsController.SendEvent(new Event
            {
                name = "MODAL_LOADED"
            });
        }

        protected override void OpenModalCore(ViewType viewType = ViewType.None)
        {
            if (viewType == ViewType.None)
            {
                ModalController.Open(IsAccountConnected ? ViewType.Account : ViewType.Connect);
            }
            else
            {
                if (IsAccountConnected && viewType == ViewType.Connect)
                    // TODO: use custom exception type
                    throw new Exception("Trying to open Connect view when account is already connected.");
                ModalController.Open(viewType);
            }
        }

        protected override void CloseModalCore()
        {
            ModalController.Close();
        }

        protected override Task DisconnectAsyncCore()
        {
            return ConnectorController.DisconnectAsync();
        }

        protected virtual ModalController CreateModalController()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return new Reown.AppKit.Unity.WebGl.ModalControllerWebGl();
#else
            return new ModalControllerUtk();
#endif
        }

        private async Task CreateSignClient()
        {
            SignClient = await SignClientUnity.Create(new SignClientOptions
            {
                Name = Config.metadata.Name,
                ProjectId = Config.projectId,
                Metadata = Config.metadata
            });
        }

        private static void AccountConnectedHandler(object sender, Connector.AccountConnectedEventArgs e)
        {
            if (WalletUtils.TryGetLastViewedWallet(out var lastViewedWallet))
                WalletUtils.SetRecentWallet(lastViewedWallet);

            if (!SiweController.IsEnabled)
                CloseModal();
        }

        private static void AccountDisconnectedHandler(object sender, Connector.AccountDisconnectedEventArgs e)
        {
            CloseModal();
        }
    }
}