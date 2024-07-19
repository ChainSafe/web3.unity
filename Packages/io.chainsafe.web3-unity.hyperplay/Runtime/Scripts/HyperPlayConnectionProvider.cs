using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Connection provider for connecting via HyperPlay Launcher.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider/HyperPlay", fileName = nameof(HyperPlayConnectionProvider))]
    public class HyperPlayConnectionProvider : RestorableConnectionProvider
    {
        public override bool IsAvailable => Application.isEditor || !Application.isMobilePlatform;

        private bool _storedSessionAvailable;
        
        public override Task Initialize()
        {
            return Task.CompletedTask;
        }
        
        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
            var config = new HyperPlayConfig
            {
                // RememberSession = rememberMeToggle.isOn || _storedSessionAvailable,
                RememberSession = true,
            };
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseHyperPlay<HyperPlayWebGLProvider>(config);
#else
            services.UseHyperPlay(config);
#endif
            services.UseWalletSigner().UseWalletTransactionExecutor();
        }

        public override async Task<bool> SavedSessionAvailable()
        {
            var data = new HyperPlayData();

            await data.LoadOneTime();
            
            _storedSessionAvailable = data.RememberSession;

            return _storedSessionAvailable;
        }
    }
}
