using System;
using System.Runtime.InteropServices;
using AOT;
using ChainSafe.GamingSdk.ScriptableObjects;
using UnityEngine;

#if UNITY_IOS
namespace ChainSafe.GamingSdk.RampIntegration
{
    public class ChainsafeRampIntegrationiOS : ChainsafeRampIntegrationBase
    {
        public ChainsafeRampIntegrationiOS(RampData rampData) : base(rampData)
        {
            setOnRampPurchase(SetOnRampPurchase);
            setOffRampSale(SetOffRampSale);
        }

        [DllImport("__Internal")]
        private static extern void setOnRampPurchase(OnRampPurchaseCallback callback);

        [DllImport("__Internal")]
        private static extern void setOffRampSale(OffRampSaleCallback callback);

        [MonoPInvokeCallback(typeof(OnRampPurchaseCallback))]
        public static void SetOnRampPurchase(OnRampPurchaseData data)
        {
            OnRampPurchaseEvent?.Invoke(data);
            Debug.LogError("RAMP PURCHASEDDD");
        }

        [MonoPInvokeCallback(typeof(OffRampSaleCallback))]
        public static void SetOffRampSale(string createdAt, // createdAt
            string cryptoAmount, // cryptoAmount
            string cryptoAssetAddress, // cryptoAssetAddress (nullable in Swift, so it's optional)
            string cryptoAssetChain, // cryptoAssetChain
            int cryptoAssetDecimals, // cryptoAssetDecimals
            string cryptoAssetName, // cryptoAssetName
            string cryptoAssetSymbol, // cryptoAssetSymbol
            string cryptoAssetType, // cryptoAssetType
            double fiatAmount, // fiatAmount
            string fiatCurrencySymbol, // fiatCurrencySymbol
            Guid id) // id (UUID in Swift) data)
        {
            OffRampSaleEvent?.Invoke(new OffRampSaleData
            {
                CreatedAt = createdAt,
                Crypto = new OffRampSaleData.CryptoOffRamp(cryptoAmount,
                    new OfframpAssetInfo(cryptoAssetAddress, cryptoAssetChain, cryptoAssetDecimals, cryptoAssetName,
                        cryptoAssetSymbol, cryptoAssetType)),
                Fiat = new OffRampSaleData.FiatOffRamp(fiatAmount, fiatCurrencySymbol)
            });
            Debug.LogError("RAMP SOLLDDDD");
            Debug.Log($"{nameof(cryptoAmount)}: {cryptoAmount}\n" +
                      $"{nameof(cryptoAssetAddress)}: {cryptoAssetAddress ?? "null"}\n" +
                      $"{nameof(cryptoAssetChain)}: {cryptoAssetChain}\n" +
                      $"{nameof(cryptoAssetDecimals)}: {cryptoAssetDecimals}\n" +
                      $"{nameof(cryptoAssetName)}: {cryptoAssetName}\n" +
                      $"{nameof(cryptoAssetSymbol)}: {cryptoAssetSymbol}\n" +
                      $"{nameof(cryptoAssetType)}: {cryptoAssetType}\n" +
                      $"{nameof(fiatAmount)}: {fiatAmount}\n" +
                      $"{nameof(fiatCurrencySymbol)}: {fiatCurrencySymbol}\n" +
                      $"{nameof(id)}: {id}");
        }

        [DllImport("__Internal")]
        private static extern void OpenRampInChainsafe(string swapAsset, string offrampAsset, string swapAmount,
            string fiatCurrency, string fiatValue, string userAddress, string hostLogoUrl, string hostAppName,
            string userEmailAddress, string selectedCountryCode, string defaultAsset, string url,
            string webhookStatusUrl, string finalUrl, string containerNode, string hostApiKey,
            int useSendCryptoCallbackVersion);

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