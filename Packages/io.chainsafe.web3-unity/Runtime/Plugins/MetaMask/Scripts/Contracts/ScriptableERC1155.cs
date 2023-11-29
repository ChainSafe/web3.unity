using System;
using System.Numerics;
using System.Threading.Tasks;
using MetaMask.Contracts;
using UnityEngine;
using evm.net.Models;

namespace MetaMask.Unity.Contracts
{
    [CreateAssetMenu(menuName = "MetaMask/Contract Templates/ERC1155")]
    public class ScriptableERC1155 : ScriptableContract<ERC1155>, ERC1155
    {
        public string Address => CurrentContract.Address;
        public Task<ERC1155> DeployNew(string uri_)
        {
            return CurrentContract.DeployNew(uri_);
        }

        public Task<BigInteger> BalanceOf(EvmAddress account, BigInteger id)
        {
            return CurrentContract.BalanceOf(account, id);
        }

        public Task<BigInteger[]> BalanceOfBatch(EvmAddress[] accounts, BigInteger[] ids)
        {
            return CurrentContract.BalanceOfBatch(accounts, ids);
        }

        public Task<bool> IsApprovedForAll(EvmAddress account, EvmAddress @operator)
        {
            return CurrentContract.IsApprovedForAll(account, @operator);
        }

        public Task<Transaction> SafeBatchTransferFrom(EvmAddress from, EvmAddress to, BigInteger[] ids, BigInteger[] amounts, byte[] data)
        {
            return CurrentContract.SafeBatchTransferFrom(from, to, ids, amounts, data);
        }

        public Task<Transaction> SafeTransferFrom(EvmAddress from, EvmAddress to, BigInteger id, BigInteger amount, byte[] data)
        {
            return CurrentContract.SafeTransferFrom(from, to, id, amount, data);
        }

        public Task<Transaction> SetApprovalForAll(EvmAddress @operator, bool approved)
        {
            return CurrentContract.SetApprovalForAll(@operator, approved);
        }

        public Task<bool> SupportsInterface(byte[] interfaceId)
        {
            return CurrentContract.SupportsInterface(interfaceId);
        }

        public Task<string> Uri(BigInteger tokenId)
        {
            return CurrentContract.Uri(tokenId);
        }
    }
}