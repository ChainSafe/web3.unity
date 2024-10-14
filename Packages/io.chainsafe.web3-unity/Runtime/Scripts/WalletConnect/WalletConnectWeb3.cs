using System.Threading.Tasks;
using ChainSafe.Gaming.EVM.Events;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Helper class to build preconfigured Web3 clients for WalletConnect.
    /// </summary>
    public static class WalletConnectWeb3
    {
        /// <summary>
        /// Builds a lightweight Web3 client with basic WalletConnect functionality.
        /// </summary>
        /// <param name="wcConfig">Your WalletConnect config.</param>
        /// <param name="projectConfig">ChainSafe SDK project configuration (optional).</param>
        /// <returns>A lightweight version of Web3 client with basic WalletConnect functionality.</returns>
        public static ValueTask<Web3.Web3> BuildLightweightWeb3(IWalletConnectConfig wcConfig, ICompleteProjectConfig projectConfig = null)
        {
            projectConfig ??= ProjectConfigUtilities.Load();

            return new Web3Builder(projectConfig).Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseWalletConnect(wcConfig);
            }).LaunchAsync();
        }
    }
}