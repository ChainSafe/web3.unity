using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LootBoxes.Scene.StageItems
{
    [CreateAssetMenu(menuName = Menues.Root + "LootBoxPrefabSet", fileName = "LootBoxPrefabSet", order = 0)]
    public class StageItemPrefabSet : ScriptableObject
    {
        [Serializable]
        public class LootBoxPrefabByTypeId
        {
            public uint TypeId;
            public StageItem Prefab;
        }

        public List<LootBoxPrefabByTypeId> PrefabByTypeIdList;
        
        private Dictionary<uint, StageItem> _prefabByTypeId;

        public Dictionary<uint, StageItem> PrefabByTypeId =>
            _prefabByTypeId ??= PrefabByTypeIdList.ToDictionary(t => t.TypeId, t => t.Prefab);
    }
}