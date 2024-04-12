using System;
using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient.Contracts;
using ChainSafe.Gaming.SygmaClient.DepositDataHandlers;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Environment = ChainSafe.Gaming.SygmaClient.Types.Environment;

namespace ChainSafe.Gaming.SygmaClient
{
    public class SygmaClient : ISygmaClient, ILifecycleParticipant
    {
        private const string Deposit = "deposit";

        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;
        private readonly IChainConfig sourceChainConfig;
        private readonly IAnalyticsClient analyticsClient;
        private readonly IProjectConfig projectConfig;
        private readonly Config clientConfiguration;
        private readonly IHttpClient httpClient;
        private readonly ILogWriter logWriter;
        private readonly ITransactionExecutor transactionExecutor;

        public SygmaClient(
            ILogWriter logWriter,
            IHttpClient httpClient,
            IChainConfig sourceChainConfig,
            ISigner signer,
            IContractBuilder contractBuilder,
            IAnalyticsClient analyticsClient,
            IProjectConfig projectConfig,
            ITransactionExecutor transactionExecutor)
        {
            this.contractBuilder = contractBuilder;
            this.signer = signer;
            this.sourceChainConfig = sourceChainConfig;
            this.analyticsClient = analyticsClient;
            this.projectConfig = projectConfig;
            clientConfiguration = new Config(httpClient, uint.Parse(sourceChainConfig.ChainId));
            this.httpClient = httpClient;
            this.logWriter = logWriter;
            this.transactionExecutor = transactionExecutor;
        }

        public ValueTask WillStartAsync()
        {
            return default;
        }

        public ValueTask WillStopAsync()
        {
            return default;
        }

        public bool Initialize(Environment environment)
        {
            clientConfiguration.Fetch(environment);
            return true;
        }

        public async Task<Transfer<NonFungible>> CreateNonFungibleTransfer(
            string sourceAddress,
            string destinationAddress,
            uint destinationChainId,
            ResourceType resourceType,
            string tokenId,
            HexBigInteger amount = null,
            string destinationProviderUrl = "")
        {
            var transfer = await CreateTransfer<NonFungible>(
                sourceAddress,
                destinationChainId,
                resourceType);

            transfer.Details = new NonFungible(destinationAddress, tokenId, amount);
            return transfer;
        }

        public async Task<TransactionResponse> Transfer(SygmaTransferParams transferParams)
        {
            // TODO: Check if you own a token/have enough balance of the token before doing anything else so that end-user knows why the transfers would potentially fail, if their configuration is ok.
            Transfer transfer = null;
            switch (transferParams.ResourceType)
            {
                case ResourceType.Erc721:
                case ResourceType.Erc1155:
                    transfer = await CreateNonFungibleTransfer(transferParams.SourceAddress, transferParams.DestinationAddress, transferParams.DestinationChainId, transferParams.ResourceType, transferParams.TokenId, transferParams.Amount);
                    break;
                case ResourceType.Erc20:
                    transfer = await CreateFungibleTransfer(transferParams.SourceAddress, transferParams.DestinationAddress, transferParams.DestinationChainId, transferParams.ResourceType, transferParams.Amount);
                    break;
                default:
                    throw new NotImplementedException(
                        $"For resource type {transferParams.ResourceType} we don't have any handlers");
            }

            var fee = await Fee(transfer);

            var approvalsTransaction = await BuildApprovals(transfer);
            await transactionExecutor.SendTransaction(approvalsTransaction);

            var transferTransaction = await BuildTransferTransaction(transfer, fee);
            return await transactionExecutor.SendTransaction(transferTransaction);
        }

        public async Task<Transfer<Fungible>> CreateFungibleTransfer(
            string sourceAddress,
            string destinationAddress,
            uint destinationChainId,
            ResourceType resourceType,
            HexBigInteger amount,
            string destinationProviderUrl = "")
        {
            var transfer = await CreateTransfer<Fungible>(
                sourceAddress,
                destinationChainId,
                resourceType);

            transfer.Details = new Fungible(destinationAddress, amount);
            return transfer;
        }

        private Task<Transfer<T>> CreateTransfer<T>(
            string sourceAddress,
            uint destinationChainId,
            ResourceType resourceType)
            where T : TransferType
        {
            var transferParams = BaseTransferParams(resourceType, destinationChainId);
            var transfer = new Transfer<T>(transferParams.DestinationDomain, transferParams.SourceDomain, sourceAddress)
            {
                Resource = transferParams.Resource,
            };

            return Task.FromResult(transfer);
        }

        public async Task<EvmFee> Fee(Transfer transfer)
        {
            var feeData = await FeeInformation(transfer);
            switch (feeData.Type)
            {
                case FeeHandlerType.Basic:
                    return await CalculateBasicFee(transfer, feeData);
                default:
                    throw new Exception("Unsupported fee handler type");
            }
        }

