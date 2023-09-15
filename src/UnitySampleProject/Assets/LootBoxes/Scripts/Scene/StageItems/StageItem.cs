using UnityEngine;

namespace LootBoxes.Scene.StageItems
{
    public class StageItem : MonoBehaviour
    {
        public StageItemStrategy strategyPrototype;
        public Animator animator;
        
        private StageItemStrategy strategy;
        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                animator.SetBool("Selected", _selected);
            }
        }

        private void Awake()
        {
            strategy = Instantiate(strategyPrototype);
            strategy.Initialize(this);
        }

        private void OnDestroy()
        {
            Destroy(strategy);
        }

        public void OnClick()
        {
            strategy.OnClick();
        }
    }
}