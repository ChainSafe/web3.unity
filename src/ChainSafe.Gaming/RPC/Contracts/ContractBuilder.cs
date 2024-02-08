using System;
using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.Evm.Contracts
{
    public class ContractBuilder : IContractBuilder
    {
        private readonly Dictionary<string, ContractData> registeredContracts;
        private readonly IRpcProvider rpcProvider;
        private readonly ISigner signer;
        private readonly ITransactionExecutor transactionExecutor;
        private readonly IAnalyticsClient analyticsClient; // Added analytics client

        public ContractBuilder(IRpcProvider rpcProvider, IAnalyticsClient analyticsClient)
            : this(new(), rpcProvider, null, null, analyticsClient)
        {
        }

        public ContractBuilder(IRpcProvider rpcProvider, ISigner signer, IAnalyticsClient analyticsClient)
            : this(new(), rpcProvider, signer, null, analyticsClient)
        {
        }

        public ContractBuilder(ContractBuilderConfig config, IRpcProvider rpcProvider, ISigner signer, IAnalyticsClient analyticsClient)
            : this(config, rpcProvider, signer, null, analyticsClient)
        {
        }

        public ContractBuilder(IRpcProvider rpcProvider, ISigner signer, ITransactionExecutor transactionExecutor, IAnalyticsClient analyticsClient)
            : this(new(), rpcProvider, signer, transactionExecutor, analyticsClient)
        {
        }

        public ContractBuilder(ContractBuilderConfig config, IRpcProvider rpcProvider, ISigner signer, ITransactionExecutor transactionExecutor, IAnalyticsClient analyticsClient)
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
        }

        public Contract Build(string name)
        {
            if (!registeredContracts.TryGetValue(name, out ContractData data))
            {
                throw new Web3Exception($"Contract with name '{name}' was not registered.");
            }

            return new Contract(data.Abi, data.Address, rpcProvider, signer, transactionExecutor, analyticsClient); // Pass analytics client to Contract
        }

        public Contract Build(string abi, string address) =>
            new Contract(abi, address, rpcProvider, signer, transactionExecutor, analyticsClient); // Pass analytics client to Contract
    }
}
