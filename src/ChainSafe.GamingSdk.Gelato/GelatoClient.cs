using System;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoClient
    {
        public const int DefaultDeadlineGap = 86_400; // 24H
        public const string UserNonceAbi = "[{\"inputs\": [" +
                "{\"internalType\": \"address\",\"name\": \"account\",\"type\": \"address\"}" +
            "]," +
            "\"name\": \"userNonce\"," +
            "\"outputs\": [" +
                "{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}" +
            "]," +
            "\"stateMutability\": \"view\"," +
            "\"type\": \"function\"" +
            "}]";

        private readonly IHttpClient httpClient;
        private readonly GelatoConfig config;

        public GelatoClient(IHttpClient httpClient, GelatoConfig config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

        public async Task<TResponse> Post<TRequest, TResponse>(RelayCall relayCall, TRequest request)
        {
            return relayCall switch
            {
                RelayCall.CallWithSyncFee => (await this.httpClient.Post<TRequest, TResponse>(
                    $"{config.Url}/relays/v2/call-with-sync-fee", request)).EnsureResponse(),
                RelayCall.CallWithSyncFeeErc2771 => (await this.httpClient.Post<TRequest, TResponse>(
                    $"{config.Url}/relays/v2/call-with-sync-fee-erc2771", request)).EnsureResponse(),
                RelayCall.SponsoredCall => (await this.httpClient.Post<TRequest, TResponse>(
                    $"{config.Url}/relays/v2/sponsored-call", request)).EnsureResponse(),
                RelayCall.SponsoredCallErc2771 => (await this.httpClient.Post<TRequest, TResponse>(
                    $"{config.Url}/relays/v2/sponsored-call-erc2771", request)).EnsureResponse(),
                _ => throw new Exception("relayCall option not found")
            };
        }

        public async Task<string[]> GetSupportedNetworks()
        {
            try
            {
                return (await this.httpClient.Get<SupportedNetworksResponse>($"{config.Url}/relays/v2")).EnsureResponse().Relays;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getSupportedNetworks: Failed with error: ${e.Message}");
            }
        }

        public async Task<string[]> GetGelatoOracles()
        {
            try
            {
                return (await this.httpClient.Get<OraclesResponse>($"{config.Url}/oracles/")).EnsureResponse().Oracles;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getGelatoOracles: Failed with error: ${e.Message}");
            }
        }

        public async Task<string[]> GetPaymentTokens(string chainId)
        {
            try
            {
                return (await this.httpClient.Get<PaymentTokensResponse>($"{config.Url}/oracles/${chainId}/paymentTokens/")).EnsureResponse().PaymentTokens;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getPaymentTokens: Failed with error: ${e.Message}");
            }
        }

        public async Task<HexBigInteger> GetEstimatedFeeRequest(EstimatedFeeRequest request)
        {
            try
            {
                return (await this.httpClient.Post<EstimatedFeeRequest, EstimatedFeeResponse>($"{config.Url}/oracles/${request.ChainId}/estimate/", request)).EnsureResponse().EstimatedFee;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getEstimatedFee: Failed with error: ${e.Message}");
            }
        }

        public async Task<RelayedTask> GetTaskStatus(string taskId)
        {
            try
            {
                return (await this.httpClient.Get<TransactionStatusResponse>($"{config.Url}/tasks/status/{taskId}")).EnsureResponse().Task;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getTaskStatus: Failed with error: ${e.Message}");
            }
        }
    }
}