using System;
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
using Environment = ChainSafe.Gaming.SygmaClient.Types.Environment;

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

        private Config clientConfiguration;

        public SygmaClient(IHttpClient httpClient, IChainConfig sourceChainConfig, IChainConfig destinationChainConfig, ISigner signer, IContractBuilder contractBuilder, IAnalyticsClient analyticsClient, IProjectConfig projectConfig)
        {
            this.contractBuilder = contractBuilder;
            this.signer = signer;
            this.sourceChainConfig = sourceChainConfig;
            this.destinationChainConfig = destinationChainConfig;
            this.analyticsClient = analyticsClient;
            this.projectConfig = projectConfig;

            clientConfiguration = new Config(httpClient, uint.Parse(sourceChainConfig.ChainId));
        }

        public ValueTask WillStartAsync()
        {
            throw new System.NotImplementedException();
        }

        public ValueTask WillStopAsync()
        {
            throw new System.NotImplementedException();
        }

        public bool Initialize(Environment environment)
        {
            clientConfiguration.Fetch(environment);

            return true;
        }

        public Task<Transfer<T>> CreateTransfer<T>(string sourceAddress, uint destinationChainId, string destinationAddress, string assetResourceId, HexBigInteger amount)
            where T : TransferType
        {
            throw new System.NotImplementedException();
        }

        public Task<EvmFee> Fee<T>(Transfer<T> transfer)
            where T : TransferType
        {
            var domainConfig = clientConfiguration.SourceDomainConfig();
            /*
             * const domainConfig = this.config.getSourceDomainConfig() as EthereumConfig;
               const feeRouter = FeeHandlerRouter__factory.connect(domainConfig.feeRouter, this.provider);
               const feeHandlerAddress = await feeRouter._domainResourceIDToFeeHandlerAddress(
                 transfer.to.id,
                 transfer.resource.resourceId,
               );
               const feeHandlerConfig = domainConfig.feeHandlers.find(
                 feeHandler => feeHandler.address == feeHandlerAddress,
               )!;

               switch (feeHandlerConfig.type) {
                 case FeeHandlerType.BASIC: {
                   return await calculateBasicfee({
                     basicFeeHandlerAddress: feeHandlerAddress,
                     provider: this.provider,
                     fromDomainID: transfer.from.id,
                     toDomainID: transfer.to.id,
                     resourceID: transfer.resource.resourceId,
                     sender: transfer.sender,
                   });
                 }
                 case FeeHandlerType.DYNAMIC: {
                   const fungibleTransfer = transfer as Transfer<Fungible>;
                   return await calculateDynamicFee({
                     provider: this.provider,
                     sender: transfer.sender,
                     fromDomainID: Number(transfer.from.id),
                     toDomainID: Number(transfer.to.id),
                     resourceID: transfer.resource.resourceId,
                     tokenAmount: fungibleTransfer.details.amount,
                     feeOracleBaseUrl: getFeeOracleBaseURL(this.environment),
                     feeHandlerAddress: feeHandlerAddress,
                     depositData: createERCDepositData(
                       fungibleTransfer.details.amount,
                       fungibleTransfer.details.recipient,
                       fungibleTransfer.details.parachainId,
                     ),
                   });
                 }
                 case FeeHandlerType.PERCENTAGE: {
                   const fungibleTransfer = transfer as Transfer<Fungible>;
                   return await getPercentageFee({
                     precentageFeeHandlerAddress: feeHandlerAddress,
                     provider: this.provider,
                     sender: transfer.sender,
                     fromDomainID: Number(transfer.from.id),
                     toDomainID: Number(transfer.to.id),
                     resourceID: transfer.resource.resourceId,
                     depositData: createERCDepositData(
                       fungibleTransfer.details.amount,
                       fungibleTransfer.details.recipient,
                       fungibleTransfer.details.parachainId,
                     ),
                   });
                 }
                 default:
                   throw new Error(`Unsupported fee handler type`);
               }
             */
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
    }
}