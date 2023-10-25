using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;

namespace Chainsafe.Gaming.Chainlink
{
    /// <summary>
    /// Represents a sub-category for Chainlink within the Web3 context.
    /// This allows for more specific Web3 operations related to Chainlink.
    /// </summary>
    public class ChainlinkSubCategory : IWeb3SubCategory
    {
        private readonly Web3 web3;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainlinkSubCategory"/> class.
        /// </summary>
        /// <param name="web3">The Web3 instance to be associated with this sub-category.</param>
        public ChainlinkSubCategory(Web3 web3)
        {
            this.web3 = web3;
        }

        /// <summary>
        /// Gets the associated Web3 instance for this sub-category.
        /// </summary>
        Web3 IWeb3SubCategory.Web3 => this.web3;
    }
}