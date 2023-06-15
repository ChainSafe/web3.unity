﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Contracts
{
    public class ContractFactory : IContractFactory
    {
        private readonly Dictionary<string, ContractData> registeredContracts;
        private readonly IRpcProvider rpcProvider;
        private readonly ISigner signer;
        private readonly ITransactionExecutor transactionExecutor;

        public ContractFactory(IRpcProvider rpcProvider, ISigner signer, ITransactionExecutor transactionExecutor)
        {
            registeredContracts = new();
            this.rpcProvider = rpcProvider;
            this.signer = signer;
            this.transactionExecutor = transactionExecutor;
        }

        public ContractFactory(ContractFactoryConfig config, IRpcProvider rpcProvider, ISigner signer, ITransactionExecutor transactionExecutor)
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
        }

        public Contract Build(string name)
        {
            if (!registeredContracts.TryGetValue(name, out ContractData data))
            {
                throw new Web3Exception($"Contract with name '{name}' was not registered.");
            }

            return new(data.Abi, data.Address, rpcProvider, signer, transactionExecutor);
        }

        public Contract Build(string abi, string address) =>
            new(abi, address, rpcProvider, signer, transactionExecutor);
    }
}
