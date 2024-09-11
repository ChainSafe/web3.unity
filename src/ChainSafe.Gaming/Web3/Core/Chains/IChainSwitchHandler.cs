using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public interface IChainSwitchHandler
    {
        public Task HandleChainSwitching();
    }
}