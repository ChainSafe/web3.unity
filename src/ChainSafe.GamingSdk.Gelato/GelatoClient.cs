using System;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoClient
    {
        public const int DefaultDeadlineGap = 86_400; // 24H
        private readonly IHttpClient httpClient;
        private readonly GelatoConfig config;

        public GelatoClient(IHttpClient httpClient, GelatoConfig config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

        public async Task<TResponse> Post<TRequest, TResponse>(RelayCall relayCall, TRequest request)
        {
            var url = relayCall switch
            {
                RelayCall.CallWithSyncFee => $"{config.Url}/relays/v2/call-with-sync-fee",
                RelayCall.CallWithSyncFeeErc2771 => $"{config.Url}/relays/v2/call-with-sync-fee-erc2771",
                RelayCall.SponsoredCall => $"{config.Url}/relays/v2/sponsored-call",
                RelayCall.SponsoredCallErc2771 => $"{config.Url}/relays/v2/sponsored-call-erc2771",
                _ => throw new Web3Exception("relayCall option not found")
            };

            return (await httpClient.Post<TRequest, TResponse>(url, request)).EnsureResponse();
        }

        public async Task<string[]> GetSupportedNetworks()
        {
            try
            {
                return (await httpClient.Get<SupportedNetworksResponse>($"{config.Url}/relays/v2")).EnsureResponse().Relays;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getSupportedNetworks: Failed with error: ${e.Message}");
            }
        }

        public async Task<string[]> GetGelatoOracles()
        {
            try
            {
                return (await httpClient.Get<OraclesResponse>($"{config.Url}/oracles/")).EnsureResponse().Oracles;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getGelatoOracles: Failed with error: ${e.Message}");
            }
        }

        public async Task<string[]> GetPaymentTokens(string chainId)
        {
            try
            {
                return (await httpClient.Get<PaymentTokensResponse>($"{config.Url}/oracles/${chainId}/paymentTokens/")).EnsureResponse().PaymentTokens;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getPaymentTokens: Failed with error: ${e.Message}");
            }
        }

        public async Task<HexBigInteger> GetEstimatedFeeRequest(EstimatedFeeRequest request)
        {
            try
            {
                return (await httpClient.Post<EstimatedFeeRequest, EstimatedFeeResponse>($"{config.Url}/oracles/${request.ChainId}/estimate/", request)).EnsureResponse().EstimatedFee;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getEstimatedFee: Failed with error: ${e.Message}");
            }
        }

        public async Task<RelayedTask> GetTaskStatus(string taskId)
        {
            try
            {
                return (await httpClient.Get<TransactionStatusResponse>($"{config.Url}/tasks/status/{taskId}")).EnsureResponse().Task;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getTaskStatus: Failed with error: ${e.Message}");
            }
        }
    }
}