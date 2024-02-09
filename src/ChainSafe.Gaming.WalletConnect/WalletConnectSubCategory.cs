using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectSubCategory : IWeb3SubCategory
    {
        private readonly Web3.Web3 web3;

        public WalletConnectSubCategory(Web3.Web3 web3)
        {
            this.web3 = web3;
        }

        Web3.Web3 IWeb3SubCategory.Web3 => web3;
    }
}