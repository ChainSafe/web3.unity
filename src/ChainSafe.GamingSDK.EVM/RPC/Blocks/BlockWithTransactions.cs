using Newtonsoft.Json;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Blocks
{
    /// <summary>
    ///     Block including transaction objects.
    /// </summary>
    public class BlockWithTransactions : Block
    {
        /// <summary>
        ///     Array - Array of transaction objects.
        /// </summary>
        [JsonProperty(PropertyName = "transactions")]
        public TransactionResponse[] Transactions { get; set; }
    }
}