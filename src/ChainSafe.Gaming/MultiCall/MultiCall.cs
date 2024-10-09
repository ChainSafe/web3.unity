using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Util;

namespace ChainSafe.Gaming.MultiCall
{
    public class MultiCall : IMultiCall, IChainSwitchHandler, ILifecycleParticipant
    {
        private const int DefaultCallsPerRequest = 3000;

        private const string ContractAbi =
            "[{\"inputs\":[{\"components\":[{\"internalType\":\"address\",\"name\":\"target\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"callData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Call[]\",\"name\":\"calls\",\"type\":\"tuple[]\"}],\"name\":\"aggregate\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"},{\"internalType\":\"bytes[]\",\"name\":\"returnData\",\"type\":\"bytes[]\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"components\":[{\"internalType\":\"address\",\"name\":\"target\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"allowFailure\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"callData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Call3[]\",\"name\":\"calls\",\"type\":\"tuple[]\"}],\"name\":\"aggregate3\",\"outputs\":[{\"components\":[{\"internalType\":\"bool\",\"name\":\"success\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"returnData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Result[]\",\"name\":\"returnData\",\"type\":\"tuple[]\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"components\":[{\"internalType\":\"address\",\"name\":\"target\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"allowFailure\",\"type\":\"bool\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"callData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Call3Value[]\",\"name\":\"calls\",\"type\":\"tuple[]\"}],\"name\":\"aggregate3Value\",\"outputs\":[{\"components\":[{\"internalType\":\"bool\",\"name\":\"success\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"returnData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Result[]\",\"name\":\"returnData\",\"type\":\"tuple[]\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"components\":[{\"internalType\":\"address\",\"name\":\"target\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"callData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Call[]\",\"name\":\"calls\",\"type\":\"tuple[]\"}],\"name\":\"blockAndAggregate\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"},{\"internalType\":\"bytes32\",\"name\":\"blockHash\",\"type\":\"bytes32\"},{\"components\":[{\"internalType\":\"bool\",\"name\":\"success\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"returnData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Result[]\",\"name\":\"returnData\",\"type\":\"tuple[]\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getBasefee\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"basefee\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"}],\"name\":\"getBlockHash\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"blockHash\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getBlockNumber\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getChainId\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"chainid\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentBlockCoinbase\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"coinbase\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentBlockDifficulty\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"difficulty\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentBlockGasLimit\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"gaslimit\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentBlockTimestamp\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"timestamp\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"addr\",\"type\":\"address\"}],\"name\":\"getEthBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"balance\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getLastBlockHash\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"blockHash\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bool\",\"name\":\"requireSuccess\",\"type\":\"bool\"},{\"components\":[{\"internalType\":\"address\",\"name\":\"target\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"callData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Call[]\",\"name\":\"calls\",\"type\":\"tuple[]\"}],\"name\":\"tryAggregate\",\"outputs\":[{\"components\":[{\"internalType\":\"bool\",\"name\":\"success\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"returnData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Result[]\",\"name\":\"returnData\",\"type\":\"tuple[]\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bool\",\"name\":\"requireSuccess\",\"type\":\"bool\"},{\"components\":[{\"internalType\":\"address\",\"name\":\"target\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"callData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Call[]\",\"name\":\"calls\",\"type\":\"tuple[]\"}],\"name\":\"tryBlockAndAggregate\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"},{\"internalType\":\"bytes32\",\"name\":\"blockHash\",\"type\":\"bytes32\"},{\"components\":[{\"internalType\":\"bool\",\"name\":\"success\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"returnData\",\"type\":\"bytes\"}],\"internalType\":\"struct Multicall3.Result[]\",\"name\":\"returnData\",\"type\":\"tuple[]\"}],\"stateMutability\":\"payable\",\"type\":\"function\"}]";

        private const string DefaultDeploymentAddress = "0xcA11bde05977b3631167028862bE2a173976CA11";

        private readonly IContractBuilder builder;
        private readonly IChainConfig chainConfig;
        private readonly MultiCallConfig config;
        private readonly ILogWriter logWriter;

        private Contract multiCallContract;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCall"/> class.
        /// </summary>
        /// <param name="builder">An implementation of the contract builder.</param>
        /// <param name="chainConfig">The blockchain configuration for the associated chain.</param>
        /// <param name="config">The configuration settings for MultiCall.</param>
        public MultiCall(IContractBuilder builder, IChainConfig chainConfig, MultiCallConfig config, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.config = config;
            this.chainConfig = chainConfig;
            this.builder = builder;
        }

        public ValueTask WillStartAsync()
        {
            BuildMultiCallContract();
            return default;
        }

        public ValueTask WillStopAsync() => default;

        public Task HandleChainSwitching()
        {
            // build a new instance of the contract client
            BuildMultiCallContract();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes a batch of Ethereum function calls using the MultiCall contract asynchronously.
        /// This utilizes MultiCall V3's implementation and dynamically checks whether to use value calls or not.
        /// </summary>
        /// <param name="multiCalls">An array of function calls to execute in a batch.</param>
        /// <param name="pageSize">The maximum number of calls per batch request.</param>
        /// <returns>A list of results from executing the batched calls.</returns>
        public async Task<List<Result>> MultiCallAsync(Call3Value[] multiCalls, int pageSize = DefaultCallsPerRequest)
        {
            AssertContractAvailable();

            if (multiCalls.Any(x => x.Value > 0))
            {
                var results = new List<object[]>();
                foreach (var page in multiCalls.Batch(pageSize))
                {
                    var contractCalls = new List<Call3Value>();
                    foreach (var multiCall in page)
                    {
                        contractCalls.Add(new Call3Value { CallData = multiCall.CallData, Target = multiCall.Target, AllowFailure = multiCall.AllowFailure, Value = multiCall.Value });
                    }

                    if (contractCalls.Count > 0)
                    {
                        var callParams = new object[]
                        {
                             contractCalls,
                        };
                        var callResults = await multiCallContract.Call(MultiCallCommonMethods.Aggregate3Value, callParams);

                        results.Add(callResults);
                    }
                }

                return ExtractResults(results);
            }
            else
            {
                var results = new List<object[]>();
                foreach (var page in multiCalls.Batch(pageSize))
                {
                    var contractCalls = new List<Call3>();
                    foreach (var multiCall in page)
                    {
                        contractCalls.Add(new Call3 { CallData = multiCall.CallData, Target = multiCall.Target, AllowFailure = multiCall.AllowFailure });
                    }

                    var aggregateFunction = new Aggregate3Function();
                    aggregateFunction.Calls = contractCalls;
                    var callParams = new object[]
                    {
                         contractCalls,
                    };
                    var callResults = await multiCallContract.Call(MultiCallCommonMethods.Aggregate3, callParams);
                    results.Add(callResults);
                }

                return ExtractResults(results);
            }
        }

        private void BuildMultiCallContract()
        {
            multiCallContract = null;

            try
            {
                if (MultiCallDefaults.DeployedNetworks.Contains(chainConfig.ChainId))
                {
                    multiCallContract = builder.Build(ContractAbi, DefaultDeploymentAddress);
                    return;
                }

                if (config.CustomNetworks.TryGetValue(chainConfig.ChainId, out var address))
                {
                    multiCallContract = builder.Build(ContractAbi, address);
                }
            }
            catch (Exception e)
            {
                throw new Web3Exception("Error occured while building Multicall contract client.", e);
            }

            switch (config.UnavailableBehaviour)
            {
                case MultiCallConfig.UnavailableBehaviourType.Throw:
                    throw new Web3Exception(
                        "Couldn't build Multicall contract. " +
                        $"No configuration for chain id \"{chainConfig.ChainId}\" were found.");
                case MultiCallConfig.UnavailableBehaviourType.DisableAndLog:
                    logWriter.Log("Couldn't build Multicall contract. " +
                                  $"No configuration for chain id \"{chainConfig.ChainId}\" were found.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Extracts and formats the results of Multicall function calls into a list of <see cref="Result"/> objects.
        /// </summary>
        /// <param name="results">The response from calling the Multicall function.</param>
        /// <returns>A list of <see cref="Result"/> objects with success and return data information.</returns>
        private List<Result> ExtractResults(IReadOnlyList<object[]> results)
        {
            var extracted = results[0][0] as List<List<ParameterOutput>>;
            var parsed = new List<Result>();

            if (extracted != null)
            {
                parsed.AddRange(from callResult in extracted where callResult[0] != null && callResult[1] != null select new Result { Success = callResult[0].Result as bool? ?? false, ReturnData = callResult[1].Result as byte[], });
            }

            return parsed;
        }

        private void AssertContractAvailable()
        {
            if (multiCallContract is null)
            {
                throw new Web3Exception("Can't use MultiCall. No contract for the active chain is available.");
            }
        }
    }
}