using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class ViewRewardsState : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.viewRewardsUI.gameObject.SetActive(true);

            Context.viewRewardsUI.PrevRewardButton.onClick.AddListener(OnPrevRewardClick);
            Context.viewRewardsUI.NextRewardButton.onClick.AddListener(OnNextRewardClick);
            Context.viewRewardsUI.ContinueButton.onClick.AddListener(OnContinueClick);
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.stage.ResetTempMaxItems();
            Context.viewRewardsUI.gameObject.SetActive(false);

            Context.viewRewardsUI.PrevRewardButton.onClick.RemoveListener(OnPrevRewardClick);
            Context.viewRewardsUI.NextRewardButton.onClick.RemoveListener(OnNextRewardClick);
            Context.viewRewardsUI.ContinueButton.onClick.RemoveListener(OnContinueClick);
        }

        protected override void OnLootBoxSceneStateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Context.stageFocus.FocusDelta(-1);
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Context.stageFocus.FocusDelta(+1);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Continue();
            }
        }

        private void Continue()
        {
            Context.animator.SetTrigger("FinishedViewingRewards");
        }

        private void OnPrevRewardClick()
        {
            Context.stageFocus.FocusDelta(-1);
        }

        private void OnNextRewardClick()
        {
            Context.stageFocus.FocusDelta(+1);
        }

        private void OnContinueClick()
        {
            Continue();
        }
    }
}