using System.Collections.Generic;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectUnity.Modal;

namespace ChainSafe.Gaming.WalletConnectUnity.Samples
{
    public class WalletConnectSample : MonoBehaviour
    {
        public WalletConnectModal modal;
        public WalletConnectUnityConfigAsset walletConnectConfig;
        public List<Button> buttons;
        
        private Web3.Web3 web3;

        private async void Awake()
        {
            SetButtonsInteractable(false);
            
            await modal.AwaitReady();

            web3 = await new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseRpcProvider();
                    services.UseWalletConnectUnity(walletConnectConfig);
                    services.UseWalletConnectSigner();
                    services.UseWalletConnectTransactionExecutor();
                }).LaunchAsync();
            
            SetButtonsInteractable(true);
        }

        public async void SignMessage(string message)
        {
            SetButtonsInteractable(false);
            
            var response = await web3.Signer.SignMessage(message);
            Debug.Log($"Signed message. Response: \"{response}\"");
            
            SetButtonsInteractable(true);
        }

        public async void SendTransaction(int wei)
        {
            SetButtonsInteractable(false);
            
            var playerAddress = web3.Signer.PublicAddress;
            var response = await web3.TransactionExecutor.SendTransaction(new TransactionRequest
            {
                To = playerAddress, // send to yourself
                Value = new HexBigInteger(wei)
            });
            
            Debug.Log($"Sent {wei} wei. Response: \"{response}\".");
            
            SetButtonsInteractable(true);
        }

        public async void Disconnect()
        {
            SetButtonsInteractable(false);
            await web3.TerminateAsync(true);
            Debug.Log("Logged out. Restart the scene to connect again.");
        }

        private void SetButtonsInteractable(bool interactable)
        {
            foreach (var button in buttons)
            {
                button.interactable = interactable;
            }
        }

        [ContextMenu("Dis")]
        public void ForceDisconnect()
        {
            WalletConnectModal.Disconnect();
        }
    }
}