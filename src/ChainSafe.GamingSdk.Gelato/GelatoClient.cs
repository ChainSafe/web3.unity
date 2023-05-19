using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;

namespace ChainSafe.GamingSdk.Gelato
{
    public class GelatoClient
    {
        private IHttpClient httpClient;

        public GelatoClient(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void CallWithSyncFee()
        {
        }

        public void CallWithSyncFeeERC2771()
        {
        }

        public void SponsoredCall()
        {
        }

        public void SponsoredCallERC2771()
        {
        }

        public void IsNetworkSupported(uint networkId)
        {
        }

        public void GetSupportedNetworks()
        {
        }

        public void IsOracleActive()
        {
        }

        public void GetGelatoOracles()
        {
        }

        public void GetPaymentTokens()
        {
        }

        public void GetEstimatedFee()
        {
        }

        public void GetTaskStatus()
        {
        }
    }
}