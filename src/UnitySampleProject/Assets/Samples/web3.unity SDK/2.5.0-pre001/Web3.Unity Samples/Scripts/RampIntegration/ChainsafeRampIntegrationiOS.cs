using System.Runtime.InteropServices;
using ChainSafe.GamingSdk.ScriptableObjects;

#if UNITY_IOS
namespace ChainSafe.GamingSdk.RampIntegration
{
    public class ChainsafeRampIntegrationiOS : ChainsafeRampIntegrationBase
    {
        public ChainsafeRampIntegrationiOS(RampData rampData) : base(rampData)
        {
        }

        [DllImport("__Internal")]
        private static extern void OpenRampInChainsafe(string swapAsset, string offrampAsset, string swapAmount,
            string fiatCurrency, string fiatValue, string userAddress, string hostLogoUrl, string hostAppName,
            string userEmailAddress, string selectedCountryCode, string defaultAsset, string url,
            string webhookStatusUrl, string finalUrl, string containerNode, string hostApiKey, int useSendCryptoCallbackVersion);

        public override void OpenRamp()
        {
            OpenRampInChainsafe(_rampData.SwapAsset, _rampData.OfframpAsset, _rampData.SwapAmount,
                _rampData.FiatCurrency, _rampData.FiatValue, _rampData.UserAddress, _rampData.HostLogoUrl,
                _rampData.HostAppName, _rampData.UserEmailAddress, _rampData.SelectedCountryCode,
                _rampData.DefaultAsset, _rampData.Url, _rampData.WebhookStatusUrl, _rampData.FinalUrl,
                _rampData.ContainerNode, _rampData.HostApiKey, _rampData.UseSendCryptoCallbackVersion ? 1 : 0);      
        }
    }
}

#endif