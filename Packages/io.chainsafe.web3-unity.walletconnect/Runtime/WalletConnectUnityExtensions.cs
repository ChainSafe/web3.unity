using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WalletConnectUnity.Modal;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public static class WalletConnectUnityExtensions
    {
        public static IWeb3ServiceCollection ConfigureWalletConnectUnity(this IWeb3ServiceCollection services, IWalletConnectUnityConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletConnectConfig), config));
            return services;
        }
        
        public static IWeb3ServiceCollection UseWalletConnectUnity(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();
            services.AddSingleton<IWalletConnectProvider, ILifecycleParticipant, WalletConnectUnityProvider>();
            return services;
        }
        
        public static IWeb3ServiceCollection UseWalletConnectUnity(this IWeb3ServiceCollection services, IWalletConnectUnityConfig config)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();
            services.ConfigureWalletConnectUnity(config);
            services.UseWalletConnectUnity();
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
                tcs.SetResult(0);
                WalletConnectModal.Ready -= OnReady;
            }
        }
    }
}