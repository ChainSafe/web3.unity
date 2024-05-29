using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.HyperPlay;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;

namespace ChainSafe.Gaming.HyperPlay
{
    public static class HyperPlayWeb3
    {
        public static ValueTask<Web3.Web3> BuildLightweightWeb3()
        {
            var projectConfig = ProjectConfigUtilities.Load();

            return new Web3Builder(projectConfig).Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseHyperPlay(new HyperPlayConfig());
            }).LaunchAsync();
        }
    }
}
