namespace LootBoxes.Scene.States
{
    public class ResetStageState : LootboxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.stage.Clear();
            Context.ActiveStageItem = 0;
            Context.Animator.SetTrigger("Next");
        }
    }
}