namespace LootBoxes.Scene.States
{
    public class ResetStageState : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.stage.Clear();
            Context.stageFocus.FocusImmediately(0);
            Context.LastClaimedRewards = null;
            Context.animator.SetTrigger("Next");
        }
    }
}