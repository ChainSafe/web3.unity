using System.Collections.Generic;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxItemList : List<OutterItem>
    {
    }

    public class OutterItem : List<InnerItem>
    {
    }

    public class InnerItem : List<Item>
    {
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