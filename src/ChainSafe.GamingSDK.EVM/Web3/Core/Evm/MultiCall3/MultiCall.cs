using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto;
using ChainSafe.GamingWeb3;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using BigInteger = Org.BouncyCastle.Math.BigInteger;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3
{
    public class MultiCall : ILifecycleParticipant
    {
        private readonly MultiQueryHandler handler;
        private Contract multiCallContract;

        public MultiCall(IRpcProvider provider, IChainConfig chainConfig, MultiCallConfig config, MultiQueryHandler handler)
        {
            this.handler = handler;
            if (MultiCallDefaults.DeployedNetworks.Contains(chainConfig.ChainId))
            {
                multiCallContract = new Contract(MultiCallDefaults.MultiCallAbi, MultiCallDefaults.OfficialAddress, provider);
                handler = new Nethereum.Web3.Web3(chainConfig.Rpc).Eth.GetMultiQueryHandler();
            }
            else
            {
                if (config.CustomNetworks.TryGetValue(chainConfig.ChainId, out var address))
                {
                    handler = new Nethereum.Web3.Web3(chainConfig.Rpc).Eth.GetMultiQueryHandler(address);

                    multiCallContract = new Contract(MultiCallDefaults.MultiCallAbi, address, provider);
                }
            }
        }

        public async Task<IMultiCallRequest[]> MultiCallV3(IMultiCallRequest[] calls, bool? staticCall = true)
        {
            await handler.MultiCallAsync(calls.ToArray()).ConfigureAwait(false);
            return calls;

            // function aggregate3(Call3[] calldata calls) external payable returns (Result[] memory returnData);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns a MultiCall request item for getting the transaction base fee.
        /// </returns>
        public MultiCallRequest<GetBaseFeeFunction, GetBaseFeeOutputDto> GetBaseFeeCallData()
        {
            return new MultiCallRequest<GetBaseFeeFunction, GetBaseFeeOutputDto>(
                new GetBaseFeeFunction(),
                handler.ContractAddress);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the block hash for a given block.
        /// </returns>
        public MultiCallRequest<GetBlockHashFunction, GetBlockHashOutputDto> GetBlockHash(BigInteger blockNumber)
        {
            return new MultiCallRequest<GetBlockHashFunction, GetBlockHashOutputDto>(
                new GetBlockHashFunction() { BlockNumber = blockNumber },
                handler.ContractAddress);

            // function getBlockHash(uint256 blockNumber) external view returns (bytes32 blockHash);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the block number.
        /// </returns>
        public MultiCallRequest<GetBlockNumberFunction, GetBlockNumberOutputDto> GetBlockNumber()
        {
            return new MultiCallRequest<GetBlockNumberFunction, GetBlockNumberOutputDto>(
                new GetBlockNumberFunction(),
                handler.ContractAddress);

            // function getBlockNumber() external view returns (uint256 blockNumber);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the chain ID.
        /// </returns>
        public MultiCallRequest<GetChainIdFunction, GetChainIdOutputDto> GetChainId()
        {
            return new MultiCallRequest<GetChainIdFunction, GetChainIdOutputDto>(
                new GetChainIdFunction(),
                handler.ContractAddress);

            // function getChainId() external view returns (uint256 chainid);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the current coinbase.
        /// </returns>
        public MultiCallRequest<GetCurrentBlockCoinbaseFunction, GetCurrentBlockCoinbaseOutputDto> GetCurrentBlockCoinbase()
        {
            return new MultiCallRequest<GetCurrentBlockCoinbaseFunction, GetCurrentBlockCoinbaseOutputDto>(
                new GetCurrentBlockCoinbaseFunction(),
                handler.ContractAddress);

            // function getCurrentBlockCoinbase() external view returns (address coinbase);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the current block difficulty.
        /// </returns>
        public MultiCallRequest<GetCurrentBlockDifficultyFunction, GetCurrentBlockDifficultyOutputDto> GetCurrentBlockDifficulty()
        {
            return new MultiCallRequest<GetCurrentBlockDifficultyFunction, GetCurrentBlockDifficultyOutputDto>(
                new GetCurrentBlockDifficultyFunction(),
                handler.ContractAddress);

            // function getCurrentBlockDifficulty() external view returns (uint256 difficulty);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the current gas limit.
        /// </returns>
        public MultiCallRequest<GetCurrentBlockGasLimitFunction, GetCurrentBlockGasLimitOutputDto> GetCurrentBlockGasLimit()
        {
            return new MultiCallRequest<GetCurrentBlockGasLimitFunction, GetCurrentBlockGasLimitOutputDto>(
                new GetCurrentBlockGasLimitFunction(),
                handler.ContractAddress);

            // function getCurrentBlockGasLimit() external view returns (uint256 gaslimit);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Gets the current block timestamp.
        /// </returns>
        public MultiCallRequest<GetCurrentBlockTimestampFunction, GetCurrentBlockTimestampOutputDto> GetCurrentBlockTimestamp()
        {
            return new MultiCallRequest<GetCurrentBlockTimestampFunction, GetCurrentBlockTimestampOutputDto>(
                new GetCurrentBlockTimestampFunction(),
                handler.ContractAddress);

            // function getCurrentBlockTimestamp() external view returns (uint256 timestamp);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the Eth balance for a given address.
        /// </returns>
        public MultiCallRequest<GetEthBalanceFunction, GetEthBalanceOutputDto> GetEthBalance(string address)
        {
            return new MultiCallRequest<GetEthBalanceFunction, GetEthBalanceOutputDto>(
                new GetEthBalanceFunction() { Addr = address },
                handler.ContractAddress);

            // function getEthBalance(address addr) external view returns (uint256 balance);
        }

        /// <summary>
        /// Internal state data call.
        /// </summary>
        /// <returns>
        /// Returns the last block hash.
        /// </returns>
        public MultiCallRequest<GetLastBlockHashFunction, GetLastBlockHashOutputDto> GetLastBlockHash()
        {
            return new MultiCallRequest<GetLastBlockHashFunction, GetLastBlockHashOutputDto>(
                new GetLastBlockHashFunction(),
                handler.ContractAddress);

            // function getLastBlockHash() external view returns (bytes32 blockHash);
        }

        public ValueTask WillStartAsync()
        {
            return default;
        }

        public ValueTask WillStopAsync()
        {
            return default;
        }
    }
}