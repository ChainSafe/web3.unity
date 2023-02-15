using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// <param name="_contract"></param>
        /// <param name="_account"></param>
        /// <returns></returns>
        public static async Task<int> BalanceOf(string _contract, string _account)
        {
            var method = ETH_METHOD.BalanceOf;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var contractData = await contract.Call(method, new object[]
            {
                _account
            });
            Debug.Log("Value: " + contractData[0]);
            return int.Parse(contractData[0].ToString());
        }

        /// <summary>
        /// Owner Of ERC721 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <param name="_tokenId"></param>
        /// <returns></returns>
        public static async Task<string> OwnerOf(string _contract, string _tokenId)
        {
            var method = ETH_METHOD.OwnerOf;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var contractData = await contract.Call(method, new object[]
            {
                _tokenId
            });
            return contractData[0].ToString();
        }

        /// <summary>
        /// Owner Of Batch ERC721 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <param name="_tokenIds"></param>
        /// <param name="_multicall"></param>
        /// <returns></returns>
        public static async Task<List<string>> OwnerOfBatch(string _contract, string[] _tokenIds,
            string _multicall = "")
        {
            var method = ETH_METHOD.OwnerOf;
            // build array of args
            var obj = new string[_tokenIds.Length][];
            for (var i = 0; i < _tokenIds.Length; i++)
                obj[i] = new string[]
                {
                    _tokenIds[i]
                };
            var args = JsonConvert.SerializeObject(obj);
            var response = await EVM.Multicall(PlayerPrefs.GetString("ChainID"), PlayerPrefs.GetString("Network"),
                _contract, abi, method, args, _multicall, PlayerPrefs.GetString("RPC"));
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
        /// <param name="_contract"></param>
        /// <param name="_tokenId"></param>
        /// <param name="_rpc"></param>
        /// <returns></returns>
        public static async Task<string> URI(string _contract, string _tokenId, string _rpc = "")
        {
            const string ipfsPath = "https://ipfs.io/ipfs/";
            var method = ETH_METHOD.TokenUri;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            if (_tokenId.StartsWith("0x"))
            {
                var convertURI = _tokenId.Replace("0x", "f");
                return ipfsPath + convertURI;
            }

            var contractData = await contract.Call(method, new object[]
            {
                _tokenId
            });
            return contractData[0].ToString();
        }
    }
}