using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient
{
    public class SygmaClient : ISygmaClient, ILifecycleParticipant
    {
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;
        private readonly IChainConfig sourceChainConfig;
        private readonly IChainConfig destinationChainConfig;
        private readonly IAnalyticsClient analyticsClient;
        private readonly IProjectConfig projectConfig;

        public SygmaClient(IHttpClient httpClient, IChainConfig sourceChainConfig, IChainConfig destinationChainConfig, ISigner signer, IContractBuilder contractBuilder, IAnalyticsClient analyticsClient, IProjectConfig projectConfig)
        {
            this.contractBuilder = contractBuilder;
            this.signer = signer;
            this.sourceChainConfig = sourceChainConfig;
            this.destinationChainConfig = destinationChainConfig;
            this.analyticsClient = analyticsClient;
            this.projectConfig = projectConfig;
        }

        public bool Initialize(Environment environment)
        {
            throw new System.NotImplementedException();
        }

        public Task<Transfer<T>> CreateTransfer<T>(string sourceAddress, uint destinationChainId, string destinationAddress, string assetResourceId, HexBigInteger amount)
            where T : TransferType
        {
            throw new System.NotImplementedException();
        }

        public Task<EvmFee> Fee<T>(Transfer<T> transfer)
            where T : TransferType
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Transaction>> BuildApprovals<T>(Transfer<T> transfer, EvmFee fee)
            where T : TransferType
        {
            throw new System.NotImplementedException();
        }

        public Task<Transaction> BuildTransferTransaction<T>(Transfer<T> transfer, EvmFee fee)
            where T : TransferType
        {
            throw new System.NotImplementedException();
        }

        public Task<TransferStatus> TransferStatusData(Environment environment, string transactionHash)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask WillStartAsync()
        {
            throw new System.NotImplementedException();
        }

        public ValueTask WillStopAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}