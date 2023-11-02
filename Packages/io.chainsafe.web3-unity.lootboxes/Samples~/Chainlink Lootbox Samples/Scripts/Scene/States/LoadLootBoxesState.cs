using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LootBoxes.Chainlink.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class LoadLootBoxesState : LootBoxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            var stageItems = await SpawnStageItems();
            Context.stage.Clear();
            Context.stage.SetItems(stageItems);
            Context.stageFocus.Focus(0, immediately: true);
            Context.animator.SetTrigger("LootBoxesLoaded");
        }

        private async Task<List<StageItem>> SpawnStageItems()
        {
            var typeId = Context.ActiveType;
            var balance = await Context.GetBalance(typeId);
            var prefab = Context.frontEndDataSet.GetLootBoxTypeInfo(typeId).StageItemPrefab;
            var itemCount = Mathf.Min((int)balance, Context.stage.CurrentMaxItems);
            var stageItems = Enumerable.Range(0, itemCount)
                .Select(index => Context.LootBoxStageItemFactory.Create(prefab, index))
                .ToList();

            return stageItems;
        }
    }
}
