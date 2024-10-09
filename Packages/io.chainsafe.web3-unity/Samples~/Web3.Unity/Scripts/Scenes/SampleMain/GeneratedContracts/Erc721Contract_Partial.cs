using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using JetBrains.Annotations;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public partial class Erc721Contract
    {
        [Pure]
        public async Task<List<OwnerOfBatchModel>> GetOwnerOfBatch(string[] tokenIds)
        {
            var multiCall = (IMultiCall)Web3Unity.Web3.ServiceProvider.GetService(typeof(IMultiCall));
            if (multiCall == null)
                throw new Web3Exception(
                    $"Can't execute {nameof(GetOwnerOfBatch)}. No MultiCall component was provided during construction.");

            var calls = tokenIds
                .Select(BuildCall)
                .ToList();

            var multiCallResponse = await multiCall.MultiCallAsync(calls.ToArray());

            return multiCallResponse
                .Select(BuildResult)
                .ToList();

            Call3Value BuildCall(string tokenId)
            {
                object param = tokenId.StartsWith("0x") ? tokenId : BigInteger.Parse(tokenId);
                var callData = OriginalContract.Calldata(EthMethods.OwnerOf, new[] { param });
                return new Call3Value
                { Target = OriginalContract.Address, AllowFailure = true, CallData = callData.HexToByteArray() };
            }

            OwnerOfBatchModel BuildResult(Result result, int index)
            {
                if (result is not { Success: true }) return new OwnerOfBatchModel { Failure = true };

                var owner = OriginalContract.Decode(EthMethods.OwnerOf, result.ReturnData.ToHex());
                return new OwnerOfBatchModel { TokenId = tokenIds[index], Owner = owner[0].ToString() };
            }
        }
    }
}
