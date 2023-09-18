namespace LootBoxes.Scene.States
{
    public class LoadLootBoxesState : LootBoxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            var typeId = Context.ActiveType;
            var balance = await Context.GetBalance(typeId);
            var prefab = Context.prefabSet.GetLootBoxStageItemPrefab(typeId);
            Context.stage.SpawnItems(prefab, balance);
            Context.stageFocus.FocusImmediately(0);
            Context.animator.SetTrigger("Next");
        }
    }
}
