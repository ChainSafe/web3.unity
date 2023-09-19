namespace LootBoxes.Scene.States
{
    public class LoadRewardsState : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.stage.Clear();
            
            var stageItems = Context.rewardSpawner.Spawn(Context.LastClaimedRewards);
            if (stageItems.Count > Context.stage.CurrentMaxItems)
            {
                Context.stage.SetTempMaxItems(stageItems.Count);
            }
            
            Context.stage.SetItems(stageItems);
            Context.stageFocus.FocusImmediately(0);
            Context.animator.SetTrigger("RewardsLoaded");
        }
    }
}