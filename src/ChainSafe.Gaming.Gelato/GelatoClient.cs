using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato
{
    /// <summary>
    /// Client for interacting with the Gelato Relay service.
    /// </summary>
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

        /// <summary>
        /// Posts a request to the Gelato Relay service and retrieves the response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request payload.</typeparam>
        /// <typeparam name="TResponse">The expected response type.</typeparam>
        /// <param name="relayCall">The specific type of relay call being made.</param>
        /// <param name="request">The request payload.</param>
        /// <returns>The response from the Gelato Relay service, mapped to the specified type.</returns>
        /// <exception cref="Web3Exception">Thrown when an unsupported relay call is provided or when there's an error in the HTTP request.</exception>
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

        /// <summary>
        /// Retrieves an array of supported networks from the Gelato Relay service.
        /// </summary>
        /// <returns>An array of strings representing the supported networks by the Gelato Relay service.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
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

        /// <summary>
        /// Retrieves an array of available Gelato oracles.
        /// </summary>
        /// <returns>An array of strings representing the available Gelato oracles.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
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

        /// <summary>
        /// Retrieves an array of available payment tokens for a specified chain.
        /// </summary>
        /// <param name="chainId">The ID of the chain for which payment tokens are required.</param>
        /// <returns>An array of strings representing the available payment tokens for the specified chain.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
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

        /// <summary>
        /// Gets the estimated fee for a specified request.
        /// </summary>
        /// <param name="request">The Gelato relay request object which is being sent for cost estimation.</param>
        /// <returns>The estimated fee as a <see cref="HexBigInteger"/>.</returns>
        /// <exception cref="Web3Exception">Thrown when the estimation process encounters any issues or the underlying HTTP call fails.</exception>
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

        /// <summary>
        /// Retrieves the status of a task from the Gelato Relay service.
        /// </summary>
        /// <param name="taskId">The unique ID of the task for which the status is being fetched.</param>
        /// <returns>The current status of the specified task.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
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