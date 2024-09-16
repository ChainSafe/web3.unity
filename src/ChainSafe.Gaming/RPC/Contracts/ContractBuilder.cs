﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public class ContractBuilder : IContractBuilder, ILifecycleParticipant
    {
        private readonly Dictionary<string, ContractData> registeredContracts;
        private readonly IRpcProvider rpcProvider;
        private readonly ISigner signer;
        private readonly ITransactionExecutor transactionExecutor;
        private readonly IAnalyticsClient analyticsClient; // Added analytics client
        private readonly IChainConfig chainConfig;
        private readonly ILogWriter logWriter;

        public ContractBuilder(IRpcProvider rpcProvider, IAnalyticsClient analyticsClient, ILogWriter logWriter, IChainConfig chainConfig)
            : this(new(), rpcProvider, null, null, analyticsClient, logWriter, chainConfig)
        {
        }

        public ContractBuilder(IRpcProvider rpcProvider, ISigner signer, IAnalyticsClient analyticsClient, ILogWriter logWriter, IChainConfig chainConfig)
            : this(new(), rpcProvider, signer, null, analyticsClient, logWriter, chainConfig)
        {
        }

        public ContractBuilder(ContractBuilderConfig config, IRpcProvider rpcProvider, ISigner signer, IAnalyticsClient analyticsClient, ILogWriter logWriter, IChainConfig chainConfig)
            : this(config, rpcProvider, signer, null, analyticsClient, logWriter, chainConfig)
        {
        }

        public ContractBuilder(IRpcProvider rpcProvider, ISigner signer, ITransactionExecutor transactionExecutor, IAnalyticsClient analyticsClient, ILogWriter logWriter, IChainConfig chainConfig)
            : this(new(), rpcProvider, signer, transactionExecutor, analyticsClient, logWriter, chainConfig)
        {
        }

        public ContractBuilder(ContractBuilderConfig config, IRpcProvider rpcProvider, ISigner signer, ITransactionExecutor transactionExecutor, IAnalyticsClient analyticsClient, ILogWriter logWriter, IChainConfig chainConfig)
        {
            try
            {
                registeredContracts = config.RegisteredContracts.ToDictionary(c => c.Name);
            }
            catch (ArgumentException)
            {
                throw new Web3Exception("Duplicate contract name encountered. Configured contracts must have unique names.");
            }

            this.rpcProvider = rpcProvider;
            this.signer = signer;
            this.transactionExecutor = transactionExecutor;
            this.analyticsClient = analyticsClient; // Initialize analytics client
            this.chainConfig = chainConfig;
            this.logWriter = logWriter;
            BasicContracts = new Dictionary<string, Contract>();
            CustomContracts = new Dictionary<string, ICustomContract>();
        }

        private Dictionary<string, Contract> BasicContracts { get; }

        private Dictionary<string, ICustomContract> CustomContracts { get; }

        public Contract Build(string name)
        {
            if (!registeredContracts.TryGetValue(name, out ContractData data))
            {
                throw new Web3Exception($"Contract with name '{name}' was not registered.");
            }

            return new Contract(data.Abi, data.Address, rpcProvider, signer, transactionExecutor, analyticsClient); // Pass analytics client to Contract
        }

        public Contract Build(string abi, string address)
        {
            if (BasicContracts.TryGetValue(address, out var value))
            {
                return value;
            }

            var contract = new Contract(abi, address, rpcProvider, signer, transactionExecutor, analyticsClient); // Pass analytics client to Contract
            BasicContracts.Add(address, contract);
            return contract;
        }

        public async Task<T> Build<T>(string address)
            where T : ICustomContract, new()
        {
            if (CustomContracts.TryGetValue(address, out var value))
            {
                await value.InitAsync();
                return (T)value;
            }

            var contract = new T
            {
                ContractAddress = address,
            };

            contract.OriginalContract = Build(contract.ABI, contract.ContractAddress);
            contract.WebSocketUrl = chainConfig.Ws;

            CustomContracts.Add(contract.ContractAddress, contract);

            await contract.InitAsync();

            return contract;
        }

        public ValueTask WillStartAsync()
        {
            return default;
        }

        public async ValueTask WillStopAsync()
        {
            foreach (var contractsValue in CustomContracts.Values)
            {
                await contractsValue.DisposeAsync();
            }
        }
    }
}
