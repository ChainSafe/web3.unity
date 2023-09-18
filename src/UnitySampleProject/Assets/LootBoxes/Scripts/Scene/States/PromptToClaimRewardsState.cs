using LootBoxes.Scene.StageItems;

namespace LootBoxes.Scene.States
{
    public class PromptToClaimRewardsState : LootBoxSceneState
    {
        private StageItem focusedItem;

        protected override void OnLootBoxSceneStateEnter()
        {
            focusedItem = Context.stageFocus.FocusedItem;
            focusedItem.LootBox.Clicked += OnLootBoxClicked;
        }

        protected override void OnLootBoxSceneStateExit()
        {
            focusedItem.LootBox.Clicked -= OnLootBoxClicked;
        }

        private async void OnLootBoxClicked(LootBox lootBox)
        {
            focusedItem.LootBox.PlayClaimingRewards();
            Context.LastClaimedRewards = await Context.ClaimRewards();
            focusedItem.LootBox.PlayDisintegrate();
            Context.animator.SetTrigger("ClaimedRewards");
        }
    }
}