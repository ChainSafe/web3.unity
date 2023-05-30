using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Gelato.Relay;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

// TODO:
// Checksum address confirms
// Check network ID matches provider & is a supported network
// Get config from network.js or whichever
// Last part of the ERC2771 calls
// Oracle checks
// Check in array Contains is missing
// Clean up EIP712Domain
// Remove redundant/unused interfaces
namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoModule
    {
        private GelatoClient gelatoClient;
        private Config config;

        public GelatoModule(IHttpClient httpClient, Config config)
        {
            // Check if Config's chainId is valid
            this.gelatoClient = new GelatoClient(httpClient, config);
            this.config = config;
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
            // Confirm Wallet & Provider chain ID match
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
        }

        public void GetEstimatedFee(ulong chainId, string paymentToken, HexBigInteger gasLimit, bool isHighPriority, HexBigInteger gasLimitL1 = null)
        {
            if (gasLimitL1 == null)
            {
                gasLimitL1 = new HexBigInteger("0x0");
            }

            // this.gelatoClient.GetEstimatedFeeRequest()
        }

        private async Task<bool> IsNetworkSupported(uint networkId)
        {
            string[] supportedNetworks = await this.gelatoClient.GetSupportedNetworks();
            return supportedNetworks.Contains(networkId.ToString());
        }

        private async Task<bool> IsOracleActive(ulong chainId)
        {
            string[] oracles = await this.gelatoClient.GetGelatoOracles();
            return oracles.Contains(chainId.ToString());
        }
    }
}