using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnityEngine;
using WalletConnectUnity.Modal;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public static class WalletConnectUnityExtensions
    {
        public static IWeb3ServiceCollection UseWalletConnectUnity(this IWeb3ServiceCollection services)
        {
            var defaultConfig = LoadDefaultConfig();
            services.UseWalletConnectUnity(defaultConfig);
            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectUnity(this IWeb3ServiceCollection services, IWalletConnectUnityConfig config)
        {
            services.AssertServiceNotBound<IWalletProvider>();
            
            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletProviderConfig), config));
            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletConnectUnityConfig), config));

            services.AddSingleton<IWalletProvider, ILifecycleParticipant, WalletConnectUnityProvider>();
            return services;
        }

        public static Task AwaitReady(this WalletConnectModal modal)
        {
            if (!modal) throw new Web3Exception("Modal does not exist.");
            if (WalletConnectModal.IsReady) return Task.CompletedTask;

            var tcs = new TaskCompletionSource<byte>();
            
            WalletConnectModal.Ready += OnReady;

            return tcs.Task;

            void OnReady(object sender, ModalReadyEventArgs e)
            {
                WalletConnectModal.Ready -= OnReady;
                tcs.SetResult(0);
            }
        }

        public static WalletConnectUnityConfigAsset LoadDefaultConfig()
        {
            return Resources.Load<WalletConnectUnityConfigAsset>("DefaultWalletConnectConfig");
        }
    }
}