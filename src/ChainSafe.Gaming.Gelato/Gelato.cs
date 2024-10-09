using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato
{
    public class Gelato : IGelato, ILifecycleParticipant, IChainSwitchHandler
    {
        private readonly GelatoClient gelatoClient;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;
        private readonly GelatoConfig config;
        private readonly IChainConfig chainConfig;
        private readonly IAnalyticsClient analyticsClient;

        private bool gelatoDisabled;

        public Gelato(IHttpClient httpClient, IChainConfig chainConfig, GelatoConfig config, ISigner signer, IContractBuilder contractBuilder, IAnalyticsClient analyticsClient, IProjectConfig projectConfig)
        {
            gelatoClient = new GelatoClient(httpClient, config, analyticsClient, chainConfig, projectConfig);
            this.signer = signer;
            this.config = config;
            this.chainConfig = chainConfig;
            this.contractBuilder = contractBuilder;
            this.analyticsClient = analyticsClient;
        }

        public Gelato(IHttpClient httpClient, IChainConfig chainConfig, IProjectConfig projectConfig, GelatoConfig config, IContractBuilder contractBuilder, IAnalyticsClient analyticsClient)
        {
            gelatoClient = new GelatoClient(httpClient, config, analyticsClient, chainConfig, projectConfig);
            this.config = config;
            this.chainConfig = chainConfig;
            this.contractBuilder = contractBuilder;
        }

        public async ValueTask WillStartAsync()
        {
            if (await FetchGelatoDisabled())
            {
                return;
            }

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = "Gelato initialized",
                PackageName = "io.chainsafe.web3-unity",
            });
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public async Task HandleChainSwitching()
        {
            if (await FetchGelatoDisabled())
            {
                return;
            }

            analyticsClient.CaptureEvent(new AnalyticsEvent
            {
                EventName = "Gelato reinitialized during chain switching",
                PackageName = "io.chainsafe.web3-unity",
            });
        }

        public bool GetGelatoDisabled() => gelatoDisabled;

        public async Task<RelayResponse> CallWithSyncFee(CallWithSyncFeeRequest request)
        {
            try
            {
                request.ChainId = int.Parse(chainConfig.ChainId);
                return await gelatoClient.Post<CallWithSyncFeeRequest, RelayResponse>(RelayCall.CallWithSyncFee, request);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/CallWithSyncFee: Failed with error: {e.Message}");
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
                    User = signer.PublicAddress,
                    UserDeadline = request.UserDeadline,
                    UserNonce = request.UserNonce,
                    FeeToken = request.FeeToken,
                    IsRelayContext = request.IsRelayContext,
                };

                var optional =
                    await CallWithErc2771RequestOptionalParameters.PopulateOptionalUserParameters(
                        callRequest, Erc2771Type.CallWithSyncFee, config, chainConfig, contractBuilder);

                var newStruct =
                    callRequest.MapRequestToStruct<CallWithSyncFeeErc2771Struct>(optional, Erc2771Type.CallWithSyncFee);

                callRequest.Signature =
                    await signer.SignTypedData(GetEip712Domain(Erc2771Type.CallWithSyncFee), newStruct);
                callRequest.UserDeadline ??= optional.UserDeadline;
                callRequest.UserNonce ??= optional.UserNonce;
                return await gelatoClient.Post<CallWithErc2771Request, RelayResponse>(RelayCall.CallWithSyncFeeErc2771, callRequest);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/CallWithSyncFeeErc721: Failed with error: {e.Message}");
            }
        }

        public async Task<RelayResponse> SponsoredCall(SponsoredCallRequest request)
        {
            if (config.SponsorApiKey == null)
            {
                throw new Web3Exception("GelatoRelaySDK/SponsoredCall: Sponsor api key not provided");
            }

            try
            {
                request.SponsorApiKey = config.SponsorApiKey;
                request.ChainId = int.Parse(chainConfig.ChainId);
                return await gelatoClient.Post<SponsoredCallRequest, RelayResponse>(RelayCall.SponsoredCall, request);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/SponsoredCall: Failed with error: {e.Message}");
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
                    throw new Web3Exception("GelatoRelaySDK/SponsoredCallErc2771: Sponsor api key not provided");
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
                var optional =
                    await CallWithErc2771RequestOptionalParameters.PopulateOptionalUserParameters(
                        callRequest, Erc2771Type.SponsoredCall, config, chainConfig, contractBuilder);

                var newStruct =
                    callRequest.MapRequestToStruct<SponsoredCallErc2771Struct>(optional, Erc2771Type.SponsoredCall);

                callRequest.Signature =
                    await signer.SignTypedData(GetEip712Domain(Erc2771Type.SponsoredCall), newStruct);

                callRequest.UserDeadline ??= optional.UserDeadline;
                callRequest.UserNonce ??= optional.UserNonce;
                callRequest.SponsorApiKey = config.SponsorApiKey;

                return await gelatoClient.Post<CallWithErc2771Request, RelayResponse>(RelayCall.SponsoredCallErc2771, callRequest);
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/SponsoredCallErc2771: Failed with error: {e.Message}");
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
                    ChainId = BigInteger.Parse(chainConfig.ChainId),
                    VerifyingContract = CallWithErc2771RequestOptionalParameters
                        .GetGelatoRelayErc2771Address(type, config, chainConfig),
                },
                Erc2771Type.SponsoredCall => new SerializableDomain()
                {
                    Name = "GelatoRelay1BalanceERC2771",
                    Version = "1",
                    ChainId = BigInteger.Parse(chainConfig.ChainId),
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

        private async Task<bool> FetchGelatoDisabled()
        {
            return gelatoDisabled = !await IsNetworkSupported(chainConfig.ChainId);
        }
    }
}