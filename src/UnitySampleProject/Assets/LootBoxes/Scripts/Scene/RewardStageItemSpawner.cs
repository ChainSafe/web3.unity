using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using LootBoxes.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Scene
{
    public class RewardStageItemSpawner : MonoBehaviour
    {
        public StageItem StubStageItemPrefab;
        
        public List<StageItem> Spawn(LootboxRewards rewards)
        {
            // todo spawn real stage items
            var totalCount = rewards.Erc20Rewards.Count 
                             + rewards.Erc721Rewards.Count 
                             + rewards.Erc1155Rewards.Count 
                             + rewards.Erc1155NftRewards.Count;
            return Enumerable.Range(0, totalCount)
                .Select(_ => Instantiate(StubStageItemPrefab))
                .ToList();
        }
    }
}