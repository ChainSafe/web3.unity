namespace LootBoxes.Chainlink.Scene.States
{
    public class LoadRewardsState : LootBoxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            Context.stage.Clear();

            var stageItems = await Context.rewardSpawner.Spawn(Context.LastClaimedRewards);
            if (stageItems.Count > Context.stage.CurrentMaxItems)
            {
                Context.stage.SetTempMaxItems(stageItems.Count);
            }

            Context.stage.SetItems(stageItems);
            Context.stageFocus.Focus(0, immediately: true);
            Context.animator.SetTrigger("RewardsLoaded");
        }
    }
}