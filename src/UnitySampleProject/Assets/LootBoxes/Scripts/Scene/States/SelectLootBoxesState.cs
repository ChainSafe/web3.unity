using System.Linq;

namespace LootBoxes.Scene.States
{
    public class SelectLootBoxesState : LootboxSceneState
    {
        private int FocusedLootBoxIndex;
        
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.SelectLootBoxesUI.gameObject.SetActive(true);
            
            Context.SelectLootBoxesUI.PrevTypeButton.onClick.AddListener(OnPrevTypeClick);
            Context.SelectLootBoxesUI.NextTypeButton.onClick.AddListener(OnNextTypeClick);
            Context.SelectLootBoxesUI.PrevLootBoxButton.onClick.AddListener(OnPrevLootBoxClick);
            Context.SelectLootBoxesUI.NextLootBoxButton.onClick.AddListener(OnNextLootBoxClick);
            Context.SelectLootBoxesUI.OpenSelectedButton.onClick.AddListener(OnOpenSelectedClick);

            FocusedLootBoxIndex = 0;
            Context.stageFocus.Focus(0);
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.SelectLootBoxesUI.gameObject.SetActive(false);
            
            Context.SelectLootBoxesUI.PrevTypeButton.onClick.RemoveListener(OnPrevTypeClick);
            Context.SelectLootBoxesUI.NextTypeButton.onClick.RemoveListener(OnNextTypeClick);
            Context.SelectLootBoxesUI.PrevLootBoxButton.onClick.RemoveListener(OnPrevLootBoxClick);
            Context.SelectLootBoxesUI.NextLootBoxButton.onClick.RemoveListener(OnNextLootBoxClick);
            Context.SelectLootBoxesUI.OpenSelectedButton.onClick.RemoveListener(OnOpenSelectedClick);
        }

        private void OnNextLootBoxClick()
        {
            Context.stageFocus.FocusDelta(+1);
        }

        private void OnPrevLootBoxClick()
        {
            Context.stageFocus.FocusDelta(-1);
        }

        private void OnNextTypeClick()
        {
            throw new System.NotImplementedException();
        }

        private void OnPrevTypeClick()
        {
            throw new System.NotImplementedException();
        }

        private void OnOpenSelectedClick()
        {
            var selected = Context.stage.StageItems.Select(item => item.Selected);
        }
    }
}