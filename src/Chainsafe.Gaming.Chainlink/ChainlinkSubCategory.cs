using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;

namespace Chainsafe.Gaming.Chainlink
{
    public class ChainlinkSubCategory : IWeb3SubCategory
    {
        private readonly Web3 web3;

        public ChainlinkSubCategory(Web3 web3)
        {
            this.web3 = web3;
        }

        Web3 IWeb3SubCategory.Web3 => this.web3;
    }
}