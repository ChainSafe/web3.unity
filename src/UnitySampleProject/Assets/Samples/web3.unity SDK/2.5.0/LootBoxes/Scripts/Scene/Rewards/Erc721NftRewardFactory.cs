using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.UnityPackage;
using LootBoxes.Scene.StageItems;
using UnityEngine;

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
            Debug.Log(uri);

            try
            {
                var texture = DownloadImage();
                reward.ImageRenderer.material.mainTexture = texture;
            }
            catch (NotImplementedException)
            {
                Debug.LogError("Image loading for ERC721 is not implemented yet.");
            }

            return item;
        }

        private Texture DownloadImage()
        {
            throw new System.NotImplementedException();
        }
    }
}