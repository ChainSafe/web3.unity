using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.MultiCall.Dto;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.MultiCall
{
    public interface IMultiCall : ILifecycleParticipant
    {
        public Task<IMultiCallRequest[]> MultiCallV3(IMultiCallRequest[] calls, bool? staticCall = true);

        public MultiCallRequest<GetBaseFeeFunction, GetBaseFeeOutputDto> GetBaseFeeCallData();

        public MultiCallRequest<GetBlockHashFunction, GetBlockHashOutputDto> GetBlockHash(BigInteger blockNumber);

        public MultiCallRequest<GetBlockNumberFunction, GetBlockNumberOutputDto> GetBlockNumber();

        public MultiCallRequest<GetChainIdFunction, GetChainIdOutputDto> GetChainId();

        public MultiCallRequest<GetCurrentBlockCoinbaseFunction, GetCurrentBlockCoinbaseOutputDto>
            GetCurrentBlockCoinbase();

        public MultiCallRequest<GetCurrentBlockDifficultyFunction, GetCurrentBlockDifficultyOutputDto>
            GetCurrentBlockDifficulty();

        public MultiCallRequest<GetCurrentBlockGasLimitFunction, GetCurrentBlockGasLimitOutputDto>
            GetCurrentBlockGasLimit();

        public MultiCallRequest<GetCurrentBlockTimestampFunction, GetCurrentBlockTimestampOutputDto>
            GetCurrentBlockTimestamp();

        public MultiCallRequest<GetEthBalanceFunction, GetEthBalanceOutputDto> GetEthBalance(string address);

        public MultiCallRequest<GetLastBlockHashFunction, GetLastBlockHashOutputDto> GetLastBlockHash();
    }
}