using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;

namespace Web3Unity.Scripts.Library.ETHEREUEM.EIP
{
    public class ERC1155
    {
        private static string abi = ABI.ERC_1155;
        /// <summary>
        /// Balance of ERC1155 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <param name="_account"></param>
        /// <param name="_tokenId"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(string _contract, string _account, string _tokenId)
        {
            string method = ETH_METHOD.BalanceOf;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var contractData = await contract.Call(method, new object[]
            {
                _account,
                _tokenId
            });
            return BigInteger.Parse(contractData[0].ToString());
        }
        /// <summary>
        /// Balance of Batch ERC1155
        /// </summary>
        /// <param name="_contract"></param>
        /// <param name="_accounts"></param>
        /// <param name="_tokenIds"></param>
        /// <returns></returns>
        public static async Task<List<BigInteger>> BalanceOfBatch(string _contract, string[] _accounts, string[] _tokenIds)
        {
            string method = ETH_METHOD.BalanceOfBatch;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var contractData = await contract.Call(method, new object[]
            {
                _accounts,
                _tokenIds
            });
            try
            {
                //string[] responses = JsonConvert.DeserializeObject<string[]>(contractData.ToString());
                //Debug.Log("Responses: " + responses);
                List<BigInteger> balances = new List<BigInteger>();
                for (int i = 0; i < contractData.Length; i++)
                {
                    Debug.Log(contractData[i].GetHashCode());//balances.Add(contractData[i].GetHashCode());
                }
                return balances;
            }
            catch
            {
                Debug.LogError(contractData);
                throw;
            }
        }
        /// <summary>
        /// Token URI of ERC1155 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <param name="_tokenId"></param>
        /// <returns></returns>
        public static async Task<string> URI(string _contract, string _tokenId)
        {
            const string ipfsPath = "https://ipfs.io/ipfs/";
            string method = ETH_METHOD.Uri;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            if (_tokenId.StartsWith("0x"))
            {
                string convertURI = _tokenId.Replace("0x", "f");
                return ipfsPath + convertURI;
            }
            var contractData = await contract.Call(method, new object[]
            {
                _tokenId
            });
            return contractData.ToString();
        }

    }
}
