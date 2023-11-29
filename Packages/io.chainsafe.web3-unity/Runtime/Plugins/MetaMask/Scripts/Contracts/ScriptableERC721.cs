using System;
using System.Numerics;
using System.Threading.Tasks;
using MetaMask.Contracts;
using UnityEngine;
using evm.net.Models;

namespace MetaMask.Unity.Contracts
{
    [CreateAssetMenu(menuName = "MetaMask/Contract Templates/ERC721")]
    public class ScriptableERC721 : ScriptableContract<ERC721PresetMinterPauserAutoId>, ERC721PresetMinterPauserAutoId
    {
        public string Address => CurrentContract.Address;

        public Task<ERC721PresetMinterPauserAutoId> DeployNew(string name, string symbol, string baseTokenURI)
        {
            return CurrentContract.DeployNew(name, symbol, baseTokenURI);
        }
        
        public Task<ERC721> DeployNew(string name_, string symbol_)
        {
            return CurrentContract.DeployNew(name_, symbol_);
        }

        public Task<HexString> DEFAULT_ADMIN_ROLE()
        {
            return CurrentContract.DEFAULT_ADMIN_ROLE();
        }

        public Task<HexString> MINTER_ROLE()
        {
            return CurrentContract.MINTER_ROLE();
        }

        public Task<HexString> PAUSER_ROLE()
        {
            return CurrentContract.PAUSER_ROLE();
        }

        public Task<Transaction> Approve(EvmAddress to, BigInteger tokenId)
        {
            return CurrentContract.Approve(to, tokenId);
        }

        public Task<BigInteger> BalanceOf(EvmAddress owner)
        {
            return CurrentContract.BalanceOf(owner);
        }

        public Task<Transaction> Burn(BigInteger tokenId)
        {
            return CurrentContract.Burn(tokenId);
        }

        public Task<EvmAddress> GetApproved(BigInteger tokenId)
        {
            return CurrentContract.GetApproved(tokenId);
        }

        public Task<HexString> GetRoleAdmin(HexString role)
        {
            return CurrentContract.GetRoleAdmin(role);
        }

        public Task<EvmAddress> GetRoleMember(HexString role, BigInteger index)
        {
            return CurrentContract.GetRoleMember(role, index);
        }

        public Task<BigInteger> GetRoleMemberCount(HexString role)
        {
            return CurrentContract.GetRoleMemberCount(role);
        }

        public Task<Transaction> GrantRole(HexString role, EvmAddress account)
        {
            return CurrentContract.GrantRole(role, account);
        }

        public Task<bool> HasRole(HexString role, EvmAddress account)
        {
            return CurrentContract.HasRole(role, account);
        }

        public Task<bool> IsApprovedForAll(EvmAddress owner, EvmAddress @operator)
        {
            return CurrentContract.IsApprovedForAll(owner, @operator);
        }

        public Task<Transaction> Mint(EvmAddress to)
        {
            return CurrentContract.Mint(to);
        }

        public Task<string> Name()
        {
            return CurrentContract.Name();
        }

        public Task<EvmAddress> OwnerOf(BigInteger tokenId)
        {
            return CurrentContract.OwnerOf(tokenId);
        }

        public Task<Transaction> Pause()
        {
            return CurrentContract.Pause();
        }

        public Task<bool> Paused()
        {
            return CurrentContract.Paused();
        }

        public Task<Transaction> RenounceRole(HexString role, EvmAddress account)
        {
            return CurrentContract.RenounceRole(role, account);
        }

        public Task<Transaction> RevokeRole(HexString role, EvmAddress account)
        {
            return CurrentContract.RevokeRole(role, account);
        }

        public Task<Transaction> SafeTransferFrom(EvmAddress from, EvmAddress to, BigInteger tokenId)
        {
            return CurrentContract.SafeTransferFrom(from, to, tokenId);
        }

        public Task<Transaction> SafeTransferFrom(EvmAddress from, EvmAddress to, BigInteger tokenId, byte[] data)
        {
            return CurrentContract.SafeTransferFrom(from, to, tokenId, data);
        }

        public Task<Transaction> SetApprovalForAll(EvmAddress @operator, bool approved)
        {
            return CurrentContract.SetApprovalForAll(@operator, approved);
        }

        public Task<bool> SupportsInterface(byte[] interfaceId)
        {
            return CurrentContract.SupportsInterface(interfaceId);
        }

        public Task<string> Symbol()
        {
            return CurrentContract.Symbol();
        }

        public Task<BigInteger> TokenByIndex(BigInteger index)
        {
            return CurrentContract.TokenByIndex(index);
        }

        public Task<BigInteger> TokenOfOwnerByIndex(EvmAddress owner, BigInteger index)
        {
            return CurrentContract.TokenOfOwnerByIndex(owner, index);
        }

        public Task<string> TokenURI(BigInteger tokenId)
        {
            return CurrentContract.TokenURI(tokenId);
        }

        public Task<BigInteger> TotalSupply()
        {
            return CurrentContract.TotalSupply();
        }

        public Task<Transaction> TransferFrom(EvmAddress from, EvmAddress to, BigInteger tokenId)
        {
            return CurrentContract.TransferFrom(from, to, tokenId);
        }

        public Task<Transaction> Unpause()
        {
            return CurrentContract.Unpause();
        }
    }
}