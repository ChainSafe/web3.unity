using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Evm.Utils;
using ChainSafe.Gaming.SygmaClient.Contracts;
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
        private readonly Config clientConfiguration;

        public SygmaClient(IHttpClient httpClient, IChainConfig sourceChainConfig, IChainConfig destinationChainConfig, ISigner signer, IContractBuilder contractBuilder, IAnalyticsClient analyticsClient, IProjectConfig projectConfig)
        {
            this.contractBuilder = contractBuilder;
            this.signer = signer;
            this.sourceChainConfig = sourceChainConfig;
            this.destinationChainConfig = destinationChainConfig;
            this.analyticsClient = analyticsClient;
            this.projectConfig = projectConfig;
            this.clientConfiguration = new Config(httpClient, uint.Parse(sourceChainConfig.ChainId));
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
            this.clientConfiguration.Fetch(environment);
            return true;
        }

        public async Task<Transfer<NonFungible>> CreateNonFungibleTransfer(
            string sourceAddress,
            uint destinationChainId,
            string destinationAddress,
            string resourceId,
            string tokenId,
            string destinationProviderUrl = "")
        {
            var transfer = await this.CreateTransfer<NonFungible>(
                sourceAddress,
                destinationChainId,
                destinationAddress,
                resourceId,
                destinationProviderUrl);

            transfer.Details = new NonFungible(sourceAddress, tokenId);
            return transfer;
        }

        private Task<Transfer<T>> CreateTransfer<T>(
            string sourceAddress,
            uint destinationChainId,
            string destinationAddress,
            string resourceId,
            string destinationProviderUrl = "")
            where T : TransferType
        {
            var transferParams = this.BaseTransferParams(destinationChainId, resourceId);
            var transfer = new Transfer<T>(transferParams.DestinationDomain, transferParams.SourceDomain, sourceAddress)
            {
                Resource = transferParams.Resource,
            };

            return Task.FromResult(transfer);
        }

        public async Task<EvmFee> Fee<T>(Transfer<T> transfer)
            where T : TransferType
        {
            var feeData = await this.GetFeeInformation(transfer);
            switch (feeData.Type)
            {
                case FeeHandlerType.Basic:
                    return await this.CalculateBasicFee(transfer, feeData);
                default:
                    throw new Exception("Unsupported fee handler type");
            }
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

        private async Task<EvmFee> CalculateBasicFee<T>(Transfer<T> transfer, EvmFee feeData)
            where T : TransferType
        {
            var basicFeeHandler = new BasicFeeHandler(this.contractBuilder, feeData.HandlerAddress);
            return await basicFeeHandler.CalculateBasicFee(transfer.Sender, transfer.From.Id, transfer.To.Id, transfer.Resource.ResourceId);
        }

        private async Task<EvmFee> GetFeeInformation<T>(Transfer<T> transfer)
            where T : TransferType
        {
            var domainConfig = this.clientConfiguration.SourceDomainConfig();
            var feeRouter = new FeeHandlerRouter(this.contractBuilder, domainConfig.FeeRouter);
            var feeHandlerAddress = await feeRouter.DomainResourceIDToFeeHandlerAddress(transfer.To.Id, transfer.Resource.ResourceId);
            var feeHandlerConfig = domainConfig.FeeHandlers.Find(feeHandler => feeHandler.Address.Equals(feeHandlerAddress, StringComparison.OrdinalIgnoreCase));
            if (feeHandlerConfig == null)
            {
                throw new Exception("Fee handler not found");
            }

            return await Task.FromResult(new EvmFee(feeHandlerAddress, feeHandlerConfig.Type));
        }

        private BaseTransferParams BaseTransferParams(uint destinationChainId, string resourceId)
        {
            var sourceDomain = this.clientConfiguration.SourceDomainConfig();
            if (sourceDomain == null)
            {
                throw new Exception("Config for the provided source domain is not setup");
            }

            var destinationDomain = this.clientConfiguration.EnvironmentConfig.Domains.Find(
                cfg => cfg.ChainId == destinationChainId);
            if (destinationDomain == null)
            {
                throw new Exception("Config for the provided destination domain is not setup");
            }

            var resource = sourceDomain.Resources.Find(r => r.ResourceId.Equals(resourceId, StringComparison.OrdinalIgnoreCase));
            if (resource == null)
            {
                throw new Exception("Config for the provided resource is not setup");
            }

            return new BaseTransferParams
            {
                SourceDomain = sourceDomain,
                DestinationDomain = destinationDomain,
                Resource = resource,
            };
        }
    }
}