using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Relay;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoModule
    {
        private GelatoClient gelatoClient;

        public GelatoModule(IHttpClient httpClient)
        {
            this.gelatoClient = new GelatoClient(httpClient);
        }

        public void CallWithSyncFee(CallWithSyncFeeRequest request, RelayRequestOptions options = null)
        {
        }

        public void CallWithSyncFeeERC2771(CallWithSyncFeeERC2771Request request, IEvmProvider provider, RelayRequestOptions options = null)
        {
        }

        public void CallWithSyncFeeERC2771(CallWithSyncFeeERC2771Request request, IEvmSigner wallet, RelayRequestOptions options = null)
        {
        }

        public void SponsoredCall(SponsoredCallRequest request, string sponsorApiKey, RelayRequestOptions options = null)
        {
        }

        public void SponsoredCallERC2771(SponsoredCallERC2771Request request, IEvmProvider provider, string sponsorApiKey, RelayRequestOptions options = null)
        {
        }

        public void SponsoredCallERC2771(SponsoredCallERC2771Request request, IEvmSigner provider, string sponsorApiKey, RelayRequestOptions options = null)
        {
        }

        public void IsNetworkSupported(uint networkId)
        {
            // const supportedNetworks = await getSupportedNetworks(config);
            // return supportedNetworks.includes(payload.chainId.toString());
        }

        public void GetSupportedNetworks()
        {
            // try {
            //     return (await axios.get<{ relays: string[] }>(`${config.url}/relays/v2`))
            //     .data.relays;
            // } catch (error) {
            //     throw new Error(
            //     `GelatoRelaySDK/getSupportedNetworks: Failed with error: ${getHttpErrorMessage(
            //         error
            //     )}`
            //     );
            // }
        }

        public void IsOracleActive(ulong chainId)
        {
            // const oracles = await getGelatoOracles(config);
            // return oracles.includes(payload.chainId.toString());
        }

        public void GetPaymentTokens(ulong chainId)
        {
        }

        public void GetEstimatedFee(ulong chainId, string paymentToken, HexBigInteger gasLimit, bool isHighPriority, HexBigInteger gasLimitL1 = null)
        {
            if (gasLimitL1 == null)
            {
                gasLimitL1 = new HexBigInteger("0x0");
            }
        }

        public void GetTaskStatus(string taskId)
        {
        }
    }
}