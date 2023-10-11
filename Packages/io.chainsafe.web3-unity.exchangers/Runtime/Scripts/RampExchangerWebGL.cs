using System;
using System.Runtime.InteropServices;
using AOT;
using JetBrains.Annotations;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    // todo add #if UNITY_WEBGL
    
    public class RampExchangerWebGL : RampExchanger
    {
        private delegate void OnOnRampPurchaseCallback(double appliedFee, string? assetAddress, int assetDecimals,
            string assetName, string assetSymbol, string assetType, double assetExchangeRate, double baseRampFee,
            string createdAt, string cryptoAmount, string? endTime, string fiatCurrency, double fiatValue,
            string? finalTxHash, string id, double networkFee, string paymentMethodType, string receiverAddress,
            string status, string updatedAt);
        
        private readonly RampData rampData;

        // todo move rampData parameter to OpenRamp method
        public RampExchangerWebGL([NotNull] RampData rampData) : base(rampData)
        {
            this.rampData = rampData;
            
            injectRamp();
            setOnRampPurchaseCallback(OnOnRampPurchase);
            setOffRampSaleCallback(OnOffRampSale);
        }

        // todo use instanceIds instead for static callbacks
        public override void OpenRamp()
        {
            testRamp(rampData.HostApiKey);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOnRampPurchase(
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
            // string? escrowAddress, not found in JS SDK
            // string? escrowDetailsHash, 
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
            Debug.Log(
                "double appliedFee is " + appliedFee + "\n" +
                "string? assetAddress is " + assetAddress + "\n" +
                "int assetDecimals is " + assetDecimals + "\n" +
                "string assetName is " + assetName + "\n" +
                "string assetSymbol is " + assetSymbol + "\n" +
                "string assetType is " + assetType + "\n" +
                "double assetExchangeRate is " + assetExchangeRate + "\n" +
                "double baseRampFee is " + baseRampFee + "\n" +
                "string createdAt is " + createdAt + "\n" +
                "string cryptoAmount is " + cryptoAmount + "\n" +
                "string? endTime is " + endTime + "\n" +
                "string fiatCurrency is " + fiatCurrency + "\n" +
                "double fiatValue is " + fiatValue + "\n" +
                "string? finalTxHash is " + finalTxHash + "\n" +
                "string id is " + id + "\n" +
                "double networkFee is " + networkFee + "\n" +
                "string paymentMethodType is " + paymentMethodType + "\n" +
                "string receiverAddress is " + receiverAddress + "\n" +
                "string status is " + status + "\n" +
                "string updatedAt is " + updatedAt
            );
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOffRampSale()
        {
            Debug.Log("On off-ramp sale called.");
        }

        #region JS interop methods

        [DllImport("__Internal")]
        private static extern void injectRamp();
        
        [DllImport("__Internal")]
        private static extern void setOnRampPurchaseCallback(OnOnRampPurchaseCallback callback);
        
        [DllImport("__Internal")]
        private static extern void setOffRampSaleCallback(Action callback);
        
        [DllImport("__Internal")]
        private static extern void testRamp(string hostApiKey);

        #endregion
    }
}