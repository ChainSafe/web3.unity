using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Helper class to build preconfigured Web3 clients for HyperPlay.
    /// </summary>
    public static class HyperPlayWeb3
    {
        /// <summary>
        /// Builds a lightweight Web3 client with basic HyperPlay functionality.
        /// </summary>
        /// <param name="config">Your HyperPlay config.</param>
        /// <returns>A lightweight version of Web3 client with basic HyperPlay functionality.</returns>
        public static ValueTask<Web3.Web3> BuildLightweightWeb3(IHyperPlayConfig config)
        {
            var projectConfig = ProjectConfigUtilities.Load();

            return new Web3Builder(projectConfig).Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseHyperPlay(config);
            }).LaunchAsync();
        }
    }
}