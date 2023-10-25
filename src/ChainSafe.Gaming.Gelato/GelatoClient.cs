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

            return (await httpClient.Post<TRequest, TResponse>(url, request)).AssertSuccess();
        }

        /// <summary>
        /// Retrieves an array of supported network names from the Gelato Relay service.
        /// </summary>
        /// <remarks>
        /// This method fetches a list of network names that represent the supported blockchain networks by the Gelato Relay service.
        /// Each string in the returned array corresponds to the name of a supported network, providing information about the networks
        /// on which Gelato relayers are operational.
        /// </remarks>
        /// <returns>An array of strings containing the names of supported networks by the Gelato Relay service.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
        public async Task<string[]> GetSupportedNetworks()
        {
            try
            {
                return (await httpClient.Get<SupportedNetworksResponse>($"{config.Url}/relays/v2")).AssertSuccess().Relays;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getSupportedNetworks: Failed with error: ${e.Message}");
            }
        }

        /// <summary>
        /// Retrieves an array of available Gelato oracles representing functional networks.
        /// </summary>
        /// <remarks>
        /// This method fetches a list of Gelato oracles, where each string in the returned array corresponds to a functional network
        /// where Gelato oracles are operational. The strings can be interpreted as network names, network identifiers, or some other
        /// network-related information, depending on how Gelato defines and uses these values.
        /// </remarks>
        /// <returns>An array of strings containing information about available Gelato oracles and their associated networks.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
        public async Task<string[]> GetGelatoOracles()
        {
            try
            {
                return (await httpClient.Get<OraclesResponse>($"{config.Url}/oracles/")).AssertSuccess().Oracles;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getGelatoOracles: Failed with error: ${e.Message}");
            }
        }

        /// <summary>
        /// Retrieves an array of available payment tokens for a specified blockchain chain.
        /// </summary>
        /// <param name="chainId">The unique identifier of the blockchain chain for which payment tokens are required.</param>
        /// <returns>An array of strings representing the available ERC20 token symbols accepted for payments on the specified blockchain chain.</returns>
        /// <remarks>
        /// This method fetches a list of ERC20 token symbols that are accepted for payments on the specified blockchain chain.
        /// The returned strings represent the symbols (e.g., "ETH" for Ethereum, "USDT" for Tether) of ERC20 tokens that can be used
        /// for payments and transactions on the specified chain.
        /// </remarks>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
        public async Task<string[]> GetPaymentTokens(string chainId)
        {
            try
            {
                return (await httpClient.Get<PaymentTokensResponse>($"{config.Url}/oracles/${chainId}/paymentTokens/")).AssertSuccess().PaymentTokens;
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
                return (await httpClient.Post<EstimatedFeeRequest, EstimatedFeeResponse>($"{config.Url}/oracles/${request.ChainId}/estimate/", request)).AssertSuccess().EstimatedFee;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getEstimatedFee: Failed with error: ${e.Message}");
            }
        }

        /// <summary>
        /// Retrieves the status of a task from the Gelato Relay service.
        /// </summary>
        /// <param name="taskId">The unique ID of a task which has been received by the Gelato relayer.</param>
        /// <returns>The current status of the specified task.</returns>
        /// <exception cref="Web3Exception">Thrown when the retrieval process encounters any issues or the underlying HTTP call fails.</exception>
        public async Task<RelayedTask> GetTaskStatus(string taskId)
        {
            try
            {
                return (await httpClient.Get<TransactionStatusResponse>($"{config.Url}/tasks/status/{taskId}")).AssertSuccess().Task;
            }
            catch (Exception e)
            {
                throw new Web3Exception($"GelatoRelaySDK/getTaskStatus: Failed with error: ${e.Message}");
            }
        }
    }
}