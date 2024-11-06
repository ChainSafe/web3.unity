using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage
{
    /// <summary>
    /// Controls used to easily connect/disconnect a wallet, display and copy address.
    /// </summary>
    public class ConnectToWallet : ServiceAdapter, IWeb3InitializedHandler, ILogoutHandler
    {
        [SerializeField] private bool rememberMe = true;

        [Space] [SerializeField] private Button connectButton;

        [SerializeField] private Button disconnectButton;

        [Space] [SerializeField] private TextMeshProUGUI addressText;

        [SerializeField] private Button copyAddressButton;

        [Space] [SerializeField] private Transform connectedTransform;

        [SerializeField] private Transform disconnectedTransform;

        private async void Start()
        {
            try
            {
                await Web3Unity.Instance.Initialize(rememberMe);
            }
            finally
            {
                AddButtonListeners();

                ConnectionStateChanged(Web3Unity.Connected, Web3Unity.Instance.PublicAddress);
            }
        }

        private void AddButtonListeners()
        {
            connectButton.onClick.AddListener(Web3Unity.ConnectModal.Open);

            disconnectButton.onClick.AddListener(Disconnect);

            copyAddressButton.onClick.AddListener(CopyAddress);

            void CopyAddress()
            {
                ClipboardManager.CopyText(addressText.text);
            }
        }

        private void ConnectionStateChanged(bool connected, string address = "")
        {
            connectedTransform.gameObject.SetActive(connected);

            disconnectedTransform.gameObject.SetActive(!connected);

            if (connected)
            {
                addressText.text = address;
            }
        }

        public Task OnWeb3Initialized(CWeb3 web3)
        {
            ConnectionStateChanged(true, web3.Signer.PublicAddress);

            return Task.CompletedTask;
        }

        private async void Disconnect()
        {
            await Web3Unity.Instance.Disconnect();
        }

        public Task OnLogout()
        {
            ConnectionStateChanged(false);

            return Task.CompletedTask;
        }

        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.AddSingleton<ILogoutHandler, IWeb3InitializedHandler, ConnectToWallet>(_ => this);
            });
        }
    }
}