using UnityEngine;
using Object = UnityEngine.Object;

namespace LootBoxes.Chainlink.Scene.StageItems
{
    public class LootBoxStageItemFactory
    {
        public StageItem Create(StageItem prefab, int index)
        {
            // todo use pools
            var item = Object.Instantiate(prefab);
            // todo set data here
            return item;
        }
    }
}