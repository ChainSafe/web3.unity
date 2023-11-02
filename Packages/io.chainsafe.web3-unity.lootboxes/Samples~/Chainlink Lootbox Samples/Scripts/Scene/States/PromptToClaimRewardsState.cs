using System.Threading.Tasks;
using LootBoxes.Chainlink.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class PromptToClaimRewardsState : LootBoxSceneState
    {
        private StageItem focusedItem;

        protected override void OnLootBoxSceneStateEnter()
        {
            Context.promptClaimRewardsUI.gameObject.SetActive(true);

            focusedItem = Context.stageFocus.FocusedItem;
            focusedItem.lootbox.PlayReadyToBeClaimed();

            focusedItem.lootbox.Clicked += OnLootboxClicked;
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.promptClaimRewardsUI.gameObject.SetActive(false);

            focusedItem.lootbox.Clicked -= OnLootboxClicked;
        }

        protected override async void OnLootBoxSceneStateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                await ClaimRewards();
            }
        }

        private async Task ClaimRewards()
        {
            focusedItem.lootbox.PlayClaimingRewards();
            Context.LastClaimedRewards = await Context.ClaimRewards();
            focusedItem.lootbox.PlayDisintegrate();
            Context.animator.SetTrigger("ClaimedRewards");
        }

        private async void OnLootboxClicked(Lootbox lootbox)
        {
            await ClaimRewards();
        }
    }
}