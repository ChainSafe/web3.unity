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
        public string? ContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the ABI (Application Binary Interface) of the smart contract that the LootboxService interacts with.
        /// The ABI is essential for decoding the data and events emitted by Ethereum contracts.
        /// </summary>
        public string? ContractAbi { get; set; }
    }
}