using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Connection provider for connecting via HyperPlay Launcher.
    /// </summary>
    public class HyperPlayConnectionProvider : ConnectionProvider
    {
        public override bool IsAvailable => Application.isEditor || !Application.isMobilePlatform;

        public override Task Initialize()
        {
            return Task.CompletedTask;
        }

        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                var config = new HyperPlayConfig
                {
                    // RememberSession = rememberMeToggle.isOn || _storedSessionAvailable,
                };
#if UNITY_WEBGL && !UNITY_EDITOR
                services.UseHyperPlay<HyperPlayWebGLProvider>(config);
#else
                services.UseHyperPlay(config);
#endif
                services.UseWalletSigner().UseWalletTransactionExecutor();
            });
        }
    }
}
