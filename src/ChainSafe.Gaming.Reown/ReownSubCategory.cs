using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.Reown
{
    public class ReownSubCategory : IWeb3SubCategory
    {
        private readonly Web3.Web3 web3;

        public ReownSubCategory(Web3.Web3 web3)
        {
            this.web3 = web3;
        }

        Web3.Web3 IWeb3SubCategory.Web3 => web3;
    }
}