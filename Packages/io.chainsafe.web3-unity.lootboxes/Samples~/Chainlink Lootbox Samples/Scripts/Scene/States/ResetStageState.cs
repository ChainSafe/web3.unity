using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class ResetStageState : LootBoxSceneState
    {
        protected override async void OnLootBoxSceneStateEnter()
        {
            Context.stage.Clear();
            Context.LastClaimedRewards = null;
            Context.stageFocus.Focus(0, immediately: true);

            if (await Context.CanClaimRewards())
            {
                LaunchCanClaimRewards();
                return;
            }

            if (await Context.IsOpeningLootBox())
            {
                LaunchOpening();
                return;
            }

            var lootBoxTypes = await Context.FetchAllLootBoxes();
            Context.LastFetchedLootBoxes = lootBoxTypes;
            if (!lootBoxTypes.Any(info => info.Amount > 0))
            {
                LaunchEmpty();
                return;
            }

            LaunchSelection(lootBoxTypes);
        }

        private void LaunchSelection(List<LootboxTypeInfo> lootBoxTypes)
        {
            Context.ActiveType = lootBoxTypes.First(info => info.Amount > 0).TypeId;
            Context.animator.SetTrigger("LaunchSelection");
        }

        private void LaunchEmpty()
        {
            Context.animator.SetTrigger("LaunchEmpty");
        }

        private async void LaunchOpening()
        {
            //If there is no opening lootbox type, we will default to the first type
            Context.ActiveType = (uint)Mathf.Max(1, await Context.OpeningLootBoxType());
            Context.animator.SetTrigger("LaunchOpeningLootboxes");
        }

        private async void LaunchCanClaimRewards()
        {
            //If there is no opening lootbox type, we will default to the first type
            Context.ActiveType = (uint)Mathf.Max(1, await Context.OpeningLootBoxType());
            Context.animator.SetTrigger("LaunchCanClaimRewards");
        }
    }
}