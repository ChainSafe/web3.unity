using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingWeb3;

namespace Chainsafe.Gaming.Chainlink
{
    public class ChainlinkClient : IWeb3Container
    {
        private readonly Web3 web3;

        Web3 IWeb3Container.Web3 => web3;

        public ChainlinkClient(Web3 web3)
        {
            this.web3 = web3;
        }
    }
}