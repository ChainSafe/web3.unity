using UnityEngine;

namespace LootBoxes.Scene.StageItems
{
    public abstract class StageItemStrategy : ScriptableObject
    {
        protected StageItem Context { get; private set; }

        public void Initialize(StageItem item)
        {
            Context = item;
        }

        public virtual void OnClick()
        {
            Context.animator.SetTrigger("Clicked");
        }
    }
}