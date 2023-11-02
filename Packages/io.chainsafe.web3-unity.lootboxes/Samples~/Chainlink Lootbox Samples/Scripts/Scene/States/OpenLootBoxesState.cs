using System.Linq;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class OpenLootBoxesState : LootBoxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            Context.openLootBoxUI.gameObject.SetActive(true);

            // Focus nearest selected item, play animations
            var stageItemToFocusIndex = GetStageItemToFocus();
            Context.stageFocus.Focus(stageItemToFocusIndex);
            PlayAnimations();

            // Send "OpenLootBoxes" request
            var amountToOpen = (uint)Context.stage.StageItems.Count(item => item.lootbox.Selected);
            await Context.OpenLootBoxes(Context.ActiveType, amountToOpen);

            Context.animator.SetTrigger("LootBoxesOpenInitiated");

            int GetStageItemToFocus()
            {
                // Nearest selected item to the focused one
                return Context.stage.StageItems
                    .Select((item, index) => (item, index))
                    .Where(itemAndIndex => itemAndIndex.item.lootbox.Selected)
                    .Aggregate((a, b) =>
                    {
                        var focusedItemIndex = Context.stageFocus.FocusedItemIndex;
                        return Mathf.Abs(b.index - focusedItemIndex) < Mathf.Abs(a.index - focusedItemIndex) ? b : a;
                    }).index;
            }

            void PlayAnimations()
            {
                var focusedStageItem = Context.stage.StageItems.Skip(stageItemToFocusIndex).First();
                var itemsToHide = Context.stage.StageItems.Where(item => item != focusedStageItem);
                foreach (var itemToHide in itemsToHide)
                {
                    itemToHide.lootbox.PlayDisintegrate();
                }

                var itemsToOpen = Context.stage.StageItems.Where(item => item.lootbox.Selected);
                foreach (var itemToOpen in itemsToOpen)
                {
                    itemToOpen.lootbox.PlayOpening();
                }
            }
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.openLootBoxUI.gameObject.SetActive(false);
        }
    }
}