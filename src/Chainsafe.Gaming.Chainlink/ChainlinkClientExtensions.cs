using ChainSafe.Gaming.Web3;

namespace Chainsafe.Gaming.Chainlink
{
    /// <summary>
    /// Provides extension methods for Web3 to easily access Chainlink-specific functionalities.
    /// </summary>
    public static class ChainlinkClientExtensions
    {
        /// <summary>
        /// Adds Chainlink-specific functionalities to the given Web3 instance.
        /// </summary>
        /// <param name="web3">The Web3 instance to extend.</param>
        /// <returns>A <see cref="ChainlinkSubCategory"/> object which offers Chainlink-specific methods.</returns>
        public static ChainlinkSubCategory Chainlink(this Web3 web3)
        {
            return new ChainlinkSubCategory(web3);
        }
    }
}