using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Unity.EthereumWindow;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Unity.MetaMask
{
    public class MetaMaskController : EthereumWindowController
    {
        [DllImport("__Internal")]
        public static extern bool IsMetaMask();

        public override Task<string> Connect(IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider)
        {
            if (!IsMetaMask())
            {
                throw new Web3Exception("MetaMask is not installed.");
            }

            return base.Connect(chainConfig, chainRegistryProvider);
        }
    }
}