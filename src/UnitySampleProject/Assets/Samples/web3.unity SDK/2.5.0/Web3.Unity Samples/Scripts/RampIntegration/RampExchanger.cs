#nullable enable
using System;
using ChainSafe.GamingSdk.ScriptableObjects;

namespace ChainSafe.Gaming.Exchangers
{
    public abstract class RampExchanger
    {
        
        protected readonly RampData _rampData;
        
        //It is very difficult to create a struct that matches 1:1 from swift/obj-c to C#,
        //That's why I'm passing so  many parameters, but our end-users (i.e. the developers) wouldn't have to worry about it
        //since they would be using the OnRampPurchaseData/OffRampSaleData classes for retrieving the infos.
        #region On Ramp
        protected delegate void OnRampPurchaseCallback(double appliedFee,                
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

        protected RampExchanger(RampData rampData)
        {
            _rampData = rampData;
        }

        public abstract void OpenRamp();
    }
}