using ChainSafe.Gaming.Web3;

namespace Chainsafe.Gaming.Chainlink
{
    public static class ChainlinkClientExtensions
    {
        public static ChainlinkSubCategory Chainlink(this Web3 web3)
        {
            return new ChainlinkSubCategory(web3);
        }
    }
}