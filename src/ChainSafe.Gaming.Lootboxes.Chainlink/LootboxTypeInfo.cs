namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxTypeInfo
    {
        /// <summary>
        /// Lootbox Type ID. (Common, Rare, Epic, etc.)
        /// </summary>
        public uint TypeId { get; set; }

        /// <summary>
        /// Current Balance of Lootboxes for this Type.
        /// </summary>
        public uint Amount { get; set; }
    }
}