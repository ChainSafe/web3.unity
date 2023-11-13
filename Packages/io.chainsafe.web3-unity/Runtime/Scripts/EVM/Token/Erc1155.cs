using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Scripts.EVM.Remote;
using UnityEngine;
using UnityEngine.Networking;
using EthMethod = ChainSafe.Gaming.UnityPackage.EthMethod;

namespace Scripts.EVM.Token
{
    public static class Erc1155
    {
        private static readonly string Abi = ABI.Erc1155;
        
        /// <summary>
        /// Fetches all 1155 Nfts from an account
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="chain"></param>
        /// <param name="network"></param>
        /// <param name="account"></param>
        /// <param name="contract"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static async Task<TokenResponse[]> AllErc1155(Web3 web3, string chain, string network, string account, string contract, int take, int skip)
        {
            return await CSServer.AllErc1155(web3, chain, network, account, contract, take, skip);
        }

        /// <summary>
        /// Balance of ERC1155 Token (string parameter)
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account, string tokenId)
        {
            return await BalanceOf(web3, contractAddress, account, new object[]
            {
                account,
                tokenId
            });
        }

        /// <summary>
        /// Balance of ERC1155 Token (biginteger parameter)
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account, BigInteger tokenId)
        {
            return await BalanceOf(web3, contractAddress, account, new object[]
            {
                account,
                tokenId
            });
        }
        private static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account, object[] parameters)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(CommonMethod.BalanceOf, parameters);
            return BigInteger.Parse(contractData[0].ToString());
        }

        /// <summary>
        /// Balance of Batch ERC1155
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="accounts"></param>
        /// <param name="tokenIds"></param>
        /// <returns></returns>
        public static async Task<List<BigInteger>> BalanceOfBatch(Web3 web3, string contractAddress, string[] accounts, string[] tokenIds)
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
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<string> Uri(Web3 web3, string contractAddress, string tokenId)
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
        
        /// <summary>
        /// Mints ERC721 token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="abi"></param>
        /// <param name="contractAddress"></param>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<object[]> MintErc1155(Web3 web3, string abi, string contractAddress, BigInteger id, BigInteger amount)
        {
            byte[] dataObject = { };
            const string method = "mint";
            var destination = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, new object[] { destination, id, amount, dataObject });
        }
        
        /// <summary>
        /// Transfers ERC721 token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <param name="amount"></param>
        /// <param name="toAccount"></param>
        /// <returns></returns>
        public static async Task<object[]> TransferErc1155(Web3 web3, string contractAddress, BigInteger tokenId, BigInteger amount, string toAccount)
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
        /// <param name="web3"></param>
        /// <param name="contract"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<Texture2D> ImportNftTexture1155(Web3 web3, string contract, string tokenId)
        {
            // fetch uri from chain
            string uri = await Uri(web3, contract, tokenId);
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