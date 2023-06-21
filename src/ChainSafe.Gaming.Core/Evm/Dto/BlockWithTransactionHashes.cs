﻿using Newtonsoft.Json;

namespace ChainSafe.Gaming.Evm
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