using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// Helps in the construction of new FilterInput objects with default parameters set.
    /// </summary>
    public class FilterInputBuilder
    {
        /// <summary>
        /// Constructs a new FilterInput with default parameters set and given 'address', 'fromBlock', and 'toBlock'.
        /// </summary>
        /// <param name="address">A string representing a Ethereum blockchain address.</param>
        /// <param name="fromBlock">A block parameter object representing the block to start filtering from. Defaults to null.</param>
        /// <param name="toBlock">A block parameter object representing the block until which to perform the filter. Defaults to null.</param>
        /// <returns>Returns a NewFilterInput object with address, fromBlock, and toBlock set.</returns>
        public static NewFilterInput GetDefaultFilterInput(string address, BlockParameter fromBlock = null, BlockParameter toBlock = null)
        {
            string[] addresses = null;
            if (!string.IsNullOrEmpty(address))
            {
                addresses = new[] { address };
            }

            return GetDefaultFilterInput(addresses, fromBlock, toBlock);
        }

        /// <summary>
        /// Constructs a new FilterInput with default parameters set and given array of 'addresses', 'fromBlock', and 'toBlock'.
        /// </summary>
        /// <param name="addresses">An array of strings, each representing an Ethereum blockchain address.</param>
        /// <param name="fromBlock">A block parameter object representing the block to start filtering from. Defaults to null.</param>
        /// <param name="toBlock">A block parameter object representing the block until which to perform the filter. Defaults to null.</param>
        /// <returns>Returns a NewFilterInput object with address, fromBlock, and toBlock set.</returns>
        public static NewFilterInput GetDefaultFilterInput(string[] addresses, BlockParameter fromBlock = null, BlockParameter toBlock = null)
        {
            var ethFilterInput = new NewFilterInput
            {
                FromBlock = fromBlock ?? BlockParameter.CreateEarliest(),
                ToBlock = toBlock ?? BlockParameter.CreateLatest(),
                Address = addresses,
            };
            return ethFilterInput;
        }
    }
}