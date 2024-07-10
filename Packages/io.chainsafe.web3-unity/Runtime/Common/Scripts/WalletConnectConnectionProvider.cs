using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class WalletConnectConnectionProvider : ConnectionProvider
    {
        [SerializeField] private WalletConnectConfigSO walletConnectConfig;
        
        public override bool IsAvailable => Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer;
        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                // var rememberSession = rememberSessionToggle.isOn || storedSessionAvailable;
                services.UseWalletConnect(walletConnectConfig.WithRememberSession(false))
                    .UseWalletSigner().UseWalletTransactionExecutor();
            });
        }
    }
}
