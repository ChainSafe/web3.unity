using System.Collections.Generic;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxItemList : List<List<List<Item>>>
    {
    }

    public class Parameter
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }

    public class Item
    {
        public Parameter Parameter { get; set; }

        public object Result { get; set; }
    }
}