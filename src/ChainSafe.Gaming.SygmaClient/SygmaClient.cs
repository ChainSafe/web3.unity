using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
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
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json.Linq;
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
        private readonly IHttpClient httpClient;

        public SygmaClient(IHttpClient httpClient, IChainConfig sourceChainConfig, IChainConfig destinationChainConfig, ISigner signer, IContractBuilder contractBuilder, IAnalyticsClient analyticsClient, IProjectConfig projectConfig)
        {
            this.contractBuilder = contractBuilder;
            this.signer = signer;
            this.sourceChainConfig = sourceChainConfig;
            this.destinationChainConfig = destinationChainConfig;
            this.analyticsClient = analyticsClient;
            this.projectConfig = projectConfig;
            clientConfiguration = new Config(httpClient, uint.Parse(sourceChainConfig.ChainId));
            this.httpClient = httpClient;
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

        public async Task<TransactionRequest> BuildApprovals<T>(Transfer<T> transfer, EvmFee fee, string tokenAddress)
            where T : TransferType
        {
            var bridge = new Bridge(this.contractBuilder, clientConfiguration.SourceDomainConfig().Bridge);
            var handlerAddress = await bridge.DomainResourceIDToHandlerAddress(transfer.Resource.ResourceId);
            switch (transfer.Resource.Type)
            {
                case ResourceType.NonFungible:
                    return await new Erc721Approvals(contractBuilder, tokenAddress).ApprovalTransactionRequest(transfer, handlerAddress);
                default:
                    throw new NotImplementedException("This type is not implemented yet");
            }
        }

        public Task<TransactionRequest> BuildTransferTransaction<T>(Transfer<T> transfer, EvmFee fee)
            where T : TransferType
        {
            switch (transfer.Resource.Type)
            {
                case ResourceType.NonFungible:
                    var nonFungible = transfer as Transfer<NonFungible>;
                    return Erc721Transfer(
                        nonFungible!.Details.TokenId,
                        nonFungible.Details.Recipient,
                        nonFungible.To.Id.ToString(),
                        transfer.Resource.ResourceId,
                        fee);
                default:
                    throw new NotImplementedException($"This type {transfer.Resource.Type} is not implemented yet");
            }
        }

        private async Task<TransactionRequest> Erc721Transfer(string tokenId, string recipientAddress, string domainId, string resourceId, EvmFee feeData)
        {
            var sourceDomainConfig = clientConfiguration.SourceDomainConfig();
            var depositData = CreateDepositData(tokenId, recipientAddress);
            var bridge = new Bridge(contractBuilder, sourceDomainConfig.Bridge);

            var tx = await bridge.Contract.PrepareTransactionRequest(
                "deposit",
#pragma warning disable SA1118
                new object[]
                {
                domainId,
                resourceId,
                depositData,
                feeData.FeeData ?? "0x0",
                });
#pragma warning restore SA1118
            return tx;
        }

        // In sygmas SDK we also have Substrate, (check helpers.ts -> CreateERCDepositData)
        // We don't need paraChainID just yet since we are only working with Ethereum
        private string CreateDepositData(string tokenId, string recipient)
        {
            // Convert tokenId to a BigInteger and ensure it is a positive value.
            BigInteger tokenBigInt = BigInteger.Parse(tokenId);

            // Ensure the tokenId is represented in a 32-byte array, left-padded with zeros.
            byte[] tokenIdBytes = tokenBigInt.ToByteArray().Reverse().ToArray(); // Reverse to ensure little-endian to big-endian conversion if necessary.
            if (tokenIdBytes.Length < 32)
            {
                tokenIdBytes = tokenIdBytes.Concat(new byte[32 - tokenIdBytes.Length]).ToArray(); // Left-pad with zeros if necessary.
            }
            else if (tokenIdBytes.Length > 32)
            {
                throw new ArgumentException("Token ID is too large.");
            }

            // Convert recipient string to byte array.
            byte[] recipientBytes = Encoding.UTF8.GetBytes(recipient);

            // Encode the length of the recipient byte array as a 32-byte array.
            BigInteger recipientLengthBigInt = new BigInteger(recipientBytes.Length);
            byte[] recipientLengthBytes = recipientLengthBigInt.ToByteArray().Reverse().ToArray();
            if (recipientLengthBytes.Length < 32)
            {
                recipientLengthBytes = recipientLengthBytes.Concat(new byte[32 - recipientLengthBytes.Length]).ToArray();
            }

            // Concatenate the tokenIdBytes, recipientLengthBytes, and recipientBytes.
            List<byte> data = new List<byte>();
            data.AddRange(tokenIdBytes);
            data.AddRange(recipientLengthBytes);
            data.AddRange(recipientBytes);

            // Convert the resulting byte array to a hexadecimal string, ensuring it is prefixed with "0x".
            return "0x" + BitConverter.ToString(data.ToArray()).Replace("-", string.Empty).ToLower();
        }

        public Task<TransferStatus> TransferStatusData(Environment environment, string transactionHash)
        {
            var url = environment switch
            {
                Environment.Testnet => $"{IndexerUrl.Testnet}/api/transfers/txHash/{transactionHash}",
                Environment.Mainnet => $"{IndexerUrl.Mainnet}/api/transfers/txHash/{transactionHash}",
                _ => throw new InvalidOperationException($"Invalid environment {environment}")
            };

            var response = httpClient.GetRaw(url);
            if (!response.Result.IsSuccess)
            {
                throw new InvalidOperationException($"Didn't get successful response from the client");
            }

            var json = JObject.Parse(response.Result.Response);
            return Task.FromResult(Enum.Parse<TransferStatus>(json.GetValue("status").ToString()));
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