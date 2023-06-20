using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoModule : IGelatoModule, ILifecycleParticipant
    {
        private readonly GelatoClient gelatoClient;
        private readonly IRpcProvider provider;
        private readonly GelatoConfig config;
        private readonly IChainConfig chainConfig;

        public GelatoModule(IHttpClient httpClient, IChainConfig chainConfig, GelatoConfig config, IRpcProvider provider)
        {
            this.gelatoClient = new GelatoClient(httpClient, config);
            this.provider = provider;
            this.config = config;
            this.chainConfig = chainConfig;
        }

        public async Task<RelayResponse> CallWithSyncFee(CallWithSyncFeeRequest request)
        {
            try
            {
                return await this.gelatoClient.Post<CallWithSyncFeeRequest, RelayResponse>(RelayCall.CallWithSyncFee, request);
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/relayWithSyncFee: Failed with error: ${e.Message}");
            }
        }

        public void CallWithSyncFeeErc2771(CallWithSyncFeeErc2771Request request, ISigner wallet, RelayRequestOptions options = null)
        {
            throw new Exception("CallWithSyncFeeERC2771 not implemented");
            /*var callRequest = new CallWithErc2771Request{
                ChainId = request.ChainId,
                Target = request.Target,
                Data = request.Data,
                User = request.User,
                UserDeadline = request.UserDeadline,
                UserNonce = request.UserNonce,
            };
            Confirm Wallet & Provider chain ID match

            var optional = await CallWithERC2771RequestOptionalParameters.PopulateOptionalUserParameters(callRequest, ERC2771Type.SponsoredCall, provider, this.config);
            var newStruct = callRequest.MapRequestToStruct(optional);*/
        }

        public async Task<RelayResponse> SponsoredCall(SponsoredCallRequest request)
        {
            try
            {
                request.SponsorApiKey = config.SponsorApiKey;
                return await this.gelatoClient.Post<SponsoredCallRequest, RelayResponse>(RelayCall.CallWithSyncFee, request);
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/sponsoredCall: Failed with error: ${e.Message}");
            }
        }

        public async Task<RelayResponse> SponsoredCallErc2771(SponsoredCallErc2771Request request, ISigner wallet, RelayRequestOptions options = null)
        {
            // throw new Exception("SponsoredCallERC2771 not implemented");
            var callRequest = new CallWithErc2771Request
            {
                ChainId = request.ChainId,
                Target = request.Target,
                Data = request.Data,
                User = request.User,
                UserDeadline = request.UserDeadline,
                UserNonce = request.UserNonce,
                SponsorApiKey = config.SponsorApiKey,
            };

            // Confirm Wallet & Provider chain ID match
            var optional = await CallWithErc2771RequestOptionalParameters.PopulateOptionalUserParameters(callRequest, Erc2771Type.SponsoredCall, provider, config, chainConfig);
            var newStruct = callRequest.MapRequestToStruct(optional);

            // TODO: Sign typed data request
            var types = new Dictionary<string, MemberDescription[]>
            {
                ["SponsoredCallERC2771"] = new[]
                {
                    new MemberDescription { Name = "target", Type = "address" },
                    new MemberDescription { Name = "data", Type = "bytes" },
                    new MemberDescription { Name = "user", Type = "address" },
                    new MemberDescription { Name = "userNonce", Type = "uint256" },
                    new MemberDescription { Name = "userDeadline", Type = "uint256" },
                },
            };

            callRequest.Signature = await wallet.SignTypedData(GetEip712Domain(Erc2771Type.SponsoredCall), types, newStruct);
            callRequest.SponsorApiKey = config.SponsorApiKey;
            return await this.gelatoClient.Post<CallWithErc2771Request, RelayResponse>(RelayCall.SponsoredCallErc2771, callRequest);
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

            return await this.gelatoClient.GetEstimatedFeeRequest(request);
        }

        private async Task<bool> IsNetworkSupported(string networkId)
        {
            var supportedNetworks = await this.gelatoClient.GetSupportedNetworks();
            return supportedNetworks.Contains(networkId);
        }

        private async Task<bool> IsOracleActive(string chainId)
        {
            var oracles = await this.gelatoClient.GetGelatoOracles();
            return oracles.Contains(chainId);
        }

        private Domain GetEip712Domain(Erc2771Type type)
        {
            return type switch
            {
                Erc2771Type.SponsoredCall => new Domain()
                {
                    Name = "GelatoRelayERC2771",
                    Version = "1",
                    ChainId = new BigInteger(int.Parse(chainConfig.ChainId)),
                    VerifyingContract = CallWithErc2771RequestOptionalParameters
                        .GetGelatoRelayErc2771Address(type, config, chainConfig)
                        .ToString(),
                },
                Erc2771Type.CallWithSyncFee => new Domain()
                {
                    Name = "GelatoRelay1BalanceERC2771",
                    Version = "1",
                    ChainId = new BigInteger(int.Parse(chainConfig.ChainId)),
                    VerifyingContract = CallWithErc2771RequestOptionalParameters
                        .GetGelatoRelayErc2771Address(type, config, chainConfig)
                        .ToString(),
                },
                _ => throw new Exception("incorrect relay option")
            };
        }

        public async ValueTask WillStartAsync()
        {
            if (await IsNetworkSupported(chainConfig.ChainId) == false)
            {
                throw new Exception("network not supported by Gelato");
            }
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);
    }
}