using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class UnityWalletConnectExtensions
    {
        /// <summary>
        /// Builds a lightweight version of web3 with <see cref="IConnectionHelper"/>.
        /// </summary>
        /// <param name="builder">An instance of <see cref="Web3Builder"/> used to built a lightweight Web3 version.</param>
        /// <param name="config">The <see cref="IWalletConnectConfig"/> object.</param>
        /// <returns><see cref="IConnectionHelper"/> service.</returns>
        public static async ValueTask<IConnectionHelper> BuildConnectionHelper(this Web3Builder builder, IWalletConnectConfig config)
        {
            var lightWeb3 = await builder.LaunchLightweightWeb3(config); // todo refactor samples as this should be terminated after use
            return lightWeb3.WalletConnect().ConnectionHelper();
        }

        private static ValueTask<Web3.Web3> LaunchLightweightWeb3(this Web3Builder builder, IWalletConnectConfig config)
        {
            return builder.Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseWalletConnect(config);
            }).LaunchAsync();
        }
    }
}