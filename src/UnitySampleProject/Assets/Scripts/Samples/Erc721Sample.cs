using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Newtonsoft.Json;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

namespace Web3Unity.Scripts.Prefabs
{
    public class Erc721Sample
    {
        private readonly Web3 web3;

        public Erc721Sample(Web3 web3)
        {
            this.web3 = web3;
        }

        public async Task<int> BalanceOf(string contractAddress, string account)
        {
            return await ERC721.BalanceOf(web3, contractAddress, account);
        }

        public async Task<string> OwnerOf(string contractAddress, string tokenId)
        {
            return await ERC721.OwnerOf(web3, contractAddress, tokenId);
        }

        public async Task<List<string>> OwnerOfBatch(string contractAddress, string[] tokenIds, string multicall = "")
        {
            return await ERC721.OwnerOfBatch(web3, contractAddress, tokenIds, multicall);
        }

        public async Task<string> Uri(string contractAddress, string tokenId)
        {
            return await ERC721.URI(web3, contractAddress, tokenId);
        }

        public async Task<Nft[]> All(string chain, string network, string account, string contract, int take, int skip)
        {
            return await EVM.AllErc721(web3, chain, network, account, contract, take, skip);
        }
    }
}