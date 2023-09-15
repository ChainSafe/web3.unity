using LootBoxes.Scene.StageItems;

namespace LootBoxes.Scene.States
{
    public class LoadLootBoxesState : LootboxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            var typeId = Context.ActiveType;
            var balance = await Context.GetBalance(typeId);
        
            if (!Context.prefabSet.PrefabByTypeId.ContainsKey(typeId))
            {
                throw new LootBoxSceneException($"No {nameof(StageItem)} prefab found for lootbox type {typeId}.");
            }
            
            var prefab = Context.prefabSet.PrefabByTypeId[typeId];
            Context.stage.SpawnItems(prefab, balance);
            Context.stageFocus.FocusImmediately(0);
            Context.Animator.SetTrigger("Next");
        }
    }
}
