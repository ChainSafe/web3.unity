using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.StageItems
{
    [CreateAssetMenu(menuName = Menues.Root + "LootBoxPrefabSet", fileName = "LootBoxPrefabSet", order = 0)]
    public class LootBoxesFrontEndDataSet : ScriptableObject
    {
        [Serializable]
        public class LootBoxTypeInfo
        {
            public uint TypeId;
            public string Name;
            public Color Color = Color.white;
            public StageItem StageItemPrefab;
        }

        [SerializeField] private List<LootBoxTypeInfo> lootBoxPrefabByTypeIdList;

        private Dictionary<uint, LootBoxTypeInfo> _lootBoxPrefabByTypeId;

        private Dictionary<uint, LootBoxTypeInfo> TypeInfoByTypeId =>
            _lootBoxPrefabByTypeId ??= lootBoxPrefabByTypeIdList.ToDictionary(t => t.TypeId, t => t);

        public LootBoxTypeInfo GetLootBoxTypeInfo(uint typeId)
        {
            if (!TypeInfoByTypeId.ContainsKey(typeId))
            {
                throw new LootBoxSceneException($"No {nameof(StageItem)} prefab found for lootbox type {typeId}.");
            }

            return TypeInfoByTypeId[typeId];
        }
    }
}