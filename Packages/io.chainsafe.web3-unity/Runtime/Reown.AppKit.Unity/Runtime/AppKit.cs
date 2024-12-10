using System;
using System.Threading.Tasks;
using Reown.Sign.Unity;
using UnityEngine;
using UnityEngine.Scripting;

namespace Reown.AppKit.Unity
{
    public abstract class AppKit : MonoBehaviour
    {
        public const string Version = "unity-appkit-v1.1.2"; // TODO: update this from CI
        public static AppKit Instance { get; protected set; }

        public static ModalController ModalController { get; protected set; }
        public static AccountController AccountController { get; protected set; }
        public static ConnectorController ConnectorController { get; protected set; }
        public static ApiController ApiController { get; protected set; }
        public static BlockchainApiController BlockchainApiController { get; protected set; }
        public static NotificationController NotificationController { get; protected set; }
        public static NetworkController NetworkController { get; protected set; }
        public static EventsController EventsController { get; protected set; }
        public static SiweController SiweController { get; protected set; }

        public static EvmService Evm { get; protected set; }

        public static AppKitConfig Config { get; private set; }
        
        public SignClientUnity SignClient { get; protected set; }
        
        public static bool IsInitialized { get; private set; }

        public static bool IsAccountConnected
        {
            get => ConnectorController.IsAccountConnected;
        }

        public static bool IsModalOpen
        {
            get => ModalController.IsOpen;
        }

        public static event EventHandler<InitializeEventArgs> Initialized;

        public static event EventHandler<Connector.AccountConnectedEventArgs> AccountConnected
        {
            add => ConnectorController.AccountConnected += value;
            remove => ConnectorController.AccountConnected -= value;
        }

        public static event EventHandler<Connector.AccountDisconnectedEventArgs> AccountDisconnected
        {
            add => ConnectorController.AccountDisconnected += value;
            remove => ConnectorController.AccountDisconnected -= value;
        }

        public static event EventHandler<Connector.AccountChangedEventArgs> AccountChanged
        {
            add => ConnectorController.AccountChanged += value;
            remove => ConnectorController.AccountChanged -= value;
        }

        public static event EventHandler<NetworkController.ChainChangedEventArgs> ChainChanged
        {
            add => NetworkController.ChainChanged += value;
            remove => NetworkController.ChainChanged -= value;
        }

        public static async Task InitializeAsync(AppKitConfig config)
        {
            if (Instance == null)
                throw new Exception("Instance not set");
            if (IsInitialized)
                throw new Exception("Already initialized"); // TODO: use custom ex type

            Config = config ?? throw new ArgumentNullException(nameof(config));

            await Instance.InitializeAsyncCore();

            IsInitialized = true;
            Initialized?.Invoke(null, new InitializeEventArgs());
        }

        public static void OpenModal(ViewType viewType = ViewType.None)
        {
            if (!IsInitialized)
                throw new Exception("AppKit not initialized"); // TODO: use custom ex type

            Instance.OpenModalCore(viewType);
        }

        public static void CloseModal()
        {
            if (!IsModalOpen)
                return;

            Instance.CloseModalCore();
        }

        public static Task<Account> GetAccountAsync()
        {
            return ConnectorController.GetAccountAsync();
        }

        public static Task DisconnectAsync()
        {
            if (!IsInitialized)
                throw new Exception("AppKit not initialized"); // TODO: use custom ex type

            if (!IsAccountConnected)
                throw new Exception("No account connected"); // TODO: use custom ex type

            return Instance.DisconnectAsyncCore();
        }

        protected abstract Task InitializeAsyncCore();

        protected abstract void OpenModalCore(ViewType viewType = ViewType.None);

        protected abstract void CloseModalCore();

        protected abstract Task DisconnectAsyncCore();

        public class InitializeEventArgs : EventArgs
        {
            [Preserve]
            public InitializeEventArgs()
            {
            }
        }
    }
}