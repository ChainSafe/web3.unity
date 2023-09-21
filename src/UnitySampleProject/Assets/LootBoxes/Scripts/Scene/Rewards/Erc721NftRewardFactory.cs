﻿using System.Threading.Tasks;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using LootBoxes.Scene.StageItems;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;

namespace LootBoxes.Scene
{
    public class Erc721NftRewardFactory : MonoBehaviour
    {
        public StageItem NftRewardItemPrefab;
        
        private IContractBuilder contractBuilder;

        public void Configure(IContractBuilder contractBuilder)
        {
            this.contractBuilder = contractBuilder;
        }

        private void OnValidate()
        {
            if (NftRewardItemPrefab && NftRewardItemPrefab.Reward is not NftReward)
            {
                Debug.LogError($"{nameof(NftRewardItemPrefab.Reward)} is not {nameof(NftReward)}");
                NftRewardItemPrefab = null;
            }
        }

        public async Task<StageItem> Create(Erc721Reward data)
        {
            var item = Instantiate(NftRewardItemPrefab);
            var reward = (NftReward)item.Reward;
            var contract = contractBuilder.Build(ABI.ERC_721, data.ContractAddress);
            var uri = (await contract.Call("tokenURI", new object[] { data.TokenId.ToString() }))[0].ToString();
            var texture = DownloadImage();
            reward.ImageRenderer.material.mainTexture = texture;
            
            return item;
        }

        private Texture DownloadImage()
        {
            throw new System.NotImplementedException();
        }
    }
}