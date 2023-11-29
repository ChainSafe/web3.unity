using System;
using System.Numerics;
using System.Threading.Tasks;
using MetaMask.Contracts;
using UnityEngine;
using evm.net.Models;

namespace MetaMask.Unity.Contracts
{
    [CreateAssetMenu(menuName = "MetaMask/Contract Templates/ERC20")]
    public class ScriptableERC20 : ScriptableContract<ERC20>, ERC20
    {
        public Task<ERC20> DeployNew(String name_, String symbol_)
        {
            return CurrentContract.DeployNew(name_, symbol_);
        }

        public Task<BigInteger> BalanceOf(EvmAddress address)
        {
            return CurrentContract.BalanceOf(address);
        }

        public Task<Transaction> IncreaseAllowance(EvmAddress spender, BigInteger addedValue)
        {
            return CurrentContract.IncreaseAllowance(spender, addedValue);
        }

        public Task<string> Name()
        {
            return CurrentContract.Name();
        }

        public Task<string> Symbol()
        {
            return CurrentContract.Symbol();
        }

        public Task<BigInteger> Decimals()
        {
            return CurrentContract.Decimals();
        }

        public Task<Transaction> DecreaseAllowance(EvmAddress spender, BigInteger subtractedValue)
        {
            return CurrentContract.DecreaseAllowance(spender, subtractedValue);
        }

        public Task<Transaction> Approve(EvmAddress spender, BigInteger value)
        {
            return CurrentContract.Approve(spender, value);
        }

        public Task<BigInteger> TotalSupply()
        {
            return CurrentContract.TotalSupply();
        }

        public Task<Transaction> TransferFrom(EvmAddress from, EvmAddress to, BigInteger value)
        {
            return CurrentContract.TransferFrom(from, to, value);
        }

        public Task<Transaction> Transfer(EvmAddress to, BigInteger value)
        {
            return CurrentContract.Transfer(to, value);
        }

        public Task<BigInteger> Allowance(EvmAddress owner, EvmAddress spender)
        {
            return CurrentContract.Allowance(owner, spender);
        }

        public string Address
        {
            get => CurrentContract.Address;
        }
    }
}