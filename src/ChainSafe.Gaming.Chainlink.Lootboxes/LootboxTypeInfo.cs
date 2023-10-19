namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    public class LootboxTypeInfo
    {
        /// <summary>
        /// Lootbox Type ID. (Common, Rare, Epic, etc.)
        /// </summary>
        public uint TypeId { get; set; }

        /// <summary>
        /// Amount of the lootboxes.
        /// </summary>
        public uint Amount { get; set; }
    }
}