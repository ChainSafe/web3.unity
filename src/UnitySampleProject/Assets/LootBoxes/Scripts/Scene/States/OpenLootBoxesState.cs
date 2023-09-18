using System.Linq;
using UnityEngine;

namespace LootBoxes.Scene.States
{
    public class OpenLootBoxesState : LootBoxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            // Focus nearest selected item, play animations
            var stageItemToFocusIndex = GetStageItemToFocus();
            Context.stageFocus.Focus(stageItemToFocusIndex);
            PlayAnimations();

            // Send "OpenLootBoxes" request
            var amountToOpen = (uint) Context.stage.StageItems.Count(item => item.LootBox.Selected);
            await Context.OpenLootBoxes(Context.ActiveType, amountToOpen);
            
            Context.animator.SetTrigger("Next");

            int GetStageItemToFocus()
            {
                // nearest selected item to the focused one
                return Context.stage.StageItems
                    .Select((item, index) => (item, index))
                    .Where(itemAndIndex => itemAndIndex.item.LootBox.Selected)
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
                    itemToHide.LootBox.PlayDisintegrate();
                }

                var itemsToOpen = Context.stage.StageItems.Where(item => item.LootBox.Selected);
                foreach (var itemToOpen in itemsToOpen)
                {
                    itemToOpen.LootBox.PlayOpenInitiated();
                }
            }
        }
    }
}