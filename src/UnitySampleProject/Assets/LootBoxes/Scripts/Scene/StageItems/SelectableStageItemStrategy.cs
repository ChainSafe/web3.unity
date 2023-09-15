using UnityEngine;

namespace LootBoxes.Scene.StageItems
{
    [CreateAssetMenu(menuName = Menues.Root + "SelectableStageItemStrategy", fileName = "SelectableStageItemStrategy", order = 0)]
    public class SelectableStageItemStrategy : StageItemStrategy
    {
        public override void OnClick()
        {
            base.OnClick();
            Context.Selected = !Context.Selected;
        }
    }
}