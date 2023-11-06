using UnityEngine;
using UnityEngine.Serialization;

namespace LootBoxes.Chainlink.Scene.StageItems
{
    public class StageItem : MonoBehaviour
    {
        [FormerlySerializedAs("LootBox")] public Lootbox lootbox;
        public Reward Reward;
        public Light SpotLight;

        private void Awake()
        {
            SetSpotlightActive(false);
        }

        public void SetSpotlightActive(bool active)
        {
            SpotLight.gameObject.SetActive(active);
        }
    }
}