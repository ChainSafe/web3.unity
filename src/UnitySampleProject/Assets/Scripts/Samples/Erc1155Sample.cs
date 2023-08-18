using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

namespace Web3Unity.Scripts.Prefabs
{
    public class Erc1155Sample
    {
        private readonly Web3 web3;

        public Erc1155Sample(Web3 web3)
        {
            this.web3 = web3;
        }

        public async Task<BigInteger> BalanceOf(string contract, string account, string tokenId)
        {
            return await ERC1155.BalanceOf(web3, contract, account, tokenId);
        }

        public async Task<List<BigInteger>> BalanceOfBatch(string contract, string[] accounts, string[] tokenIds)
        {
            return await ERC1155.BalanceOfBatch(web3, contract, accounts, tokenIds);
        }

        public async Task<string> Uri(string contract, string tokenId)
        {
            return await ERC1155.URI(web3, contract, tokenId);
        }

        public async Task<BigInteger> ImportNftTexture()
        {
            throw new NotImplementedException();
        }

        public async Task<Nft[]> All(string chain, string network, string account, string contract, int take, int skip)
        {
            return await EVM.AllErc1155(web3, chain, network, account, contract, take, skip);
        }
    }
}