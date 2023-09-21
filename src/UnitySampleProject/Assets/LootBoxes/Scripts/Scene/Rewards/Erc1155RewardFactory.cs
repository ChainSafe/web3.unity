﻿using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using LootBoxes.Scene.StageItems;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;

namespace LootBoxes.Scene
{
    public class Erc1155RewardFactory : MonoBehaviour
    {
        public StageItem CoinRewardItemPrefab;
        
        private IContractBuilder contractBuilder;
        private Erc1155MetaDataReader metaDataReader;

        public void Configure(IContractBuilder contractBuilder, Erc1155MetaDataReader erc1155MetaDataReader)
        {
            metaDataReader = erc1155MetaDataReader;
            this.contractBuilder = contractBuilder;
        }

        private void OnValidate()
        {
            if (CoinRewardItemPrefab && CoinRewardItemPrefab.Reward is not CoinReward)
            {
                Debug.LogError($"{nameof(CoinRewardItemPrefab.Reward)} is not {nameof(CoinReward)}");
                CoinRewardItemPrefab = null;
            }
        }

        public async Task<StageItem> Create(Erc1155Reward data)
        {
            var item = Instantiate(CoinRewardItemPrefab);
            var reward = (CoinReward)item.Reward;
            var contract = contractBuilder.Build(ABI.ERC_1155, data.ContractAddress);
            var uri = (await contract.Call("uri", new object[] { data.TokenId }))[0].ToString();
            Erc1155MetaData metadata;
            try
            {
                metadata = await metaDataReader.Fetch(uri, data.TokenId);
            }
            catch (Exception)
            {
                Debug.LogError($"{nameof(Erc1155MetaDataReader)} couldn't fetch URI: {uri}");
                return item;
            }

            reward.Amount.text = data.Amount.ToString();
            reward.SymbolLabel.text = metadata.Name;
            
            return item;
        }
    }
}