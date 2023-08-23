using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato
{
    public class Gelato : IGelato, ILifecycleParticipant
    {
        private readonly GelatoClient gelatoClient;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;
        private readonly GelatoConfig config;
        private readonly IChainConfig chainConfig;

        public const string SponsoredCallErc2771TypeName = "SponsoredCallERC2771";
        public const string CallWithSyncFeeErc2771TypeName = "CallWithSyncFeeERC2771";

        public Gelato(IHttpClient httpClient, IChainConfig chainConfig, GelatoConfig config, ISigner signer, IContractBuilder contractBuilder)
        {
            gelatoClient = new GelatoClient(httpClient, config);
            this.signer = signer;
            this.config = config;
            this.chainConfig = chainConfig;
            this.contractBuilder = contractBuilder;
        }

        public Gelato(IHttpClient httpClient, IChainConfig chainConfig, GelatoConfig config, IContractBuilder contractBuilder)
        {
            gelatoClient = new GelatoClient(httpClient, config);
            this.config = config;
            this.chainConfig = chainConfig;
            this.contractBuilder = contractBuilder;
        }

        public async ValueTask WillStartAsync()
        {
            if (!await IsNetworkSupported(chainConfig.ChainId))
            {
                throw new Web3Exception("network not supported by Gelato");
            }
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public async Task<RelayResponse> CallWithSyncFee(CallWithSyncFeeRequest request)
        {
            try
            {
                request.ChainId = int.Parse(chainConfig.ChainId);
                return await gelatoClient.Post<CallWithSyncFeeRequest, RelayResponse>(RelayCall.CallWithSyncFee, request);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/relayWithSyncFee: Failed with error: {e.Message}");
            }
        }

        public async Task<RelayResponse> CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request)
        {
            return await CallWithSyncFeeErc2771(request, null);
        }

        // TODO: consume options
        public async Task<RelayResponse> CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request, RelayRequestOptions options)
        {
            if (signer == null)
            {
                throw new Web3Exception("GelatoRelaySDK/CallWithSyncFeeErc2771: No signer present for request");
            }

            try
            {
                var callRequest = new CallWithErc2771Request
                {
                    ChainId = int.Parse(chainConfig.ChainId),
                    Target = request.Target,
                    Data = request.Data,
                    User = await signer.GetAddress(),
                    UserDeadline = request.UserDeadline,
                    UserNonce = request.UserNonce,
                    FeeToken = request.FeeToken,
                    IsRelayContext = request.IsRelayContext,
                };

                var optional = await CallWithErc2771RequestOptionalParameters.PopulateOptionalUserParameters(callRequest, Erc2771Type.CallWithSyncFee, config, chainConfig, contractBuilder);
                var newStruct = callRequest.MapRequestToStruct<CallWithSyncFeeErc2771Struct>(optional, Erc2771Type.CallWithSyncFee);

                callRequest.UserDeadline ??= optional.UserDeadline;
                callRequest.UserNonce ??= optional.UserNonce;

                var types = new Dictionary<string, MemberDescription[]>
                {
                    [CallWithSyncFeeErc2771TypeName] = new[]
                    {
                        new MemberDescription { Name = "chainId", Type = "uint256" },
                        new MemberDescription { Name = "target", Type = "address" },
                        new MemberDescription { Name = "data", Type = "bytes" },
                        new MemberDescription { Name = "user", Type = "address" },
                        new MemberDescription { Name = "userNonce", Type = "uint256" },
                        new MemberDescription { Name = "userDeadline", Type = "uint256" },
                    },
                };

                callRequest.Signature = await signer.SignTypedData(
                    GetEip712Domain(Erc2771Type.CallWithSyncFee), types, CallWithSyncFeeErc2771TypeName, newStruct);

                return await gelatoClient.Post<CallWithErc2771Request, RelayResponse>(RelayCall.CallWithSyncFeeErc2771, callRequest);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/relayWithSyncFee: Failed with error: {e.Message}");
            }
        }

        public async Task<RelayResponse> SponsoredCall(SponsoredCallRequest request)
        {
            if (config.SponsorApiKey == null)
            {
                throw new Web3Exception("GelatoRelaySDK/sponsoredCall: Sponsor api key not provided");
            }

            try
            {
                request.SponsorApiKey = config.SponsorApiKey;
                request.ChainId = int.Parse(chainConfig.ChainId);
                return await gelatoClient.Post<SponsoredCallRequest, RelayResponse>(RelayCall.SponsoredCall, request);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/sponsoredCall: Failed with error: {e.Message}");
            }
        }

        public async Task<RelayResponse> SponsoredCallErc2771(SponsoredCallErc2771Request request)
        {
            return await SponsoredCallErc2771(request, null);
        }

        // TODO: consume options
        public async Task<RelayResponse> SponsoredCallErc2771(SponsoredCallErc2771Request request, RelayRequestOptions options)
        {
            if (signer == null)
            {
                throw new Web3Exception("GelatoRelaySDK/SponsoredCallErc2771: No signer present for request");
            }

            try
            {
                if (config.SponsorApiKey == null)
                {
                    throw new Web3Exception("GelatoRelaySDK/sponsoredCall: Sponsor api key not provided");
                }

                var callRequest = new CallWithErc2771Request
                {
                    ChainId = int.Parse(chainConfig.ChainId),
                    Target = request.Target,
                    Data = request.Data,
                    User = request.User,
                    UserDeadline = request.UserDeadline,
                    UserNonce = request.UserNonce,
                    SponsorApiKey = config.SponsorApiKey,
                };

                // Confirm Wallet & Provider chain ID match
                var optional = await CallWithErc2771RequestOptionalParameters.PopulateOptionalUserParameters(callRequest, Erc2771Type.SponsoredCall, config, chainConfig, contractBuilder);
                var newStruct = callRequest.MapRequestToStruct<SponsoredCallErc2771Struct>(optional, Erc2771Type.SponsoredCall);

                var types = new Dictionary<string, MemberDescription[]>
                {
                    [SponsoredCallErc2771TypeName] = new[]
                    {
                        new MemberDescription { Name = "chainId", Type = "uint256" },
                        new MemberDescription { Name = "target", Type = "address" },
                        new MemberDescription { Name = "data", Type = "bytes" },
                        new MemberDescription { Name = "user", Type = "address" },
                        new MemberDescription { Name = "userNonce", Type = "uint256" },
                        new MemberDescription { Name = "userDeadline", Type = "uint256" },
                    },
                };

                callRequest.Signature = await signer.SignTypedData(
                    GetEip712Domain(Erc2771Type.SponsoredCall), types, SponsoredCallErc2771TypeName, newStruct);
                callRequest.SponsorApiKey = config.SponsorApiKey;

                callRequest.UserDeadline ??= optional.UserDeadline;
                callRequest.UserNonce ??= optional.UserNonce;
                return await gelatoClient.Post<CallWithErc2771Request, RelayResponse>(RelayCall.SponsoredCallErc2771, callRequest);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/sponsoredCallErc2771: Failed with error: {e.Message}");
            }
        }

        public async Task<HexBigInteger> GetEstimatedFee(string paymentToken, HexBigInteger gasLimit, bool isHighPriority, HexBigInteger gasLimitL1 = null)
        {
            if (gasLimitL1 == null)
            {
                gasLimitL1 = new HexBigInteger("0x0");
            }

            var request = new EstimatedFeeRequest
            {
                ChainId = chainConfig.ChainId,
                GasLimit = gasLimit,
                GasLimitL1 = gasLimitL1,
                IsHighPriority = isHighPriority,
                PaymentToken = paymentToken,
            };

            return await gelatoClient.GetEstimatedFeeRequest(request);
        }

        private async Task<bool> IsNetworkSupported(string networkId)
        {
            var supportedNetworks = await gelatoClient.GetSupportedNetworks();
            return supportedNetworks.Contains(networkId);
        }

        private async Task<bool> IsOracleActive(string chainId)
        {
            var oracles = await gelatoClient.GetGelatoOracles();
            return oracles.Contains(chainId);
        }

        private SerializableDomain GetEip712Domain(Erc2771Type type)
        {
            return type switch
            {
                Erc2771Type.CallWithSyncFee => new SerializableDomain()
                {
                    Name = "GelatoRelayERC2771",
                    Version = "1",
                    ChainId = chainConfig.ChainId,
                    VerifyingContract = CallWithErc2771RequestOptionalParameters
                        .GetGelatoRelayErc2771Address(type, config, chainConfig),
                },
                Erc2771Type.SponsoredCall => new SerializableDomain()
                {
                    Name = "GelatoRelay1BalanceERC2771",
                    Version = "1",
                    ChainId = chainConfig.ChainId,
                    VerifyingContract = CallWithErc2771RequestOptionalParameters
                        .GetGelatoRelayErc2771Address(type, config, chainConfig),
                },
                _ => throw new Web3Exception("incorrect relay option")
            };
        }

        public async Task<RelayedTask> GetTaskStatus(string taskId)
        {
            return await gelatoClient.GetTaskStatus(taskId);
        }

        public async Task<string[]> GetPaymentTokens()
        {
            return await gelatoClient.GetPaymentTokens(chainConfig.ChainId);
        }
    }
}