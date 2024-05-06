using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using WalletConnectUnity.Modal;
using OriginalWc = WalletConnectUnity.Core.WalletConnect;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public class WalletConnectUnityProvider : IWalletConnectProvider, ILifecycleParticipant
    {
        private static bool instanceStarted;
        
        public ValueTask WillStartAsync()
        {
            if (instanceStarted)
            {
                throw new Web3Exception(
                    $"One instance of {nameof(WalletConnectUnityProvider)} is already running. This integration of WalletConnect does not support running multiple instances.");
            }

            if (!WalletConnectModal.IsReady)
            {
                throw new Web3Exception(
                    $"{nameof(WalletConnectModal)} is not ready yet. Make sure it's ready before launching {nameof(Web3.Web3)}");
            }

            instanceStarted = true;
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask WillStopAsync()
        {
            instanceStarted = false;

            return new ValueTask(Task.CompletedTask);
        }
        
        public Task<string> Connect()
        {
            throw new System.NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Request<T>(T data, long? expiry = null)
        {
            throw new System.NotImplementedException();
        }
    }

    public class WalletConnectUnityConfig
    {
        
    }

    public static class WalletConnectUnityExtensions
    {
        public static IWeb3ServiceCollection UseWalletConnectUnity(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();
            services.AddSingleton<IWalletConnectProvider, ILifecycleParticipant, WalletConnectUnityProvider>();
            return services;
        }
    }
}