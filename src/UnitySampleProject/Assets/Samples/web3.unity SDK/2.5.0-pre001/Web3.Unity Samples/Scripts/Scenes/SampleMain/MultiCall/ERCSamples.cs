using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.MultiCall.Dto;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

public class MultiCallErcSamples
{
    public string erc20ContractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    public string erc20Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    private readonly Web3 _web3;

    public MultiCallErcSamples(Web3 web3)
    {
        _web3 = web3;
    }

    // public async Task<IMultiCallRequest[]> ErcSamples()
    // {
    //     var erc20Contract = _web3.ContractBuilder.Build(ABI.ERC_20, erc20ContractAddress);
    //     var erc20BalanceOfCalldata = erc20Contract.Calldata(CommonMethod.BalanceOf, new object[]
    //     {
    //         erc20Account
    //     });
    //     
    //     //Calldata to MultiCallREquest
    //     // return await _web3.MultiCall().MultiCallV3(new IMultiCallRequest[]
    //     // {
    //     //     erc20BalanceOfCalldata
    //     // });
    // }
    
    static class CommonMethod
    {
        public const string BalanceOf = "balanceOf";
        public const string Name = "name";
        public const string Symbol = "symbol";
        public const string Decimals = "decimals";
        public const string TotalSupply = "totalSupply";
        public const string OwnerOf = "ownerOf";
        public const string TokenUri = "tokenURI";
        public const string Uri = "uri";
        public const string BalanceOfBatch = "balanceOfBatch";
        public const string Transfer = "transfer";
        public const string SafeTransferFrom = "safeTransferFrom";
    }
}