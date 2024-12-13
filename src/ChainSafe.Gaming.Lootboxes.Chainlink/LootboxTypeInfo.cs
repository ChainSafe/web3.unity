namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxTypeInfo
    {
        /// <summary>
        /// Lootbox Type ID. (Common, Rare, Epic, etc.)
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Current Balance of Lootboxes for this Type.
        /// </summary>
        public int Amount { get; set; }
    }
}