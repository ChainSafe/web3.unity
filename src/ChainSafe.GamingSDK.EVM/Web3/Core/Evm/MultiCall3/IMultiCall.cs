using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3
{
    public interface IMultiCall : ILifecycleParticipant
    {
        void MultiCall(IRpcProvider provider);
    }
}