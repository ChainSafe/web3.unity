using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingWeb3;

namespace Chainsafe.Gaming.Chainlink
{
    public class ChainlinkSubCategory : IWeb3SubCategory
    {
        private readonly Web3 web3;

        Web3 IWeb3SubCategory.Web3 => web3;

        public ChainlinkSubCategory(Web3 web3)
        {
            this.web3 = web3;
        }
    }
}