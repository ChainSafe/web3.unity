#nullable enable
using System;
using System.Runtime.InteropServices;
using AOT;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    // todo refactor taking the new approach into account
    // internal class RampExchangeriOS : RampExchanger
    // {
    //     private delegate void OnRampPurchaseCallback(
    //         double appliedFee,                
    //         string? assetAddress,             
    //         int assetDecimals,                
    //         string assetName,                 
    //         string assetSymbol,               
    //         string assetType,                 
    //         double assetExchangeRate,         
    //         double baseRampFee,               
    //         string createdAt,                 
    //         string cryptoAmount,              
    //         string? endTime,                  
    //         string? escrowAddress,            
    //         string? escrowDetailsHash,        
    //         string fiatCurrency,              
    //         double fiatValue,                 
    //         string? finalTxHash,              
    //         string id,                        
    //         double networkFee,                
    //         string paymentMethodType,         
    //         string receiverAddress,           
    //         string status,                    
    //         string updatedAt                  
    //     );
    //     
    //     private delegate void OffRampSaleCallback(
    //         string createdAt,         
    //         string cryptoAmount,      
    //         string cryptoAssetAddress, 
    //         string cryptoAssetChain,   
    //         int cryptoAssetDecimals,   
    //         string cryptoAssetName,    
    //         string cryptoAssetSymbol,  
    //         string cryptoAssetType,    
    //         double fiatAmount,         
    //         string fiatCurrencySymbol, 
    //         Guid id                    
    //     );
    //     
    //     public RampExchangeriOS(RampData rampData) : base(rampData)
    //     {
    //         setOnRampPurchase(SetOnRampPurchase);
    //         setOffRampSale(SetOffRampSale);
    //     }
    //
    //     [DllImport("__Internal")]
    //     private static extern void setOnRampPurchase(OnRampPurchaseCallback callback);
    //
    //     [DllImport("__Internal")]
    //     private static extern void setOffRampSale(OffRampSaleCallback callback);
    //
    //     [MonoPInvokeCallback(typeof(OnRampPurchaseCallback))]
    //     public static void SetOnRampPurchase(double appliedFee, 
    //         string? assetAddress, 
    //         int assetDecimals, 
    //         string assetName, 
    //         string assetSymbol, 
    //         string assetType, 
    //         double assetExchangeRate, 
    //         double baseRampFee, 
    //         string createdAt, 
    //         string cryptoAmount, 
    //         string? endTime, 
    //         string? escrowAddress, 
    //         string? escrowDetailsHash, 
    //         string fiatCurrency, 
    //         double fiatValue, 
    //         string? finalTxHash, 
    //         string id, 
    //         double networkFee, 
    //         string paymentMethodType, 
    //         string receiverAddress, 
    //         string status, 
    //         string updatedAt)
    //     {
    //         var onRampPurchased = new OnRampPurchaseData(appliedFee,
    //             new OnRampPurchaseData.AssetInfo(assetAddress, assetDecimals, assetName, assetSymbol, assetType),
    //             assetExchangeRate, baseRampFee, createdAt, cryptoAmount, endTime, escrowAddress, escrowDetailsHash,
    //             fiatCurrency, fiatValue, finalTxHash, id, networkFee, paymentMethodType, receiverAddress, status,
    //             updatedAt);
    //         OnRampPurchaseEvent?.Invoke(onRampPurchased);
    //     }
    //
    //     [MonoPInvokeCallback(typeof(OffRampSaleCallback))]
    //     public static void SetOffRampSale(string createdAt, 
    //         string cryptoAmount, 
    //         string cryptoAssetAddress, 
    //         string cryptoAssetChain, 
    //         int cryptoAssetDecimals, 
    //         string cryptoAssetName, 
    //         string cryptoAssetSymbol, 
    //         string cryptoAssetType, 
    //         double fiatAmount, 
    //         string fiatCurrencySymbol, 
    //         Guid id) 
    //     {
    //         OffRampSaleEvent?.Invoke(new OffRampSaleData
    //         {
    //             CreatedAt = createdAt,
    //             Crypto = new OffRampSaleData.CryptoOffRamp(cryptoAmount,
    //                 new OfframpAssetInfo(cryptoAssetAddress, cryptoAssetChain, cryptoAssetDecimals, cryptoAssetName,
    //                     cryptoAssetSymbol, cryptoAssetType)),
    //             Fiat = new OffRampSaleData.FiatOffRamp(fiatAmount, fiatCurrencySymbol)
    //         });
    //        
    //     }
    //
    //     
    //     [DllImport("__Internal")]
    //     private static extern void OpenRampInChainsafe(string swapAsset, string offrampAsset, int swapAmount,
    //         string fiatCurrency, int fiatValue, string userAddress, string hostLogoUrl, string hostAppName,
    //         string userEmailAddress, string selectedCountryCode, string defaultAsset, string url,
    //         string webhookStatusUrl, string finalUrl, string containerNode, string hostApiKey,
    //         int useSendCryptoCallbackVersion);
    //
    //     public override void OpenRamp()
    //     {
    //         // todo use config values
    //         // OpenRampInChainsafe(_rampData.SwapAsset, _rampData.OfframpAsset, _rampData.SwapAmount,
    //         //     _rampData.FiatCurrency, _rampData.FiatValue, _rampData.OverrideUserAddress, _rampData.OverrideHostLogoUrl,
    //         //     _rampData.OverrideHostAppName, _rampData.UserEmailAddress, _rampData.SelectedCountryCode,
    //         //     _rampData.DefaultAsset, _rampData.OverrideUrl, _rampData.OverrideWebhookStatusUrl, _rampData.FinalUrl,
    //         //     string.Empty, _rampData.HostApiKey, _rampData.UseSendCryptoCallbackVersion ? 1 : 0);
    //     }
    // }
}

