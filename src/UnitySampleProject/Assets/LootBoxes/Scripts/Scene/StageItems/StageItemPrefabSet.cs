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

        public List<LootBoxPrefabByTypeId> LootBoxPrefabByTypeIdList;
        
        private Dictionary<uint, StageItem> _lootBoxPrefabByTypeId;

        private Dictionary<uint, StageItem> PrefabByTypeId =>
            _lootBoxPrefabByTypeId ??= LootBoxPrefabByTypeIdList.ToDictionary(t => t.TypeId, t => t.Prefab);

        public StageItem GetLootBoxStageItemPrefab(uint typeId)
        {
            if (!PrefabByTypeId.ContainsKey(typeId))
            {
                throw new LootBoxSceneException($"No {nameof(StageItem)} prefab found for lootbox type {typeId}.");
            }
            
            return PrefabByTypeId[typeId];
        }
    }
}