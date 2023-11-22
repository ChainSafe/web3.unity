using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Scripts.EVM.Remote;

namespace Scripts.EVM.Token
{
    public static class Erc721
    {
        /// <summary>
        /// Balance Of ERC721 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<int> BalanceOf(Web3 web3, string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(ABI.Erc721, contractAddress);
            var contractData = await contract.Call(EthMethod.BalanceOf, new object[]
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
            var method = EthMethod.OwnerOf;
            var contract = web3.ContractBuilder.Build(ABI.Erc721, contractAddress);
            var contractData = await contract.Call(method, parameters);
            return contractData[0].ToString();
        }

        /// <summary>
        /// Returns owners of batch
        /// </summary>
        public static async Task<List<string>> OwnerOfBatch(
            Web3 web3,
            string contractAddress,
            string[] tokenIds)
        {
            var erc721Contract = web3.ContractBuilder.Build(ABI.Erc721, contractAddress);
            List<Call3Value> calls = new List<Call3Value>();
            for (int i = 0; i < tokenIds.Length; i++)
            {
                var callData = erc721Contract.Calldata(EthMethod.OwnerOf, new object[]
                {
                    tokenIds[i]
                });
                var call3Value = new Call3Value()
                {
                    Target = Contracts.Erc721,
                    AllowFailure = true,
                    CallData = callData.HexToByteArray()
                };
                calls.Add(call3Value);
            };

            var multiCallResultResponse = await web3.MultiCall().MultiCallAsync(calls.ToArray());
            var owners = new List<string>();
            for (int i = 0; i < multiCallResultResponse.Count; i++)
            {
                if (multiCallResultResponse[i] != null && multiCallResultResponse[i].Success)
                {
                    var owner = erc721Contract.Decode(EthMethod.OwnerOf, multiCallResultResponse[i].ReturnData.ToHex());
                    owners.Add(owner[0].ToString());
                }
            }
            return owners;
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
            var contract = web3.ContractBuilder.Build(ABI.Erc721, contractAddress);
            if (tokenId.StartsWith("0x"))
            {
                var convertUri = tokenId.Replace("0x", "f");
                return ipfsPath + convertUri;
            }

            var contractData = await contract.Call(EthMethod.TokenUri, new object[]
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
            const string method = EthMethod.SafeMint;
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