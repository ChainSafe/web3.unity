namespace Chainsafe.Gaming.Chainlink
{
    using ChainSafe.Gaming.Web3;

    public static class ChainlinkClientExtensions
    {
        public static ChainlinkSubCategory Chainlink(this Web3 web3)
        {
            return new ChainlinkSubCategory(web3);
        }
    }
}