using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using UnityEngine;

namespace Scripts.EVM.Token
{
    // todo convert this into a service
    public class Erc721
    {
        private static string _abi = ABI.Erc721;

        /// <summary>
        /// Balance Of ERC721 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<int> BalanceOf(Web3 web3, string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(_abi, contractAddress);
            var contractData = await contract.Call(CommonMethod.BalanceOf, new object[]
            {
                account
            });
            return int.Parse(contractData[0].ToString());
        }

        /// <summary>
        /// Owner Of ERC721 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<string> OwnerOf(Web3 web3, string contractAddress, string tokenId)
        {
            var method = CommonMethod.OwnerOf;
            var contract = web3.ContractBuilder.Build(_abi, contractAddress);
            var contractData = await contract.Call(method, new object[]
            {
                tokenId
            });
            return contractData[0].ToString();
        }

        /// <summary>
        /// Owner Of Batch ERC721 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenIds"></param>
        /// <param name="multicall"></param>
        /// <returns></returns>
        public static async Task<List<string>> OwnerOfBatch(
            Web3 web3,
            string contractAddress,
            string[] tokenIds,
            string multicall = "")
        {
            var method = CommonMethod.OwnerOf;
            // build array of args
            var obj = new string[tokenIds.Length][];
            for (var i = 0; i < tokenIds.Length; i++)
                obj[i] = new[]
                {
                    tokenIds[i]
                };
            var args = JsonConvert.SerializeObject(obj);
            var response = await Remote.CSServer.Multicall(web3, web3.ChainConfig.ChainId, web3.ChainConfig.Network,
                contractAddress, _abi, method, args, multicall, web3.ChainConfig.Rpc);
            try
            {
                var responses = JsonConvert.DeserializeObject<string[]>(response);
                var owners = new List<string>();

                foreach (var t in responses)
                {
                    // clean up address
                    var address = "0x" + t.Substring(t.Length - 40);
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
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<string> URI(Web3 web3, string contractAddress, string tokenId)
        {
            const string ipfsPath = "https://ipfs.io/ipfs/";
            var contract = web3.ContractBuilder.Build(_abi, contractAddress);
            if (tokenId.StartsWith("0x"))
            {
                var convertUri = tokenId.Replace("0x", "f");
                return ipfsPath + convertUri;
            }

            var contractData = await contract.Call(CommonMethod.TokenUri, new object[]
            {
                tokenId
            });
            return contractData[0].ToString();
        }
    }
}