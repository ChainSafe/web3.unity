#if UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    internal class RampExchangerWebGL : IRampExchanger
    {
        private delegate void OnOnRampPurchaseCallback(int requestIndex, double appliedFee, string? assetAddress,
            int assetDecimals, string assetName, string assetSymbol, string assetType, double assetExchangeRate,
            double baseRampFee, string createdAt, string cryptoAmount, string? endTime, string fiatCurrency,
            double fiatValue, string? finalTxHash, string id, double networkFee, string paymentMethodType,
            string receiverAddress, string status, string updatedAt);

        private delegate void OffRampSaleCallback(int requestId, string createdAt, string cryptoAmount, string cryptoAssetAddress,
            string cryptoAssetChain, int cryptoAssetDecimals, string cryptoAssetName, string cryptoAssetSymbol,
            string cryptoAssetType, double fiatAmount, string fiatCurrencySymbol);

        private static bool RampJsInjected;
        private static int RequestIndexer;
        private static readonly Dictionary<int, TaskCompletionSource<OnRampPurchaseData>> purchaseTaskMap = new();
        private static readonly Dictionary<int, TaskCompletionSource<OffRampSaleData>> sellTaskMap = new();
        private static readonly Dictionary<int, TaskCompletionSource<RampTransactionData>> purchaseOrSellTaskMap = new();

        public event Action<OnRampPurchaseData> OnRampPurchaseCreated;
        public event Action<OffRampSaleData> OffRampSaleCreated;

        private readonly IRampExchangerConfig config;
        private readonly ISigner signer;

        public RampExchangerWebGL(IRampExchangerConfig config, ISigner signer)
        {
            this.signer = signer;
            this.config = config;

            if (!RampJsInjected)
            {
                cs_ramp_injectRamp();
                cs_ramp_setOnRampPurchaseCallback(OnOnRampPurchase);
                cs_ramp_setOffRampSaleCallback(OnOffRampSale);
                RampJsInjected = true;
            }
        }

        public async Task<OnRampPurchaseData> BuyCrypto(RampBuyWidgetSettings settings)
        {
            var userAddress = settings.OverrideUserAddress ?? signer.PublicAddress;
            var hostLogoUrl = settings.OverrideHostLogoUrl ?? config.HostLogoUrl;
            var hostAppName = settings.OverrideHostAppName ?? config.HostAppName;
            var webhookStatusUrl = config.WebhookStatusUrl ?? settings.OverrideWebhookStatusUrl;
            var requestId = RequestIndexer++;

            cs_ramp_showWidget(requestId, settings.SwapAsset, string.Empty, settings.SwapAmount,
                settings.FiatCurrency, settings.FiatValue, userAddress, hostLogoUrl, hostAppName,
                settings.UserEmailAddress, settings.SelectedCountryCode, settings.DefaultAsset, config.Url,
                webhookStatusUrl, config.HostApiKey, true, false, string.Empty,
                false);

            var tcs = new TaskCompletionSource<OnRampPurchaseData>();
            purchaseTaskMap.Add(requestId, tcs);

            var purchaseData = await tcs.Task;
            OnRampPurchaseCreated?.Invoke(purchaseData);
            return purchaseData;
        }

        public async Task<OffRampSaleData> SellCrypto(RampSellWidgetSettings settings)
        {
            var userAddress = settings.OverrideUserAddress ?? signer.PublicAddress;
            var hostLogoUrl = settings.OverrideHostLogoUrl ?? config.HostLogoUrl;
            var hostAppName = settings.OverrideHostAppName ?? config.HostAppName;
            var offrampWebhookV3Url = config.OfframpWebHookV3Url ?? settings.OverrideOfframpWebHookV3Url;
            var requestId = RequestIndexer++;

            cs_ramp_showWidget(requestId, string.Empty, settings.OfframpAsset, settings.SwapAmount,
                settings.FiatCurrency, settings.FiatValue, userAddress, hostLogoUrl, hostAppName,
                settings.UserEmailAddress, settings.SelectedCountryCode, settings.DefaultAsset, config.Url,
                string.Empty, config.HostApiKey, false, true, offrampWebhookV3Url,
                settings.UseSendCryptoCallback);

            var tcs = new TaskCompletionSource<OffRampSaleData>();
            sellTaskMap.Add(requestId, tcs);

            var saleData = await tcs.Task;
            OffRampSaleCreated?.Invoke(saleData);
            return saleData;
        }

        public async Task<RampTransactionData> BuyOrSellCrypto(RampBuyOrSellWidgetSettings settings)
        {
            var userAddress = settings.OverrideUserAddress ?? signer.PublicAddress;
            var hostLogoUrl = settings.OverrideHostLogoUrl ?? config.HostLogoUrl;
            var hostAppName = settings.OverrideHostAppName ?? config.HostAppName;
            var webhookStatusUrl = config.WebhookStatusUrl ?? settings.OverrideWebhookStatusUrl;
            var offrampWebhookV3Url = config.OfframpWebHookV3Url ?? settings.OverrideOfframpWebHookV3Url;
            var requestId = RequestIndexer++;

            cs_ramp_showWidget(requestId, settings.SwapAsset, settings.OfframpAsset, settings.SwapAmount,
                settings.FiatCurrency, settings.FiatValue, userAddress, hostLogoUrl, hostAppName,
                settings.UserEmailAddress, settings.SelectedCountryCode, settings.DefaultAsset, config.Url,
                webhookStatusUrl, config.HostApiKey, true, true, offrampWebhookV3Url,
                settings.UseSendCryptoCallback);

            var tcs = new TaskCompletionSource<RampTransactionData>();
            purchaseOrSellTaskMap.Add(requestId, tcs);

            var transactionData = await tcs.Task;
            if (transactionData.IsPurchase)
            {
                OnRampPurchaseCreated?.Invoke(transactionData.PurchaseData!.Value);
            }
            else
            {
                OffRampSaleCreated?.Invoke(transactionData.SaleData!.Value);
            }

            return transactionData;
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOnRampPurchase(
            int requestId,
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
            var purchaseData = new OnRampPurchaseData
            {
                Asset = new OnRampPurchaseData.AssetInfo
                {
                    Address = assetAddress,
                    Decimals = assetDecimals,
                    Name = assetName,
                    Symbol = assetSymbol,
                    Type = assetType
                },
                AppliedFee = appliedFee,
                BaseRampFee = baseRampFee,
                CreatedAt = createdAt,
                CryptoAmount = cryptoAmount,
                Status = status,
                EndTime = endTime,
                FiatCurrency = fiatCurrency,
                FiatValue = fiatValue,
                NetworkFee = networkFee,
                ReceiverAddress = receiverAddress,
                UpdatedAt = updatedAt,
                AssetExchangeRate = assetExchangeRate,
                FinalTxHash = finalTxHash,
                PaymentMethodType = paymentMethodType
            };

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

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOffRampSale(int requestId, string createdAt, string cryptoAmount, string cryptoAssetAddress,
            string cryptoAssetChain, int cryptoAssetDecimals, string cryptoAssetName, string cryptoAssetSymbol,
            string cryptoAssetType, double fiatAmount, string fiatCurrencySymbol)
        {
            var saleData = new OffRampSaleData
            {
                CreatedAt = createdAt, // todo to DateTime
                Crypto = new OffRampSaleData.CryptoOffRamp
                {
                    Amount = cryptoAmount,
                    AssetInfo = new OfframpAssetInfo
                    {
                        Address = cryptoAssetAddress,
                        Chain = cryptoAssetChain,
                        Decimals = cryptoAssetDecimals,
                        Name = cryptoAssetName,
                        Symbol = cryptoAssetSymbol,
                        Type = cryptoAssetType
                    }
                },
                Fiat = new OffRampSaleData.FiatOffRamp
                {
                    Amount = fiatAmount,
                    CurrencySymbol = fiatCurrencySymbol
                }
            };

            if (sellTaskMap.ContainsKey(requestId))
            {
                var tcs = sellTaskMap[requestId];
                sellTaskMap.Remove(requestId);
                tcs.SetResult(saleData);
                return;
            }

            if (purchaseOrSellTaskMap.ContainsKey(requestId))
            {
                var tcs = purchaseOrSellTaskMap[requestId];
                purchaseOrSellTaskMap.Remove(requestId);
                tcs.SetResult(new RampTransactionData { SaleData = saleData });
                return;
            }

            throw new Web3Exception($"No handler found for sell request #{requestId}");
        }

        #region JS interop

        // adding cs_ramp_ prefix because all methods in all *.jslib files share one namespace
        [DllImport("__Internal")]
        private static extern void cs_ramp_injectRamp();

        [DllImport("__Internal")]
        private static extern void cs_ramp_setOnRampPurchaseCallback(OnOnRampPurchaseCallback callback);

        [DllImport("__Internal")]
        private static extern void cs_ramp_setOffRampSaleCallback(OffRampSaleCallback callback);

        [DllImport("__Internal")]
        private static extern void cs_ramp_showWidget(int requestId, string swapAsset, string offrampAsset, int swapAmount,
            string fiatCurrency, int fiatValue, string userAddress, string hostLogoUrl, string hostAppName,
            string userEmailAddress, string selectedCountryCode, string defaultAsset, string url,
            string webhookStatusUrl, string hostApiKey, bool enableBuy, bool enableSell, string offrampWebHookV3Url,
            bool useSendCryptoCallback);

        #endregion
    }
}
#endif