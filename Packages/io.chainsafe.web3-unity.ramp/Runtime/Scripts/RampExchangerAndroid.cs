#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using UnityEngine;


namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampExchangerAndroid : IRampExchanger
    {
        private static int RequestIndexer;
        private static readonly Dictionary<int, TaskCompletionSource<OnRampPurchaseData>> purchaseTaskMap = new();
        private static readonly Dictionary<int, TaskCompletionSource<OffRampSaleData>> sellTaskMap = new();
        private static readonly Dictionary<int, TaskCompletionSource<RampTransactionData>> purchaseOrSellTaskMap = new();
        
        
        private readonly AndroidJavaClass _unityClass;
        private readonly AndroidJavaObject _rampSDK;
        private readonly AndroidJavaObject _unityActivity;
        private RampCallback _rampCallback;
        private readonly IRampExchangerConfig _rampData;
        private readonly ISigner _signer;
        
        public RampExchangerAndroid(IRampExchangerConfig rampData, ISigner signer)
        {
            _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _rampSDK = new AndroidJavaObject("network.ramp.sdk.facade.RampSDK");
            _unityActivity = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _rampData = rampData;
            _signer = signer;

        }

        public event Action<OnRampPurchaseData> OnRampPurchaseCreated;
        public event Action<OffRampSaleData> OffRampSaleCreated;
        public async Task<OnRampPurchaseData> BuyCrypto(RampBuyWidgetSettings settings)
        {
            var taskCompletionSource = new TaskCompletionSource<OnRampPurchaseData>();
            AndroidJavaClass flowClass = new AndroidJavaClass("network.ramp.sdk.facade.Flow");

            
            AndroidJavaObject onrampFlow = flowClass.GetStatic<AndroidJavaObject>("ONRAMP");
            AndroidJavaObject offrampFlow = flowClass.GetStatic<AndroidJavaObject>("OFFRAMP");
            
            AndroidJavaObject set = new AndroidJavaObject("java.util.HashSet");
            set.Call<bool>("add", onrampFlow);
            var requestIndex = RequestIndexer++;
            // Setting up the Config object based on the provided settings
            var config = new AndroidJavaObject("network.ramp.sdk.facade.Config",
                settings.OverrideHostAppName ?? _rampData.HostAppName,
                settings.OverrideHostLogoUrl ?? _rampData.HostLogoUrl,
                _rampData.Url,
                settings.SwapAsset,
                string.Empty, // offrampAsset is not needed for buying
                settings.SwapAmount.ToString(),
                settings.FiatCurrency,
                settings.FiatValue.ToString(),
                settings.OverrideUserAddress ?? _signer.PublicAddress,
                settings.UserEmailAddress ?? "",
                settings.SelectedCountryCode ?? "",
                settings.DefaultAsset ?? "",
                settings.OverrideWebhookStatusUrl ?? "",
                _rampData.HostApiKey,
                onrampFlow, // Default flow for buying
                set,
                _rampData.OfframpWebHookV3Url,
                null,
                null
            );

            _rampCallback = new RampCallback(requestIndex, PurchaseHappened, SaleHappened);
            var tcs = new TaskCompletionSource<RampTransactionData>();
            purchaseOrSellTaskMap.Add(requestIndex, tcs);
            // Calling the Ramp SDK to start the transaction
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _rampSDK.Call("startTransaction", _unityActivity, config, _rampCallback);
            }));
            
            var transactionData = await tcs.Task;
            
            return transactionData.PurchaseData!.Value;
        }

        private void SaleHappened(int arg1, OffRampSaleData arg2)
        {
            
        }

        private void PurchaseHappened(int requestId, OnRampPurchaseData purchaseData)
        {
            if (purchaseTaskMap.ContainsKey(requestId))
            {
                var tcs = purchaseTaskMap[requestId];
                purchaseTaskMap.Remove(requestId);
                tcs.SetResult(purchaseData);
                return;
            }

            if (purchaseOrSellTaskMap.ContainsKey(requestId))
            {
                var tcs = purchaseOrSellTaskMap[requestId];
                purchaseOrSellTaskMap.Remove(requestId);
                tcs.SetResult(new RampTransactionData { PurchaseData = purchaseData });
                return;
            }
            throw new Web3Exception($"No handler found for purchase request #{requestId}");

        }

        public Task<OffRampSaleData> SellCrypto(RampSellWidgetSettings settings)
        {
            throw new NotImplementedException();

        }

        public Task<RampTransactionData> BuyOrSellCrypto(RampBuyOrSellWidgetSettings settings)
        {
            throw new NotImplementedException();

        }
    }
    
    public class RampCallback : AndroidJavaProxy
    {
        private event Action<int, OnRampPurchaseData> _onRampPurchaseCreated;
        private event Action<int, OffRampSaleData> _offRampSaleCreated;
        private readonly int _requestIndex;

        public RampCallback(int requestIndex, Action<int, OnRampPurchaseData> onRampPurchaseCreated, Action<int, OffRampSaleData> offRampSaleData) :
            base("network.ramp.sdk.facade.RampCallback") // Replace with the actual interface path
        {
            _onRampPurchaseCreated = onRampPurchaseCreated;
            _offRampSaleCreated = offRampSaleData;
            _requestIndex = requestIndex;
        }

        public void onPurchaseFailed()
        {
            Debug.Log("MainActivity: onPurchaseFailed");
        }

        public void onPurchaseCreated(AndroidJavaObject purchase, string purchaseViewToken, string apiUrl)
        {
            Debug.Log("Here");
            _onRampPurchaseCreated?.Invoke(_requestIndex,ConvertPurchase(purchase));
        }
        
        private OnRampPurchaseData ConvertPurchase(AndroidJavaObject purchase)
        {
            // Assuming your Purchase Kotlin class maps directly to these fields
            return new OnRampPurchaseData(
                purchase.Get<double>("appliedFee"),
                new OnRampPurchaseData.AssetInfo(
                    purchase.Get<string>("asset.address"),
                    purchase.Get<int>("asset.decimals"),
                    purchase.Get<string>("asset.name"),
                    purchase.Get<string>("asset.symbol"),
                    purchase.Get<string>("asset.type")
                ),
                purchase.Get<double>("assetExchangeRate"),
                purchase.Get<double>("baseRampFee"),
                purchase.Get<string>("createdAt"),
                purchase.Get<string>("cryptoAmount"),
                purchase.Get<string>("endTime"),
                purchase.Get<string>("escrowAddress"), // Check for null
                purchase.Get<string>("escrowDetailsHash"), // Check for null
                purchase.Get<string>("fiatCurrency"),
                purchase.Get<double>("fiatValue"),
                purchase.Get<string>("finalTxHash"), // Check for null
                purchase.Get<string>("id"),
                purchase.Get<double>("networkFee"),
                purchase.Get<string>("paymentMethodType"),
                purchase.Get<string>("receiverAddress"),
                purchase.Get<string>("status"),
                purchase.Get<string>("updatedAt")
            );
        }


        public void onWidgetClose()
        {
            Debug.Log("MainActivity: onWidgetClose");
        }

        public void offrampSendCrypto(AndroidJavaObject assetInfo, string amount, string address)
        {
            // Note: You might need to retrieve fields from `assetInfo` using assetInfo.Get<type>("fieldName") as necessary
            Debug.Log($"MainActivity: offrampSendCrypto  assetInfo: {assetInfo} amount: {amount} address: {address}");
        }

        public void onOfframpSaleCreated(AndroidJavaObject sale, string saleViewToken, string apiUrl)
        {
            // Example fields extracted from the sale object
            string saleId = sale.Get<string>("id");
            string saleCreatedAt = sale.Get<string>("createdAt");
        
            // Assuming `crypto` is an inner object in `sale` which contains `amount` and `assetInfo` fields
            AndroidJavaObject crypto = sale.Get<AndroidJavaObject>("crypto");
            string cryptoAmount = crypto.Get<string>("amount");
            AndroidJavaObject cryptoAssetInfo = crypto.Get<AndroidJavaObject>("assetInfo");

            Debug.Log($"MainActivity: onOfframpSaleCreated {saleId} {saleCreatedAt} crypto: {cryptoAmount} {cryptoAssetInfo}");
            
            _offRampSaleCreated?.Invoke(_requestIndex, ConvertOfframpSale(sale));
        }
        
        private OffRampSaleData ConvertOfframpSale(AndroidJavaObject sale)
        {
            var crypto = sale.Get<AndroidJavaObject>("crypto");
            var fiat = sale.Get<AndroidJavaObject>("fiat");

            return new OffRampSaleData(
                sale.Get<string>("createdAt"),
                new OffRampSaleData.CryptoOffRamp(
                    crypto.Get<string>("amount"),
                    new OfframpAssetInfo(
                        crypto.Get<AndroidJavaObject>("assetInfo").Get<string>("address"),
                        crypto.Get<AndroidJavaObject>("assetInfo").Get<string>("chain"),
                        crypto.Get<AndroidJavaObject>("assetInfo").Get<int>("decimals"),
                        crypto.Get<AndroidJavaObject>("assetInfo").Get<string>("name"),
                        crypto.Get<AndroidJavaObject>("assetInfo").Get<string>("symbol"),
                        crypto.Get<AndroidJavaObject>("assetInfo").Get<string>("type")
                    )
                ),
                new OffRampSaleData.FiatOffRamp(
                    fiat.Get<double>("amount"),
                    fiat.Get<string>("currencySymbol")
                ),
                new Guid(sale.Get<string>("id"))
            );
        }

    }
}
#endif