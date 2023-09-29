using System.Collections.Generic;
using LootBoxes.Scene.StageItems;

namespace LootBoxes.Scene.States
{
    public class LoadOpeningLootBoxes : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            var prefab = Context.frontEndDataSet.GetLootBoxTypeInfo(Context.ActiveType).StageItemPrefab;
            var stageItem = Context.LootBoxStageItemFactory.Create(prefab, 0);
            Context.stage.SetItems(new List<StageItem> { stageItem });
            Context.stageFocus.Focus(0, immediately: true);
            stageItem.LootBox.PlayOpening(); // todo set opening state immediately?
            Context.animator.SetTrigger("LootBoxesLoaded");
        }
    }
}