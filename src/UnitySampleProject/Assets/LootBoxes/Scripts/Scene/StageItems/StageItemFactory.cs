using UnityEngine;
using Object = UnityEngine.Object;

namespace LootBoxes.Scene.StageItems
{
    public class StageItemFactory
    {
        public StageItem Create(StageItem prefab, int index, Vector3 position, Quaternion rotation, Transform parent)
        {
            // todo use pools
            var item = Object.Instantiate(prefab, position, rotation, parent);
            // todo set data here
            return item;
        }
    }
}