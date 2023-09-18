using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;

namespace Scripts.EVM.Token
{
    // todo convert this into a service
    public class Erc1155
    {
        private static readonly string Abi = ABI.Erc1155;

        /// <summary>
        /// Balance of ERC1155 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account, string tokenId)
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
        public static async Task<string> URI(Web3 web3, string contractAddress, string tokenId)
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

    }
}