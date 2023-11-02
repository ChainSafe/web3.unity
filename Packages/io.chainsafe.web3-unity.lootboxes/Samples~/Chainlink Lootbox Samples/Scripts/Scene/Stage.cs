using System.Collections.Generic;
using LootBoxes.Chainlink.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class Stage : MonoBehaviour
    {
        public float radius = 10;
        [SerializeField] private int maxItems = 10;

        private List<StageItem> stageItems;
        private int? tempMaxItems;

        public int CurrentMaxItems => tempMaxItems ?? maxItems;
        public int StageItemCount => stageItems?.Count ?? 0;
        public IReadOnlyCollection<StageItem> StageItems => stageItems?.AsReadOnly();

        public Vector3 GetStagePosition(int index)
        {
            var rotation = Quaternion.AngleAxis(-1 * index * 360f / CurrentMaxItems, Vector3.up);
            return transform.position + rotation * new Vector3(0, 0, -radius);
        }

        public Vector3 GetStageVector(int index)
        {
            return (GetStagePosition(index) - transform.position).normalized;
        }

        public void SetItems(List<StageItem> items)
        {
            if (stageItems != null)
            {
                throw new LootBoxSceneException("Stage items should be cleared first.");
            }

            stageItems = items;

            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var position = GetStagePosition(index);
                var rotation = Quaternion.LookRotation(GetStageVector(index));
                var itemTransform = item.transform;
                itemTransform.parent = transform;
                itemTransform.position = position;
                itemTransform.rotation = rotation;
            }
        }

        public void Clear()
        {
            if (stageItems == null)
            {
                return;
            }

            foreach (var stageItem in stageItems)
            {
                Destroy(stageItem.gameObject);
            }

            stageItems = null;
        }

        public void SetTempMaxItems(int tempMaxItems)
        {
            this.tempMaxItems = tempMaxItems;
        }

        public void ResetTempMaxItems()
        {
            this.tempMaxItems = null;
        }
    }
}