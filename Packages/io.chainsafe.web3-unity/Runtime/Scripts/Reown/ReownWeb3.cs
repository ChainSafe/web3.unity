using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// Helper class to build preconfigured Web3 clients for Reown.
    /// </summary>
    public static class ReownWeb3
    {
        /// <summary>
        /// Builds a lightweight Web3 client with basic Reown functionality.
        /// </summary>
        /// <param name="reownConfig">Your Reown config.</param>
        /// <param name="projectConfig">ChainSafe SDK project configuration (optional).</param>
        /// <returns>A lightweight version of Web3 client with basic Reown functionality.</returns>
        public static ValueTask<Web3.Web3> BuildLightweightWeb3(IReownConfig reownConfig, ICompleteProjectConfig projectConfig = null)
        {
            projectConfig ??= ProjectConfigUtilities.Load();

            return new Web3Builder(projectConfig).Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseReown(reownConfig);
            }).LaunchAsync();
        }
    }
}