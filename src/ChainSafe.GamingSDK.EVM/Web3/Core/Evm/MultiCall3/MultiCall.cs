using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public readonly Nethereum.Web3.Web3 web3;

        public MultiCall(IRpcProvider provider, IChainConfig chainConfig, MultiCallConfig config)
        {
            if (MultiCallDefaults.DeployedNetworks.Contains(chainConfig.ChainId))
            {
                multiCallContract = new Contract(MultiCallDefaults.MultiCallAbi, MultiCallDefaults.OfficialAddress, provider);
                web3 = new Nethereum.Web3.Web3(chainConfig.Rpc);
                web3.Eth.GetMultiQueryHandler().MultiCallAsync()
            }
            else
            {
                if (config.CustomNetworks.TryGetValue(chainConfig.ChainId, out var address))
                {
                    multiCallContract = new Contract(MultiCallDefaults.MultiCallAbi, address, provider);
                }
            }
        }

        public async Task<TransactionResponse> Aggregate3(Call3[] calls, bool? staticCall = true)
        {
            var callList = new List<IMulticallInputOutput>();
            await web3.Eth.GetMultiQueryHandler().MultiCallAsync(callList.ToArray()).ConfigureAwait(false);
            // function aggregate3(Call3[] calldata calls) external payable returns (Result[] memory returnData);
        }

        public Task<TransactionResponse> Aggregate3Value(Call3[] calls, bool? staticCall)
        {
            // function aggregate3Value(Call3Value[] calldata calls)
            // external
            //     payable
            // returns (Result[] memory returnData);
        }

        public Task<TransactionResponse> Aggregate(Call[] calls, bool? staticCall)
        {
            // function aggregate(Call[] calldata calls)
            // external
            //     payable
            // returns (uint256 blockNumber, bytes[] memory returnData);
        }

        public Task<TransactionResponse> BlockAndAggregate(Call[] calls, bool? staticCall)
        {
            // function blockAndAggregate(Call[] calldata calls)
            // external
            //     payable
            // returns (uint256 blockNumber, bytes32 blockHash, Result[] memory returnData);
        }

        public Task<TransactionResponse> TryAggregate(bool requireSuccess, Call[] calls, bool? staticCall)
        {
            // function tryAggregate(bool requireSuccess, Call[] calldata calls)
            // external
            //     payable
            // returns (Result[] memory returnData);
        }

        public Task<TransactionResponse> TryBlockAndAggregate(bool requireSuccess, Call[] calls, bool? staticCall)
        {
            // function tryBlockAndAggregate(bool requireSuccess, Call[] calldata calls)
            // external
            //     payable
            // returns (uint256 blockNumber, bytes32 blockHash, Result[] memory returnData);
        }

        /// <summary>
        /// Internal state data call
        /// </summary>
        public Task<TransactionResponse> GetBasefee()
        {
            // function getBasefee() external view returns (uint256 basefee);
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