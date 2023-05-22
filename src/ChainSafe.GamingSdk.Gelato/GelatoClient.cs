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
        private const string GELATO_RELAY_URL = "https://api.gelato.digital"; //Relay GW
        private const string SIGN_TYPED_DATA_V4 = "eth_signTypedData_v4";
        private const string DEFAULT_INTERNAL_ERROR_MESSAGE = "Internal Error";
        private const Int32 DEFAULT_DEADLINE_GAP = 86_400; //24H
        private static readonly string[] USER_NONCE_ABI = {"function userNonce(address account) external view returns (uint256)"};
        private const string GELATO_RELAY_ERC2771_ADDRESS = "0xb539068872230f20456CF38EC52EF2f91AF4AE49";
        private const string GELATO_RELAY_1BALANCE_ERC2771_ADDRESS = "0xd8253782c45a12053594b9deB72d8e8aB2Fca54c";
        private const string GELATO_RELAY_ERC2771_ZKSYNC_ADDRESS = "0x22DCC39b2AC376862183dd35A1664798dafC7Da6";
        private const string GELATO_RELAY_1BALANCE_ERC2771_ZKSYNC_ADDRESS = "0x97015cD4C3d456997DD1C40e2a18c79108FCc412";
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