using LootBoxes.Scene.StageItems;

namespace LootBoxes.Scene.States
{
    public class PromptToClaimRewardsState : LootBoxSceneState
    {
        private StageItem focusedItem;

        protected override void OnLootBoxSceneStateEnter()
        {
            Context.promptClaimRewardsUI.gameObject.SetActive(true);
            
            focusedItem = Context.stageFocus.FocusedItem;
            focusedItem.LootBox.PlayReadyToBeClaimed();
            
            focusedItem.LootBox.Clicked += OnLootBoxClicked;
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.promptClaimRewardsUI.gameObject.SetActive(false);
            
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