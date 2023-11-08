using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Scripts.EVM.Remote;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Prefabs;
using EthMethod = ChainSafe.Gaming.UnityPackage.EthMethod;

namespace Scripts.EVM.Token
{
    // todo convert this into a service
    public class Erc1155
    {
        private static readonly string Abi = ABI.Erc1155;
        private Web3 web3;

        public Erc1155(Web3 web3)
        {
            this.web3 = web3 ?? throw new Web3Exception(
                "Web3 instance is null. Please ensure that the instance is properly retrieved trough the constructor");
        }
        
        public async Task<TokenResponse[]> All(string chain, string network, string account, string contract, int take, int skip)
        {
            return await CSServer.AllErc1155(web3, chain, network, account, contract, take, skip);
        }

        /// <summary>
        /// Balance of ERC1155 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public async Task<BigInteger> BalanceOf(string contractAddress, string account, string tokenId)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(CommonMethod.BalanceOf, new object[]
            {
                account,
                tokenId
            });
            return BigInteger.Parse(contractData[0].ToString());
        }

        /// <summary>
        /// Balance of Batch ERC1155
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="accounts"></param>
        /// <param name="tokenIds"></param>
        /// <returns></returns>
        public async Task<List<BigInteger>> BalanceOfBatch(string contractAddress, string[] accounts, string[] tokenIds)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(CommonMethod.BalanceOfBatch, new object[]
            {
                accounts,
                tokenIds
            });
            return contractData[0] as List<BigInteger> ?? throw new System.Exception("Unexpected result from contract call");
        }

        /// <summary>
        /// Token URI of ERC1155 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public async Task<string> Uri(string contractAddress, string tokenId)
        {
            const string ipfsPath = "https://ipfs.io/ipfs/";
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            if (tokenId.StartsWith("0x"))
            {
                string convertUri = tokenId.Replace("0x", "f");
                return ipfsPath + convertUri;
            }
            var contractData = await contract.Call(CommonMethod.Uri, new object[]
            {
                tokenId
            });
            return contractData[0].ToString();
        }

        public async Task<object[]> MintErc1155(string abi, string contractAddress, int id, int amount)
        {
            byte[] dataObject = { };
            const string method = "mint";
            var destination = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, new object[] { destination, id, amount, dataObject });
        }
        
        public async Task<object[]> TransferErc1155(string contractAddress, int tokenId, int amount, string toAccount)
        {
            var account = await web3.Signer.GetAddress();
            var abi = ABI.Erc1155;
            var method = EthMethod.SafeTransferFrom;
            byte[] dataObject = { };
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var response = await contract.Send(method, new object[]
            {
                account,
                toAccount,
                tokenId,
                amount,
                dataObject
            });
            return response;
        }
        
        /// <summary>
        /// Imports an NFT texture via Uri data
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Texture2D> ImportNftTexture1155(string contract, string tokenId)
        {
            // fetch uri from chain
            string uri = await Uri(contract, tokenId);
            // fetch json from uri
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                throw new System.Exception(webRequest.error);
            }
            // Deserialize the data into the response class
            Response data =
                JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            // parse json to get image uri
            string imageUri = data.image;
            Debug.Log("imageUri: " + imageUri);
            if (imageUri.StartsWith("ipfs://"))
            {
                imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            }
            Debug.Log("Revised URI: " + imageUri);
            // fetch image and display in game
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
            await textureRequest.SendWebRequest();
            var response = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            return response;
        }
    
        // Response class for the texture call above
        public class Response
        {
            public string image;
        }
    }
}