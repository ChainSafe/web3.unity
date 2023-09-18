using System.Linq;

namespace LootBoxes.Scene.States
{
    public class SelectLootBoxesState : LootBoxSceneState
    {
        private int FocusedLootBoxIndex;
        
        protected override void OnLootBoxSceneStateEnter()
        {
            Context.selectLootBoxesUI.gameObject.SetActive(true);
            
            Context.selectLootBoxesUI.PrevTypeButton.onClick.AddListener(OnPrevTypeClick);
            Context.selectLootBoxesUI.NextTypeButton.onClick.AddListener(OnNextTypeClick);
            Context.selectLootBoxesUI.PrevLootBoxButton.onClick.AddListener(OnPrevLootBoxClick);
            Context.selectLootBoxesUI.NextLootBoxButton.onClick.AddListener(OnNextLootBoxClick);
            Context.selectLootBoxesUI.OpenSelectedButton.onClick.AddListener(OnOpenSelectedClick);

            FocusedLootBoxIndex = 0;
            Context.stageFocus.Focus(0);
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.selectLootBoxesUI.gameObject.SetActive(false);
            
            Context.selectLootBoxesUI.PrevTypeButton.onClick.RemoveListener(OnPrevTypeClick);
            Context.selectLootBoxesUI.NextTypeButton.onClick.RemoveListener(OnNextTypeClick);
            Context.selectLootBoxesUI.PrevLootBoxButton.onClick.RemoveListener(OnPrevLootBoxClick);
            Context.selectLootBoxesUI.NextLootBoxButton.onClick.RemoveListener(OnNextLootBoxClick);
            Context.selectLootBoxesUI.OpenSelectedButton.onClick.RemoveListener(OnOpenSelectedClick);
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
            if (!Context.stage.StageItems.Any(item => item.LootBox.Selected))
            {
                return;
            }
            
            // var selected = Context.stage.StageItems.Select(item => item.LootBox.Selected);
            Context.animator.SetTrigger("OpenInitiated");
        }
    }
}