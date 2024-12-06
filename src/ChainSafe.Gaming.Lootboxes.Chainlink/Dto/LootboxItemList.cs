using System.Collections.Generic;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxItemList
    {
        public List<InnerItem> OuterItem { get; set; }
    }

    public class OuterItem
    {
        public List<InnerItem> InnerItem { get; set; }
    }

    public class InnerItem
    {
        public List<Item> Item { get; set; }
    }

    public class Item
    {
        public Parameter Parameter { get; set; }

        public object Result { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }
}