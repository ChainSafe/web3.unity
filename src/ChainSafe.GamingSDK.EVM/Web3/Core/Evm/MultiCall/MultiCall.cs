using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall
{
    public class MultiCall : ILifecycleParticipant
    {
        private Contract multiCallContract;

        public MultiCall(IRpcProvider provider, IChainConfig chainConfig, MultiCallConfig config)
        {
            if (MultiCallConfig.DeployedNetworks.Contains(chainConfig.ChainId))
            {
                multiCallContract = new Contract(MultiCallConfig.MultiCallAbi, MultiCallConfig.OfficialAddress, provider);
            }
            else
            {
                if (config.CustomNetworks.TryGetValue(chainConfig.ChainId, out var address))
                {
                    multiCallContract = new Contract(MultiCallConfig.MultiCallAbi, address, provider);
                }
            }
        }

        public ValueTask WillStartAsync()
        {
            return default;
        }

        public ValueTask WillStopAsync()
        {
            return default;
        }
    }
}