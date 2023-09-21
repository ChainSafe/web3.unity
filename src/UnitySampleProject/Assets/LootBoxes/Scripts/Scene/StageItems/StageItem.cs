using UnityEngine;

namespace LootBoxes.Scene.StageItems
{
    public class StageItem : MonoBehaviour
    {
        public LootBox LootBox;
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