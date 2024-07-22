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
    public class WalletConnectConnectionProvider : RestorableConnectionProvider
    {
        [SerializeField] private WalletConnectConfigSO walletConnectConfig;
        
        private bool _storedSessionAvailable;
        
        public override bool IsAvailable => Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer;
     
        public override Task Initialize()
        {
            return Task.CompletedTask;
        }
        
        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
            services.UseWalletConnect(walletConnectConfig.WithRememberSession(RememberSession || _storedSessionAvailable))
                .UseWalletSigner().UseWalletTransactionExecutor();
        }

        public override async Task<bool> SavedSessionAvailable()
        {
            await using (var lightWeb3 = await WalletConnectWeb3.BuildLightweightWeb3(walletConnectConfig))
            {
                _storedSessionAvailable = lightWeb3.WalletConnect().ConnectionHelper().StoredSessionAvailable;
            }

            return _storedSessionAvailable;
        }
    }
}
