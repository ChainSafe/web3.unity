using System;
using ChainSafe.GamingSdk.ScriptableObjects;

namespace ChainSafe.GamingSdk.RampIntegration
{
    
    public abstract class ChainsafeRampIntegrationBase
    {
        #region On Ramp
        public delegate void OnRampPurchaseCallback(OnRampPurchaseData data);
        
        protected readonly RampData _rampData;
        
        public static  OnRampPurchaseCallback OnRampPurchaseEvent;
        #endregion
        #region Off Ramp
        public delegate void OffRampSaleCallback(
            string createdAt,         // createdAt
            string cryptoAmount,      // cryptoAmount
            string cryptoAssetAddress, // cryptoAssetAddress (nullable in Swift, so it's optional)
            string cryptoAssetChain,   // cryptoAssetChain
            int cryptoAssetDecimals,   // cryptoAssetDecimals
            string cryptoAssetName,    // cryptoAssetName
            string cryptoAssetSymbol,  // cryptoAssetSymbol
            string cryptoAssetType,    // cryptoAssetType
            double fiatAmount,         // fiatAmount
            string fiatCurrencySymbol, // fiatCurrencySymbol
            Guid id                    // id (UUID in Swift)
        );
        
        public static  Action<OffRampSaleData> OffRampSaleEvent;
        #endregion

        protected ChainsafeRampIntegrationBase(RampData rampData)
        {
            _rampData = rampData;
        }

        public abstract void OpenRamp();
    }
}