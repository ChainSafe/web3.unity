using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// WalletConnect connection provider used for connecting to a wallet using WalletConnect.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider/Wallet Connect", fileName = nameof(WalletConnectConnectionProvider))]
    public class WalletConnectConnectionProvider : ConnectionProvider
    {
        [SerializeField] private WalletConnectConfigSO walletConnectConfig;
        
        public override bool IsAvailable => Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer;
     
        public override Task Initialize()
        {
            return Task.CompletedTask;
        }
        
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
