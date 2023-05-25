using System;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Relay;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoClient
    {
        private const string GelatoRelayUrl = "https://api.gelato.digital"; // Relay GW
        public const string SignedTypedDataV4 = "eth_signTypedData_v4";
        private const string DefaultInternalErrorMessage = "Internal Error";
        public const int DefaultDeadlineGap = 86_400; // 24H
        public const string GelatoRelayErc2771Address = "0xb539068872230f20456CF38EC52EF2f91AF4AE49";
        public const string GelatoRelay1BalanceErc2771Address = "0xd8253782c45a12053594b9deB72d8e8aB2Fca54c";
        public const string GelatoRelayErc2771ZksyncAddress = "0x22DCC39b2AC376862183dd35A1664798dafC7Da6";
        public const string GelatoRelay1BalanceErc2771ZksyncAddress = "0x97015cD4C3d456997DD1C40e2a18c79108FCc412";
        public static readonly string UserNonceAbi = "[{\"inputs\": [{\"internalType\": \"address\",\"name\": \"account\",\"type\": \"address\"}],\"name\": \"userNonce\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"}]\"}";
        private IHttpClient httpClient;
        private Config config;

        public GelatoClient(IHttpClient httpClient, Config config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

        public async Task<TResponse> Post<TRequest, TResponse>(RelayCall relayCall, TRequest request)
        {
            switch (relayCall)
            {
                case RelayCall.CallWithSyncFee:
                    return (await this.httpClient.Post<TRequest, TResponse>($"{config.Url}/relays/v2/call-with-sync-fee", request)).Response;

                case RelayCall.CallWithSyncFeeERC2771:
                    return (await this.httpClient.Post<TRequest, TResponse>($"{config.Url}/relays/v2/call-with-sync-fee-erc2771", request)).Response;

                case RelayCall.SponsoredCall:
                    return (await this.httpClient.Post<TRequest, TResponse>($"{config.Url}/relays/v2/sponsored-call", request)).Response;

                case RelayCall.SponsoredCallERC2771:
                    return (await this.httpClient.Post<TRequest, TResponse>($"{config.Url}/relays/v2/sponsored-call-erc2771", request)).Response;
                default:
                    throw new Exception("relayCall option not found");
            }
        }

        public async Task<string[]> GetSupportedNetworks()
        {
            try
            {
                return (await this.httpClient.Get<SupportedNetworksResponse>($"{config.Url}/relays/v2")).Response.Relays;
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
                return (await this.httpClient.Get<OraclesResponse>($"{config.Url}/oracles/")).Response.Oracles;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getGelatoOracles: Failed with error: ${e.Message}");
            }
        }

        public async Task<string[]> GetPaymentTokens(ulong chainId)
        {
            try
            {
                return (await this.httpClient.Get<PaymentTokensResponse>($"{config.Url}/oracles/${chainId}/paymentTokens/")).Response.PaymentTokens;
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
                return (await this.httpClient.Post<EstimatedFeeRequest, EstimatedFeeResponse>($"{config.Url}/oracles/${request.ChainId}/estimate/", request)).Response.EstimatedFee;
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
                return (await this.httpClient.Get<TransactionStatusResponse>($"{config.Url}/tasks/status/{taskId}")).Response.Task;
            }
            catch (System.Exception e)
            {
                throw new Exception($"GelatoRelaySDK/getTaskStatus: Failed with error: ${e.Message}");
            }
        }
    }
}