namespace ChainSafe.Gaming.Lootboxes.Chainlink
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