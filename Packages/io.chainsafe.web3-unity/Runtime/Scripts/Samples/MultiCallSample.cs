using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using UnityEngine;

public class MultiCallSample
{
    private const string Erc20ContractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    private const string Erc20Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    private readonly Web3 web3;

    public MultiCallSample(Web3 web3)
    {
        this.web3 = web3;
    }

    public async Task ErcSamples()
    {
        var erc20Contract = web3.ContractBuilder.Build(ABI.ERC_20, Erc20ContractAddress);
        var erc20BalanceOfCalldata = erc20Contract.Calldata(CommonMethod.BalanceOf, new object[]
        {
            Erc20Account
        });
        
        var erc20TotalSupplyCalldata = erc20Contract.Calldata(CommonMethod.TotalSupply, new object[]
        {
        });

        var calls = new[]
        {
           new Call3Value()
           {
           Target = Erc20ContractAddress,
           AllowFailure = true,
           CallData = erc20BalanceOfCalldata.HexToByteArray(),
           },
           new Call3Value()
           {
               Target = Erc20ContractAddress,
               AllowFailure = true,
               CallData = erc20TotalSupplyCalldata.HexToByteArray(),
           }
        };
        
        var multicallResultResponse = await web3.MultiCall().MultiCallAsync(calls);

        Debug.Log(multicallResultResponse);

        if (multicallResultResponse[0] != null && multicallResultResponse[0].Success)
        {
            var decodedBalanceOf = erc20Contract.Decode(CommonMethod.BalanceOf, multicallResultResponse[0].ReturnData.ToHex());
            Debug.Log($"decodedBalanceOf {((BigInteger)decodedBalanceOf[0]).ToString()}");
        }
        
        if (multicallResultResponse[1] != null && multicallResultResponse[1].Success)
        {
            var decodedTotalSupply = erc20Contract.Decode(CommonMethod.TotalSupply, multicallResultResponse[1].ReturnData.ToHex());
            Debug.Log($"decodedTotalSupply {((BigInteger)decodedTotalSupply[0]).ToString()}");
        }

    }

    private static class CommonMethod
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