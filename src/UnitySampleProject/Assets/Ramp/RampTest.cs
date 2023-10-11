using System;
using System.Runtime.InteropServices;
using AOT;
using ChainSafe.Gaming.Web3;
using ChainSafe.GamingSdk.RampIntegration;
using ChainSafe.GamingSdk.ScriptableObjects;
using JetBrains.Annotations;
using UnityEngine;

namespace Ramp
{
    public class RampTest : MonoBehaviour
    {
        private static uint InstancesCount;
        
        public string HostApiKey;
        
        public bool WidgetShowing { get; private set; }
        
        private void Awake()
        {
            AssertNoInstancesActive();
            
            InstancesCount++;
            
            injectRamp();
            setOnRampPurchaseCallback(OnOnRampPurchase);
            setOffRampSaleCallback(OnOffRampSale);

            void AssertNoInstancesActive()
            {
                if (InstancesCount > 0)
                {
                    // the reason is static javascript interop
                    throw new Web3Exception($"{nameof(RampTest)} doesn't support more than one active instance.");
                }
            }
        }

        private void OnDestroy()
        {
            InstancesCount--;
        }

        // todo temp for tests
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowWidget();
            }
        }

        public void ShowWidget()
        {
            if (WidgetShowing)
            {
                throw new Web3Exception("Widget is already showing.");
            }

            WidgetShowing = true;
            testRamp(HostApiKey);
        }

        private delegate void OnOnRampPurchaseCallback(double appliedFee, string? assetAddress, int assetDecimals,
            string assetName, string assetSymbol, string assetType, double assetExchangeRate, double baseRampFee,
            string createdAt, string cryptoAmount, string? endTime, string fiatCurrency, double fiatValue,
            string? finalTxHash, string id, double networkFee, string paymentMethodType, string receiverAddress,
            string status, string updatedAt);

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