using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LootBoxes.Chainlink.Scene.StageItems;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class RewardStageItemSpawner : MonoBehaviour
    {
        public Erc20RewardFactory Erc20Factory;
        public Erc721NftRewardFactory Erc721NftFactory;
        public Erc1155RewardFactory Erc1155Factory;
        public Erc1155NftRewardFactory Erc1155NftFactory;

        public void Configure(IContractBuilder contractBuilder, Erc1155MetaDataReader erc1155MetaDataReader)
        {
            Erc20Factory.Configure(contractBuilder);
            Erc721NftFactory.Configure(contractBuilder);
            Erc1155Factory.Configure(contractBuilder, erc1155MetaDataReader);
            Erc1155NftFactory.Configure(contractBuilder, erc1155MetaDataReader);
        }

        public async Task<List<StageItem>> Spawn(LootboxRewards rewards)
        {
            var erc20Tasks = rewards.Erc20Rewards.Select(data => Erc20Factory.Create(data));
            var erc721Tasks = rewards.Erc721Rewards.Select(data => Erc721NftFactory.Create(data));
            var erc1155Tasks = rewards.Erc1155Rewards.Select(data => Erc1155Factory.Create(data));
            var erc1155NftTasks = rewards.Erc1155NftRewards.Select(data => Erc1155NftFactory.Create(data));
            var allItemTasks = erc20Tasks.Concat(erc721Tasks).Concat(erc1155Tasks).Concat(erc1155NftTasks);
            var items = await Task.WhenAll(allItemTasks);

            return items.ToList();
        }
    }
}