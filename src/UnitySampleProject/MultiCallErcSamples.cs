using System.Threading.Tasks;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using UnityEngine;

public class MultiCallErcSamples
{
    private const string Erc20ContractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    private const string Erc20Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    private readonly Web3 _web3;

    public MultiCallErcSamples(Web3 web3)
    {
        _web3 = web3;
    }

    public async Task ErcSamples()
    {
        var erc20Contract = _web3.ContractBuilder.Build(ABI.ERC_20, Erc20ContractAddress);
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
        var temp = await _web3.MultiCall().MultiCallAsync(calls);
        Debug.Log(temp);
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