using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Newtonsoft.Json;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Contracts;

namespace Web3Unity.Scripts.Library.ETHEREUEM.EIP
{
    public class ERC721
    {
        private static string abi = ABI.ERC_721;

        /// <summary>
        /// Balance Of ERC721 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<int> BalanceOf(Web3 web3, string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var contractData = await contract.Call(EthMethod.BalanceOf, new object[]
            {
                account
            });
            return int.Parse(contractData[0].ToString());
        }

        /// <summary>
        /// Owner Of ERC721 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<string> OwnerOf(Web3 web3, string contractAddress, string tokenId)
        {
            var method = EthMethod.OwnerOf;
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            var contractData = await contract.Call(method, new object[]
            {
                tokenId
            });
            return contractData[0].ToString();
        }

        /// <summary>
        /// Owner Of Batch ERC721 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenIds"></param>
        /// <param name="_multicall"></param>
        /// <returns></returns>
        public static async Task<List<string>> OwnerOfBatch(
            Web3 web3,
            string contractAddress,
            string[] tokenIds,
            string _multicall = "")
        {
            var method = EthMethod.OwnerOf;
            // build array of args
            var obj = new string[tokenIds.Length][];
            for (var i = 0; i < tokenIds.Length; i++)
                obj[i] = new string[]
                {
                    tokenIds[i]
                };
            var args = JsonConvert.SerializeObject(obj);
            var response = await EVM.Multicall(web3, web3.ChainConfig.ChainId, web3.ChainConfig.Network,
                contractAddress, abi, method, args, _multicall, web3.ChainConfig.Rpc);
            try
            {
                var responses = JsonConvert.DeserializeObject<string[]>(response);
                var owners = new List<string>();

                for (var i = 0; i < responses.Length; i++)
                {
                    // clean up address
                    var address = "0x" + responses[i].Substring(responses[i].Length - 40);
                    owners.Add(address);
                }

                return owners;
            }
            catch
            {
                Debug.LogError(response);
                throw;
            }
        }

        /// <summary>
        /// Token URI Of ERC721 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <param name="_rpc"></param>
        /// <returns></returns>
        public static async Task<string> URI(Web3 web3, string contractAddress, string tokenId)
        {
            const string ipfsPath = "https://ipfs.io/ipfs/";
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            if (tokenId.StartsWith("0x"))
            {
                var convertURI = tokenId.Replace("0x", "f");
                return ipfsPath + convertURI;
            }

            var contractData = await contract.Call(EthMethod.TokenUri, new object[]
            {
                tokenId
            });
            return contractData[0].ToString();
        }
    }
}