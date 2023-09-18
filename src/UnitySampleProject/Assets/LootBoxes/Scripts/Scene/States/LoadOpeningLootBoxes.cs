using System.Linq;

namespace LootBoxes.Scene.States
{
    public class LoadOpeningLootBoxes : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            var prefab = Context.prefabSet.GetLootBoxStageItemPrefab(Context.ActiveType);
            Context.stage.SpawnItems(prefab, 1);
            Context.stageFocus.FocusImmediately(0);
            var stageItem = Context.stage.StageItems.Single();
            stageItem.LootBox.PlayOpenInitiated(); // todo set opening state immediately?
            Context.animator.SetTrigger("Next");
        }
    }
}