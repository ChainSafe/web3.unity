using System.Collections.Generic;
using System.Numerics;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    /// <summary>
    /// Provides configuration settings for the LootboxService.
    /// This includes details about the smart contract that the service interacts with.
    /// </summary>
    public class LootboxServiceConfig
    {
        /// <summary>
        /// Gets or sets the Ethereum address of the smart contract that the LootboxService interacts with.
        /// </summary>
        public string? LootboxAddress { get; set; }

        /// <summary>
        /// Gets or sets the lootbox account if you want to get balances from different accounts.
        /// </summary>
        public string? LootboxAccount { get; set; }

        /// <summary>
        /// Gets or sets the ABI (Application Binary Interface) of the smart contract that the LootboxService interacts with.
        /// The ABI is essential for decoding the data and events emitted by Ethereum contracts.
        /// </summary>
        public string? ContractAbi { get; set; }

        /// <summary>
        /// Gets or sets the Erc20 contracts that belong to this LootboxService.
        /// This is needed to spawn the items related to the Lootbox.
        /// </summary>
        public List<string> Erc20Contracts { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the Erc721 contracts that belong to this LootboxService.
        /// This is needed to spawn the items related to the Lootbox.
        /// </summary>
        public List<string> Erc721Contracts { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the Erc1155 contracts that belong to this LootboxService.
        /// This is needed to spawn the items related to the Lootbox.
        /// </summary>
        public List<string> Erc1155Contracts { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the Erc721 token ids that belong to this LootboxService.
        /// This is needed to spawn the items related to the Lootbox.
        /// </summary>
        public List<BigInteger> Erc721TokenIds { get; set; } = new List<BigInteger>();

        /// <summary>
        /// Gets or sets the Erc1155 token ids that belong to this LootboxService.
        /// This is needed to spawn the items related to the Lootbox.
        /// </summary>
        public List<BigInteger> Erc1155TokenIds { get; set; } = new List<BigInteger>();
    }
}