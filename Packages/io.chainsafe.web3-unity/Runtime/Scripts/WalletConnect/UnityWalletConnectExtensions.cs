using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class UnityWalletConnectExtensions
    {
        public static async ValueTask<ILoginHelper> BuildLoginHelper(this Web3Builder builder, IWalletConnectConfig config)
        {
            var web3 = await builder.LaunchLightweightWeb3(config);
            return web3.WalletConnect().LoginHelper();
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