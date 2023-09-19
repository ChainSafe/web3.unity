using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using UnityEngine;

namespace LootBoxes.Scene.States
{
    public class SelectLootBoxesState : LootBoxSceneState
    {
        private List<LootboxTypeInfo> availableLootBoxTypes;

        protected override async void OnLootBoxSceneStateEnter()
        {
            Context.selectLootBoxesUI.gameObject.SetActive(true);

            SetTypeAndAmountLabel();

            availableLootBoxTypes = Context.LastFetchedLootBoxes.Where(info => info.Amount > 0).ToList();
            
            Context.selectLootBoxesUI.PrevTypeButton.onClick.AddListener(OnPrevTypeClick);
            Context.selectLootBoxesUI.NextTypeButton.onClick.AddListener(OnNextTypeClick);
            Context.selectLootBoxesUI.PrevLootBoxButton.onClick.AddListener(OnPrevLootBoxClick);
            Context.selectLootBoxesUI.NextLootBoxButton.onClick.AddListener(OnNextLootBoxClick);
            Context.selectLootBoxesUI.OpenSelectedButton.onClick.AddListener(OnOpenSelectedClick);

            foreach (var stageItem in Context.stage.StageItems)
            {
                stageItem.LootBox.Clicked += OnLootBoxClicked;
            }
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.selectLootBoxesUI.gameObject.SetActive(false);
            
            Context.selectLootBoxesUI.PrevTypeButton.onClick.RemoveListener(OnPrevTypeClick);
            Context.selectLootBoxesUI.NextTypeButton.onClick.RemoveListener(OnNextTypeClick);
            Context.selectLootBoxesUI.PrevLootBoxButton.onClick.RemoveListener(OnPrevLootBoxClick);
            Context.selectLootBoxesUI.NextLootBoxButton.onClick.RemoveListener(OnNextLootBoxClick);
            Context.selectLootBoxesUI.OpenSelectedButton.onClick.RemoveListener(OnOpenSelectedClick);

            foreach (var stageItem in Context.stage.StageItems)
            {
                stageItem.LootBox.Clicked -= OnLootBoxClicked;
            }
        }

        protected override void OnLootBoxSceneStateUpdate()
        {
            // todo use keyboard input 
        }

        private async void SetTypeAndAmountLabel()
        {
            var typeInfo = Context.frontEndDataSet.GetLootBoxTypeInfo(Context.ActiveType);
            var amount = await Context.GetBalance(Context.ActiveType);
            var label = Context.selectLootBoxesUI.LootBoxTypeAndAmount;
            label.text = string.Empty;
            label.text = $"({amount}) {typeInfo.Name}";
            label.color = typeInfo.Color;
        }

        private void ChangeLootBoxType(int delta)
        {
            var currentTypeIndex = availableLootBoxTypes.FindIndex(info => info.TypeId == Context.ActiveType);
            var nextTypeIndex = (int)Mathf.Repeat(currentTypeIndex + delta, availableLootBoxTypes.Count);
            var nextType = availableLootBoxTypes[nextTypeIndex].TypeId;
            Context.ActiveType = nextType;
            Context.animator.SetTrigger("LootBoxTypeChanged");
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
            ChangeLootBoxType(+1);
        }

        private void OnPrevTypeClick()
        {
            ChangeLootBoxType(-1);
        }

        private void OnOpenSelectedClick()
        {
            if (!Context.stage.StageItems.Any(item => item.LootBox.Selected))
            {
                return;
            }
            
            Context.animator.SetTrigger("OpenLootboxes");
        }

        private void OnLootBoxClicked(LootBox lootBox)
        {
            if (Context.stageFocus.FocusedItem.LootBox != lootBox)
            {
                return;
            }

            lootBox.Selected = !lootBox.Selected;
        }
    }
}