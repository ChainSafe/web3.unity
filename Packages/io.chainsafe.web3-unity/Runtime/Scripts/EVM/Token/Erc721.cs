using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Scripts.EVM.Remote;
using UnityEngine;

namespace Scripts.EVM.Token
{
    public static class Erc721
    {
        private static string _abi = ABI.Erc721;
        
        /// <summary>
        /// Fetches all 721 Nfts from an account
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="chain"></param>
        /// <param name="network"></param>
        /// <param name="account"></param>
        /// <param name="contract"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static async Task<TokenResponse[]> AllErc721(Web3 web3, string chain, string network, string account, string contract, int take, int skip)
        {
            return await CSServer.AllErc721(web3, chain, network, account, contract, take, skip);
        }
        
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
        /// Owner Of ERC721 Token (string parameter)
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<string> OwnerOf(Web3 web3, string contractAddress, string tokenId)
        {
            return await OwnerOf(web3, contractAddress, new object[] { tokenId, });
        }
		
		/// <summary>
        /// Owner Of ERC721 Token (biginteger parameter)
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<string> OwnerOf(Web3 web3, string contractAddress, BigInteger tokenId)
        {
            return await OwnerOf(web3, contractAddress, new object[] { tokenId, });
        }
        private static async Task<string> OwnerOf(Web3 web3, string contractAddress, object[] parameters)
        {
            var method = CommonMethod.OwnerOf;
            var contract = web3.ContractBuilder.Build(_abi, contractAddress);
            var contractData = await contract.Call(method, parameters);
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
			{
                obj[i] = new[]
                {
                    tokenIds[i]
                };
			}
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
        public static async Task<string> Uri(Web3 web3, string contractAddress, string tokenId)
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
        
        /// <summary>
        /// Mints ERC721 token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="abi"></param>
        /// <param name="contractAddress"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<object[]> MintErc721(Web3 web3, string abi, string contractAddress, string uri)
        {
            const string method = "safeMint";
            var destination = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Send(method, new object[] { destination, uri });
        }
        
        /// <summary>
        /// Transfers ERC721 token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="toAccount"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<object[]> TransferErc721(Web3 web3, string contractAddress, string toAccount, BigInteger tokenId)
        {
            var abi = ABI.Erc721;
            var method = EthMethod.SafeTransferFrom;
            var account = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(abi, contractAddress);

            var response = await contract.Send(method, new object[]
            {
                account,
                toAccount,
                tokenId.ToString()
            });

            return response;
        }
    }
}