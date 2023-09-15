using System.Collections.Generic;
using System.Linq;
using LootBoxes.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Scene
{
    public class Stage : MonoBehaviour
    {
        public float radius = 10;
        public int maxItems = 10;
        
        private StageItemFactory factory;
        private List<StageItem> stageItems;
        
        public int LootBoxCount => stageItems?.Count ?? 0;
        public IReadOnlyCollection<StageItem> StageItems => stageItems?.AsReadOnly();

        public void Configure(StageItemFactory factory)
        {
            this.factory = factory;
        }

        public Vector3 GetStagePosition(int index)
        {
            var rotation = Quaternion.AngleAxis(-1 * index * 360f / maxItems, Vector3.up);
            return transform.position + rotation * new Vector3(0, 0, -radius);
        }

        public Vector3 GetStageVector(int index)
        {
            return (GetStagePosition(index) - transform.position).normalized;
        }
        
        public void SpawnItems(StageItem prefab, uint amount)
        {
            if (stageItems != null)
            {
                throw new LootBoxSceneException("Stage items should be cleared first.");
            }
            
            var itemCount = Mathf.Min((int)amount, maxItems);
            stageItems = Enumerable.Range(0, itemCount)
                .Select(index =>
                {
                    var position = GetStagePosition(index);
                    var rotation = Quaternion.LookRotation(GetStageVector(index));
                    return factory.Create(prefab, index, position, rotation, transform);
                })
                .ToList();
        }
        
        public void Clear()
        {
            if (stageItems == null)
            {
                return;
            }
            
            foreach (var stageItem in stageItems)
            {
                Destroy(stageItem);
            }

            stageItems = null;
        }
    }
}