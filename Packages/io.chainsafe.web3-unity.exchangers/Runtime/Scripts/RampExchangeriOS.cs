#if UNITY_IOS
#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using ChainSafe.Gaming.Evm.Signers;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    internal class RampExchangeriOS : IRampExchanger
    {
        private readonly IRampExchangerConfig config;
        private readonly ISigner signer;
        
        // todo use static {id,tcs} dictionary to ensure correct static callback->task interaction
        // (see RampExchangerWebGL for example)
        private static TaskCompletionSource<OnRampPurchaseData>? lastBuyTaskCompletionSource;
        private static TaskCompletionSource<OffRampSaleData>? lastSellTaskCompletionSource;

        public RampExchangeriOS(IRampExchangerConfig config, ISigner signer)
        {
            this.signer = signer;
            this.config = config;
            
            setOnRampPurchase(SetOnRampPurchase);
            setOffRampSale(SetOffRampSale);
        }

        public async Task<OnRampPurchaseData> BuyCrypto(RampBuyWidgetSettings settings)
        {
            var userAddress = settings.OverrideUserAddress ?? await signer.GetAddress();
            var hostLogoUrl = settings.OverrideHostLogoUrl ?? config.HostLogoUrl;
            var hostAppName = settings.OverrideHostAppName ?? config.HostAppName;
            var webhookStatusUrl = config.WebhookStatusUrl ?? settings.OverrideWebhookStatusUrl;
            
            // todo set buy flag
            OpenRampInChainsafe(settings.SwapAsset, string.Empty, settings.SwapAmount,
                settings.FiatCurrency, settings.FiatValue, userAddress, hostLogoUrl,
                hostAppName, settings.UserEmailAddress, settings.SelectedCountryCode,
                settings.DefaultAsset, config.Url, webhookStatusUrl, string.Empty, 
                string.Empty, config.HostApiKey, 0);

            lastBuyTaskCompletionSource = new TaskCompletionSource<OnRampPurchaseData>();

            return await lastBuyTaskCompletionSource.Task;
        }

        public async Task<OffRampSaleData> SellCrypto(RampSellWidgetSettings settings)
        {
            var userAddress = settings.OverrideUserAddress ?? await signer.GetAddress();
            var hostLogoUrl = settings.OverrideHostLogoUrl ?? config.HostLogoUrl;
            var hostAppName = settings.OverrideHostAppName ?? config.HostAppName;
            var offrampWebhookV3Url = config.OfframpWebHookV3Url ?? settings.OverrideOfframpWebHookV3Url;
            
            // todo set sell flag
            // todo provide offrampWebhookV3Url argument
            OpenRampInChainsafe(string.Empty, settings.OfframpAsset, settings.SwapAmount,
                settings.FiatCurrency, settings.FiatValue, userAddress, hostLogoUrl,
                hostAppName, settings.UserEmailAddress, settings.SelectedCountryCode,
                settings.DefaultAsset, config.Url, string.Empty, string.Empty, 
                string.Empty, config.HostApiKey, settings.UseSendCryptoCallback ? 1 : 0);

            lastSellTaskCompletionSource = new TaskCompletionSource<OffRampSaleData>();

            return await lastSellTaskCompletionSource.Task;
        }

        // todo add another callback from iOS for BuyOrSell, implement this method
        public Task<RampTransactionData> BuyOrSellCrypto(RampBuyOrSellWidgetSettings settings)
        {
            throw new NotImplementedException("BuyOrSellCrypto is currently not available when running on iOS.");
        }

        #region iOS interop

        [MonoPInvokeCallback(typeof(OnRampPurchaseCallback))]
        public static void SetOnRampPurchase(double appliedFee, 
            string? assetAddress, 
            int assetDecimals, 
            string assetName, 
            string assetSymbol, 
            string assetType, 
            double assetExchangeRate, 
            double baseRampFee, 
            string createdAt, 
            string cryptoAmount, 
            string? endTime, 
            string? escrowAddress, 
            string? escrowDetailsHash, 
            string fiatCurrency, 
            double fiatValue, 
            string? finalTxHash, 
            string id, 
            double networkFee, 
            string paymentMethodType, 
            string receiverAddress, 
            string status, 
            string updatedAt)
        {
            var onRampPurchased = new OnRampPurchaseData(appliedFee,
                new OnRampPurchaseData.AssetInfo(assetAddress, assetDecimals, assetName, assetSymbol, assetType),
                assetExchangeRate, baseRampFee, createdAt, cryptoAmount, endTime, escrowAddress, escrowDetailsHash,
                fiatCurrency, fiatValue, finalTxHash, id, networkFee, paymentMethodType, receiverAddress, status,
                updatedAt);
            
            lastBuyTaskCompletionSource?.SetResult(onRampPurchased);
            lastBuyTaskCompletionSource = null;
        }
    
        [MonoPInvokeCallback(typeof(OffRampSaleCallback))]
        public static void SetOffRampSale(string createdAt, 
            string cryptoAmount, 
            string cryptoAssetAddress, 
            string cryptoAssetChain, 
            int cryptoAssetDecimals, 
            string cryptoAssetName, 
            string cryptoAssetSymbol, 
            string cryptoAssetType, 
            double fiatAmount, 
            string fiatCurrencySymbol, 
            Guid id)
        {
            var offRampSaleData = new OffRampSaleData
            {
                CreatedAt = createdAt,
                Crypto = new OffRampSaleData.CryptoOffRamp(cryptoAmount,
                    new OfframpAssetInfo(cryptoAssetAddress, cryptoAssetChain, cryptoAssetDecimals, cryptoAssetName,
                        cryptoAssetSymbol, cryptoAssetType)),
                Fiat = new OffRampSaleData.FiatOffRamp(fiatAmount, fiatCurrencySymbol)
            };
            
            lastSellTaskCompletionSource?.SetResult(offRampSaleData);
            lastBuyTaskCompletionSource = null;
        }
    
        [DllImport("__Internal")]
        private static extern void setOnRampPurchase(OnRampPurchaseCallback callback);
    
        [DllImport("__Internal")]
        private static extern void setOffRampSale(OffRampSaleCallback callback);
    
        // todo add buy/sell flags
        // todo remove finalUrl parameter?
        // todo add offrampWebhookV3Url parameter
        [DllImport("__Internal")]
        private static extern void OpenRampInChainsafe(string swapAsset, string offrampAsset, int swapAmount,
            string fiatCurrency, int fiatValue, string userAddress, string hostLogoUrl, string hostAppName,
            string userEmailAddress, string selectedCountryCode, string defaultAsset, string url,
            string webhookStatusUrl, string finalUrl, string containerNode, string hostApiKey,
            int useSendCryptoCallbackVersion);

        #endregion
        
        private delegate void OnRampPurchaseCallback(
            double appliedFee,                
            string? assetAddress,             
            int assetDecimals,                
            string assetName,                 
            string assetSymbol,               
            string assetType,                 
            double assetExchangeRate,         
            double baseRampFee,               
            string createdAt,                 
            string cryptoAmount,              
            string? endTime,                  
            string? escrowAddress,            
            string? escrowDetailsHash,        
            string fiatCurrency,              
            double fiatValue,                 
            string? finalTxHash,              
            string id,                        
            double networkFee,                
            string paymentMethodType,         
            string receiverAddress,           
            string status,                    
            string updatedAt                  
        );
        
        private delegate void OffRampSaleCallback(
            string createdAt,         
            string cryptoAmount,      
            string cryptoAssetAddress, 
            string cryptoAssetChain,   
            int cryptoAssetDecimals,   
            string cryptoAssetName,    
            string cryptoAssetSymbol,  
            string cryptoAssetType,    
            double fiatAmount,         
            string fiatCurrencySymbol, 
            Guid id                    
        );
    }
}
#endif