        public async Task<TransactionRequest> BuildApprovals(Transfer transfer)
        {
            var bridge = new Bridge(contractBuilder, clientConfiguration.SourceDomainConfig.Bridge);
            var handlerAddress = await bridge.DomainResourceIDToHandlerAddress(transfer.Resource.ResourceId);
            var evmResource = (EvmResource)transfer.Resource;

            return transfer.Resource.Type switch
            {
                ResourceType.NonFungible or ResourceType.Erc1155 => await new Erc1155Approvals(
                    contractBuilder,
                    evmResource.Address).ApprovalTransactionRequest(handlerAddress, signer),
                _ => throw new NotImplementedException("This type is not implemented yet")
            };
        }

        public Task<TransactionRequest> BuildTransferTransaction(Transfer transfer, EvmFee fee)
        {
            switch (transfer.Resource.Type)
            {
                case ResourceType.NonFungible:
                case ResourceType.Erc1155:
                    var nonFungible = transfer as Transfer<NonFungible>;
                    return NonFungibleTransfer(
                        nonFungible,
                        fee);
                default:
                    throw new NotImplementedException($"This type {transfer.Resource.Type} is not implemented yet");
            }
        }

        private async Task<TransactionRequest> NonFungibleTransfer(Transfer<NonFungible> transfer, EvmFee feeData)
        {
            var sourceDomainConfig = clientConfiguration.SourceDomainConfig;
            var depositData = DepositDataFactory.Handler(transfer.Resource.Type).CreateDepositData(transfer);
            var bridge = new Bridge(contractBuilder, sourceDomainConfig.Bridge);

            var tx = await bridge.Contract.PrepareTransactionRequest(
                Deposit,
#pragma warning disable SA1118
                new object[]
                {
                    transfer.To.Id,
                    new HexBigInteger(transfer.Resource.ResourceId).ToHexByteArray(),
                    depositData,
                    feeData.FeeData.HexToByteArray(),
                }, new TransactionRequest()
                {
                    Value = feeData.Fee.ToHexBigInteger(),
                });
#pragma warning restore SA1118
            return tx;
        }

        public async Task<TransferStatus> TransferStatusData(Environment environment, string transactionHash)
        {
            var url = environment switch
            {
                Environment.Testnet => $"{IndexerUrl.Testnet}/api/transfers/txHash/{transactionHash}",
                Environment.Mainnet => $"{IndexerUrl.Mainnet}/api/transfers/txHash/{transactionHash}",
                _ => throw new InvalidOperationException($"Invalid environment {environment}")
            };

            var response = await httpClient.GetRaw(url);
            if (!response.IsSuccess)
            {
                throw new InvalidOperationException($"Didn't get successful response from the client");
            }

            JArray jArray;
            using (var reader = new JsonTextReader(new StringReader(response.Response)))
            {
                jArray = await JArray.LoadAsync(reader);
            }

            return Enum.Parse<TransferStatus>(jArray[0]["status"].ToString(), true);
        }

        private Task<EvmFee> CalculateBasicFee(Transfer transfer, EvmFee feeData)
        {
            var basicFeeHandler = new BasicFeeHandler(contractBuilder, feeData.HandlerAddress);
            return basicFeeHandler.CalculateBasicFee(transfer, feeData);
        }

        private async Task<EvmFee> FeeInformation(Transfer transfer)
        {
            var domainConfig = clientConfiguration.SourceDomainConfig;
            var feeRouter = new FeeHandlerRouter(contractBuilder, domainConfig.FeeRouter);
            var feeHandlerAddress = await feeRouter.DomainResourceIDToFeeHandlerAddress(transfer.To.Id, new HexBigInteger(transfer.Resource.ResourceId));
            var feeHandlerConfig = domainConfig.FeeHandlers.Find(feeHandler => feeHandler.Address.Equals(feeHandlerAddress, StringComparison.OrdinalIgnoreCase));
            if (feeHandlerConfig == null)
            {
                throw new Exception("Fee handler not found");
            }

            return new EvmFee(feeHandlerAddress, feeHandlerConfig.Type);
        }

        private BaseTransferParams BaseTransferParams(ResourceType resourceType, uint destinationChainId)
        {
            var sourceDomain = this.clientConfiguration.SourceDomainConfig;
            if (sourceDomain == null)
            {
                throw new Exception("Config for the provided source domain is not setup");
            }

            var destinationDomain = this.clientConfiguration.DestinationDomainConfig(destinationChainId);
            if (destinationDomain == null)
            {
                throw new Exception("Config for the provided destination domain is not setup");
            }

            var resource = sourceDomain.Resources.Find(r => r.Type == resourceType);
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