using System.Threading.Tasks;
using System.Collections.Generic;
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

        var calls = new[]
        {
           new Call3Value()
           {
           Target = Erc20ContractAddress,
           AllowFailure = true,
           CallData = erc20BalanceOfCalldata.HexToByteArray(),
           },
        };
        
        //Calldata to MultiCallRequest
        // List<object> -> [0] => List<List<ParameterOutput>>
        var temp = await web3.MultiCall().MultiCallAsync(calls);

        // var decoded = erc20Contract.Decode(temp);
        List<List<ParameterOutput>> callResult1 = temp[0][0];
        // Debug.Log($"Result success: {callResult1[0].Result}");
        // Debug.Log($"Balance success: {callResult1[1].Result}");
        Debug.Log(callResult1);
        // Debug.Log(callResult1[0]);
        // Debug.Log(callResult1[0][1]);
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