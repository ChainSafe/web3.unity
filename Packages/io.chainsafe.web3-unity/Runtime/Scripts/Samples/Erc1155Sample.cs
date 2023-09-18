using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Scripts.EVM.Remote;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Prefabs
{
    public class NftMetaDataSample
    {
        public string image { get; set; }
    }

    public class Erc1155Sample
    {
        private readonly Web3 web3;

        public Erc1155Sample(Web3 web3)
        {
            this.web3 = web3;
        }

        public async Task<BigInteger> BalanceOf(string contract, string account, string tokenId)
        {
            return await Erc1155.BalanceOf(web3, contract, account, tokenId);
        }

        public async Task<List<BigInteger>> BalanceOfBatch(string contract, string[] accounts, string[] tokenIds)
        {
            return await Erc1155.BalanceOfBatch(web3, contract, accounts, tokenIds);
        }

        public async Task<string> Uri(string contract, string tokenId)
        {
            return await Erc1155.URI(web3, contract, tokenId);
        }

        public async Task<TokenResponse[]> All(string chain, string network, string account, string contract, int take, int skip)
        {
            return await CSServer.AllErc1155(web3, chain, network, account, contract, take, skip);
        }

        // todo move this out to a service
        public async Task<Texture2D> ImportNftTexture(string contractAddress, string tokenId)
        {
            // fetch URI from blockchain
            var uri = await Erc1155.URI(web3, contractAddress, tokenId);

            // fetch metaData from URI
            var metaData = await DownloadMetaData();

            // unpack image URI if IPFS
            var imageUri = metaData.image;
            imageUri = UnpackUriIfIpfs(imageUri);

            // download texture
            var texture = await DownloadTexture(imageUri);
            return texture;

            async Task<NftMetaDataSample> DownloadMetaData()
            {
                var request = UnityWebRequest.Get(uri);
                await request.SendWebRequest();
                AssertResponseSuccess(request);

                var json = Encoding.UTF8.GetString(request.downloadHandler.data);
                var data = JsonConvert.DeserializeObject<NftMetaDataSample>(json);
                return data;
            }

            string UnpackUriIfIpfs(string originalUri)
            {
                if (!originalUri.StartsWith("ipfs://"))
                    return originalUri;

                return originalUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            }

            async Task<Texture2D> DownloadTexture(string textureUri)
            {
                var request = UnityWebRequestTexture.GetTexture(textureUri);
                await request.SendWebRequest();
                AssertResponseSuccess(request);
                var texture = DownloadHandlerTexture.GetContent(request);
                return texture;
            }

            void AssertResponseSuccess(UnityWebRequest request)
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new WebException(request.error);
                }
            }
        }
    }
}