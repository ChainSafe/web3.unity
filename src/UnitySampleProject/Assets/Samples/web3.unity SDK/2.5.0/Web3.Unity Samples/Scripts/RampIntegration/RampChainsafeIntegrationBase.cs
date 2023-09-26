#nullable enable
using System;
using ChainSafe.GamingSdk.ScriptableObjects;

namespace ChainSafe.GamingSdk.RampIntegration
{
    public abstract class RampChainsafeIntegrationBase
    {
        #region On Ramp
        public delegate void OnRampPurchaseCallback(double appliedFee,                
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
        
        protected readonly RampData _rampData;
        
        public static  Action<OnRampPurchaseData> OnRampPurchaseEvent = null!;
        #endregion
        #region Off Ramp
        protected delegate void OffRampSaleCallback(
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
        
        public static Action<OffRampSaleData> OffRampSaleEvent = null;
        #endregion

        protected RampChainsafeIntegrationBase(RampData rampData)
        {
            _rampData = rampData;
        }

        public abstract void OpenRamp();
    }
}