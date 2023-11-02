using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class SelectLootBoxesState : LootBoxSceneState
    {
        private List<LootboxTypeInfo> availableLootBoxTypes;

        protected override void OnLootBoxSceneStateEnter()
        {
            Context.selectLootBoxesUI.gameObject.SetActive(true);

            SetCallToAction();
            OnSelectedCountUpdated();

            availableLootBoxTypes = Context.LastFetchedLootBoxes.Where(info => info.Amount > 0).ToList();

            Context.selectLootBoxesUI.PrevTypeButton.onClick.AddListener(OnPrevTypeClick);
            Context.selectLootBoxesUI.NextTypeButton.onClick.AddListener(OnNextTypeClick);
            Context.selectLootBoxesUI.PrevLootBoxButton.onClick.AddListener(OnPrevLootBoxClick);
            Context.selectLootBoxesUI.NextLootBoxButton.onClick.AddListener(OnNextLootBoxClick);
            Context.selectLootBoxesUI.OpenSelectedButton.onClick.AddListener(OnOpenSelectedClick);

            foreach (var stageItem in Context.stage.StageItems) stageItem.lootbox.Clicked += OnLootboxClicked;
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.selectLootBoxesUI.gameObject.SetActive(false);

            Context.selectLootBoxesUI.PrevTypeButton.onClick.RemoveListener(OnPrevTypeClick);
            Context.selectLootBoxesUI.NextTypeButton.onClick.RemoveListener(OnNextTypeClick);
            Context.selectLootBoxesUI.PrevLootBoxButton.onClick.RemoveListener(OnPrevLootBoxClick);
            Context.selectLootBoxesUI.NextLootBoxButton.onClick.RemoveListener(OnNextLootBoxClick);
            Context.selectLootBoxesUI.OpenSelectedButton.onClick.RemoveListener(OnOpenSelectedClick);

            foreach (var stageItem in Context.stage.StageItems) stageItem.lootbox.Clicked -= OnLootboxClicked;
        }

        protected override void OnLootBoxSceneStateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Context.stageFocus.FocusDelta(-1);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Context.stageFocus.FocusDelta(+1);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ChangeLootBoxType(-1);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) ChangeLootBoxType(+1);

            if (Input.GetKeyDown(KeyCode.Space)) ToggleFocusedSelected();

            if (Input.GetKeyDown(KeyCode.Return)) OpenSelectedLootBoxes();
        }

        private void ToggleFocusedSelected()
        {
            var focusedLootBox = Context.stageFocus.FocusedItem.lootbox;
            focusedLootBox.Selected = !focusedLootBox.Selected;
            OnSelectedCountUpdated();
        }

        private async void SetCallToAction()
        {
            var typeInfo = Context.frontEndDataSet.GetLootBoxTypeInfo(Context.ActiveType);
            var amount = await Context.GetBalance(Context.ActiveType);
            var label = Context.selectLootBoxesUI.CallToAction;
            label.text = string.Empty;
            label.text = $"Open <color=#{typeInfo.Color.ToHexString()}><b>{typeInfo.Name}</b></color> loot box(es)";
        }

        private void ChangeLootBoxType(int delta)
        {
            var currentTypeIndex = availableLootBoxTypes.FindIndex(info => info.TypeId == Context.ActiveType);
            var nextTypeIndex = (int)Mathf.Repeat(currentTypeIndex + delta, availableLootBoxTypes.Count);
            var nextType = availableLootBoxTypes[nextTypeIndex].TypeId;
            Context.ActiveType = nextType;
            Context.animator.SetTrigger("LootBoxTypeChanged");
        }

        private void OpenSelectedLootBoxes()
        {
            if (!Context.stage.StageItems.Any(item => item.lootbox.Selected)) return;

            Context.animator.SetTrigger("OpenLootboxes");
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
            OpenSelectedLootBoxes();
        }

        private void OnLootboxClicked(Lootbox lootbox)
        {
            if (Context.stageFocus.FocusedItem.lootbox != lootbox) return;

            ToggleFocusedSelected();
        }

        private void OnSelectedCountUpdated()
        {
            var selectedAmount = Context.stage.StageItems.Count(item => item.lootbox.Selected);
            Context.selectLootBoxesUI.SelectedAmount.text = selectedAmount.ToString();
            Context.selectLootBoxesUI.OpenSelectedButton.interactable = selectedAmount > 0;
        }
    }
}