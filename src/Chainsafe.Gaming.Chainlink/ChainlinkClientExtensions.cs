using ChainSafe.GamingWeb3;

namespace Chainsafe.Gaming.Chainlink
{
    public static class ChainlinkClientExtensions
    {
        public static ChainlinkClient Chainlink(this Web3 web3)
        {
            return new ChainlinkClient(web3);
        }
    }
}