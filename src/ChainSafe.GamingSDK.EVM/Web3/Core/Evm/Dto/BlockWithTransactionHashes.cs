using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.Blocks
{
    /// <summary>
    ///     Block including just the transaction hashes.
    /// </summary>
    public class BlockWithTransactionHashes : Block
    {
        /// <summary>
        ///     Array - Array of transaction hashes.
        /// </summary>
        [JsonProperty(PropertyName = "transactions")]
        public string[] TransactionHashes { get; set; }
    }
}