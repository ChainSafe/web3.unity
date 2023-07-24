using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Newtonsoft.Json;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;

namespace Web3Unity.Scripts.Library.ETHEREUEM.EIP
{
    public class ERC1155
    {
        private static readonly string Abi = ABI.ERC_1155;

        /// <summary>
        /// Balance of ERC1155 Token
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="account"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account, string tokenId)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(EthMethod.BalanceOf, new object[]
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
        public static async Task<List<BigInteger>> BalanceOfBatch(Web3 web3, string contractAddress, string[] accounts, string[] tokenIds)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(EthMethod.BalanceOfBatch, new object[]
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
        public static async Task<string> URI(Web3 web3, string contractAddress, string tokenId)
        {
            const string ipfsPath = "https://ipfs.io/ipfs/";
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            if (tokenId.StartsWith("0x"))
            {
                string convertURI = tokenId.Replace("0x", "f");
                return ipfsPath + convertURI;
            }
            var contractData = await contract.Call(EthMethod.Uri, new object[]
            {
                tokenId
            });
            return contractData.ToString();
        }

    }
}
