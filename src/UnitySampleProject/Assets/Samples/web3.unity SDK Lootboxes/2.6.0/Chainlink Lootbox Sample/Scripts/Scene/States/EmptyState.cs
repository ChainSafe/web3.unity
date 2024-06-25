namespace LootBoxes.Chainlink.Scene.States
{
    public class EmptyState : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.emptyStateUI.gameObject.SetActive(true);
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.emptyStateUI.gameObject.SetActive(false);
        }
    }
}