using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto;
using ChainSafe.GamingWeb3;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3
{
    public class MultiCall : ILifecycleParticipant
    {
        private Contract multiCallContract;
        private readonly MultiQueryHandler handler;

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

        public async Task<TransactionResponse> MultiCallV3(IMultiCallRequest[] calls, bool? staticCall = true)
        {
            IMultiCallRequest temp = new MulticallInputOutput<>()
            {
                
            };
            await handler.MultiCallAsync(calls.ToArray()).ConfigureAwait(false);
            return calls;

            // function aggregate3(Call3[] calldata calls) external payable returns (Result[] memory returnData);
        }

        /// <summary>
        /// Internal state data call
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
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetBlockHash(uint blockNumber)
        {
            // function getBlockHash(uint256 blockNumber) external view returns (bytes32 blockHash);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetBlockNumber()
        {
            // function getBlockNumber() external view returns (uint256 blockNumber);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetChainId()
        {
            // function getChainId() external view returns (uint256 chainid);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetCurrentBlockCoinbase()
        {
            // function getCurrentBlockCoinbase() external view returns (address coinbase);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetCurrentBlockDifficulty()
        {
            // function getCurrentBlockDifficulty() external view returns (uint256 difficulty);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetCurrentBlockGasLimit()
        {
            // function getCurrentBlockGasLimit() external view returns (uint256 gaslimit);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetCurrentBlockTimestamp()
        {
            // function getCurrentBlockTimestamp() external view returns (uint256 timestamp);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetEthBalance()
        {
            // function getEthBalance(address addr) external view returns (uint256 balance);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetLastBlockHash()
        {
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