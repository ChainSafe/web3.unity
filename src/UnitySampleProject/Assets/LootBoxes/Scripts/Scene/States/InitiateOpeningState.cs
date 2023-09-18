using System.Linq;
using UnityEngine;

namespace LootBoxes.Scene.States
{
    public class InitiateOpeningState : LootBoxSceneState
    {
        protected override void OnLootBoxSceneStateEnter()
        {
            // nearest selected item to the focused one
            var stageItemToFocusIndex = Context.stage.StageItems
                .Select((item, index) => (item, index))
                .Where(itemAndIndex => itemAndIndex.item.LootBox.Selected)
                .Aggregate((a, b) =>
                {
                    var focusedItemIndex = Context.stageFocus.FocusedItemIndex;
                    return Mathf.Abs(b.index - focusedItemIndex) < Mathf.Abs(a.index - focusedItemIndex) ? b : a;
                }).index;
            
            Context.stageFocus.Focus(stageItemToFocusIndex);

            var stageItemToFocus = Context.stage.StageItems.Skip(stageItemToFocusIndex).First();
            var itemsToHide = Context.stage.StageItems.Where(item => item != stageItemToFocus);
            foreach (var itemToHide in itemsToHide)
            {
                itemToHide.LootBox.PlayDisintegrate();
            }

            var itemsToOpen = Context.stage.StageItems.Where(item => item.LootBox.Selected);
            foreach (var itemToOpen in itemsToOpen)
            {
                itemToOpen.LootBox.PlayOpenInitiated();
            }
            
            Context.animator.SetTrigger("Next");
        }
    }
}