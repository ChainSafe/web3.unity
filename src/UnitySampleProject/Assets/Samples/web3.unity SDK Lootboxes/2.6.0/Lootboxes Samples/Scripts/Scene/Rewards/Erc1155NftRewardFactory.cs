using System;
using System.Threading.Tasks;
using LootBoxes.Chainlink.Scene.StageItems;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.Networking;
using ChainSafe.Gaming.Ipfs;

namespace LootBoxes.Chainlink.Scene
{
    public class Erc1155NftRewardFactory : MonoBehaviour
    {
        public StageItem NftRewardItemPrefab;

        private IContractBuilder contractBuilder;
        private Erc1155MetaDataReader metaDataReader;

        public void Configure(IContractBuilder contractBuilder, Erc1155MetaDataReader erc1155MetaDataReader)
        {
            metaDataReader = erc1155MetaDataReader;
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

        public async Task<StageItem> Create(Erc1155NftReward data)
        {
            var item = Instantiate(NftRewardItemPrefab);
            var reward = (NftReward)item.Reward;
            var contract = contractBuilder.Build(ABI.Erc1155, data.ContractAddress);
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

            var image = await DownloadImage(metadata.Image);
            reward.ImageRenderer.material.mainTexture = image;

            return item;
        }

        private static async Task<Texture> DownloadImage(string imageUri)
        {
            var request = UnityWebRequestTexture.GetTexture(IpfsHelper.RollupIpfsUri(imageUri));

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new LootBoxSceneException($"WebRequest Error: {request.error}");
            }

            var texture = DownloadHandlerTexture.GetContent(request);

            return texture;
        }



    }


}