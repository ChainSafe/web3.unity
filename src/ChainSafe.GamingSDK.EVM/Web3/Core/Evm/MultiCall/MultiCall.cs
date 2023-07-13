using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall
{
    public class MultiCall : ILifecycleParticipant
    {
        private readonly IRpcProvider provider;

        public MultiCall(IRpcProvider provider)
        {
            this.provider = provider;
        }

        public ValueTask WillStartAsync()
        {
        }

        public ValueTask WillStopAsync()
        {
        }
    }
}