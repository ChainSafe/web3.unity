using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.MultiCall
{
    public interface IMultiCall : ILifecycleParticipant
    {
        void MultiCall(IRpcProvider provider);
    }
}