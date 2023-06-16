using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoModule
    {
        private readonly GelatoClient gelatoClient;
        private Config config;

        public GelatoModule(IHttpClient httpClient, Config config)
        {
            // Check if Config's chainId is valid
            this.gelatoClient = new GelatoClient(httpClient, config);
            this.config = config;

            // TODO: Remove blocking call once config init is resolved
            if (IsNetworkSupported(config.ChainId).Result == false)
            {
                throw new Exception("network not supported by Gelato");
            }
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

        public void CallWithSyncFeeERC2771(CallWithSyncFeeErc2771Request request, IEvmSigner wallet, RelayRequestOptions options = null)
        {
            CallWithSyncFeeERC2771(request, wallet.Provider, options);
        }

        public void CallWithSyncFeeERC2771(CallWithSyncFeeErc2771Request request, IEvmProvider provider, RelayRequestOptions options = null)
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

        public async Task<RelayResponse> SponsoredCall(SponsoredCallRequest request, string sponsorApiKey)
        {
            try
            {
                return await this.gelatoClient.Post<SponsoredCallRequest, RelayResponse>(RelayCall.CallWithSyncFee, request);
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/sponsoredCall: Failed with error: ${e.Message}");
            }
        }

        public void SponsoredCallERC2771(SponsoredCallErc2771Request request, IEvmSigner wallet, string sponsorApiKey, RelayRequestOptions options = null)
        {
            SponsoredCallERC2771(request, wallet.Provider, sponsorApiKey, options);
        }

        public void SponsoredCallERC2771(SponsoredCallErc2771Request request, IEvmProvider provider, string sponsorApiKey, RelayRequestOptions options = null)
        {
            throw new Exception("SponsoredCallERC2771 not implemented");
            /* var callRequest = new CallWithErc2771Request{
                ChainId = request.ChainId,
                Target = request.Target,
                Data = request.Data,
                User = request.User,
                UserDeadline = request.UserDeadline,
                UserNonce = request.UserNonce,
            };

            Confirm Wallet & Provider chain ID match
            var optional = await CallWithERC2771RequestOptionalParameters.PopulateOptionalUserParameters(callRequest, ERC2771Type.SponsoredCall, provider, this.config);
            var newStruct = callRequest.MapRequestToStruct(optional);

            // TODO: Sign typed data request

            return await this.gelatoClient.Post<CallWithErc2771Request, RelayResponse>(RelayCall.SponsoredCallERC2771, callRequest);*/
        }

        public async Task<HexBigInteger> GetEstimatedFee(ulong chainId, string paymentToken, HexBigInteger gasLimit, bool isHighPriority, HexBigInteger gasLimitL1 = null)
        {
            if (gasLimitL1 == null)
            {
                gasLimitL1 = new HexBigInteger("0x0");
            }

            var request = new EstimatedFeeRequest
            {
                ChainId = chainId,
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
    }
}