var RampLib = {
    $RampLib: {},

    cs_ramp_setOnRampPurchaseCallback: function (tableIndexOffset){
        RampLib.purchaseCallback = tableIndexOffset;
    },

    cs_ramp_setOffRampSaleCallback: function (tableIndexOffset){
        RampLib.sellCallback = tableIndexOffset;
    },

    // todo assign ramp instance id
    cs_ramp_injectRamp: function () {
        const scriptUrl = "https://cdn.jsdelivr.net/npm/@ramp-network/ramp-instant-sdk@4.0.4/dist/ramp-instant-sdk.umd.js";
        const script = document.createElement("script");
        script.src = scriptUrl;
        document.head.appendChild(script);
    },

    cs_ramp_showWidget: function (requestId, swapAsset, offrampAsset, swapAmount,
                                  fiatCurrency, fiatValue, userAddress, hostLogoUrl, hostAppName,
                                  userEmailAddress, selectedCountryCode, defaultAsset, url,
                                  webhookStatusUrl, hostApiKey, enableBuy, enableSell, offrampWebHookV3Url,
                                  useSendCryptoCallback) {
        const enabledFlows = enableBuy && enableSell ? ['ONRAMP', 'OFFRAMP']
            : enableBuy ? ['ONRAMP']
                : ['OFFRAMP'];
        const swapAssets = UTF8ToString(swapAsset).split(",");
        const offrampAssets = UTF8ToString(offrampAsset).split(",");
       
        const sdkInstance = new rampInstantSdk.RampInstantSDK({
            hostApiKey: UTF8ToString(hostApiKey),
            swapAsset: swapAssets,
            offrampAsset: offrampAssets,
            swapAmount: swapAmount,
            fiatCurrency: UTF8ToString(fiatCurrency),
            fiatValue: fiatValue,
            userAddress: UTF8ToString(userAddress),
            hostLogoUrl: UTF8ToString(hostLogoUrl),
            hostAppName: UTF8ToString(hostAppName),
            userEmailAddress: UTF8ToString(userEmailAddress),
            selectedCountryCode: UTF8ToString(selectedCountryCode),
            defaultAsset: UTF8ToString(defaultAsset),
            url: UTF8ToString(url),
            webhookStatusUrl: UTF8ToString(webhookStatusUrl),
            enabledFlows: enabledFlows,
            offrampWebhookV3Url: UTF8ToString(offrampWebHookV3Url),
            useSendCryptoCallback: useSendCryptoCallback
        });
        sdkInstance
            .on('PURCHASE_CREATED', (event) => {
                    const transactionData = event.payload.purchase;
                    const purchase = event.payload.purchase;
                    
                    Module.dynCall_vidiiiiiddiiiidiidiiii(RampLib.purchaseCallback,
                        requestId,
                        purchase.appliedFee,
                        stringToNewUTF8(purchase.asset.address == null ? "" : purchase.asset.address),
                        purchase.asset.decimals,
                        stringToNewUTF8(purchase.asset.name == null ? "" : purchase.asset.name),
                        stringToNewUTF8(purchase.asset.symbol == null ? "" : purchase.asset.symbol),
                        stringToNewUTF8(purchase.asset.type == null ? "" : purchase.asset.type),
                        purchase.assetExchangeRate,
                        purchase.baseRampFee,
                        stringToNewUTF8(purchase.createdAt == null ? "" : purchase.createdAt),
                        stringToNewUTF8(purchase.cryptoAmount == null ? "" : purchase.cryptoAmount),
                        stringToNewUTF8(purchase.endTime == null ? "" : purchase.endTime),
                        // skipped escrowAddress
                        // skipped escrowDetailsHash
                        stringToNewUTF8(purchase.fiatCurrency == null ? "" : purchase.fiatCurrency),
                        purchase.fiatValue,
                        stringToNewUTF8(purchase.finalTxHash == null ? "" : purchase.finalTxHash),
                        stringToNewUTF8(purchase.id == null ? "" : purchase.id),
                        purchase.networkFee,
                        stringToNewUTF8(purchase.paymentMethodType == null ? "" : purchase.paymentMethodType),
                        stringToNewUTF8(purchase.receiverAddress == null ? "" : purchase.receiverAddress),
                        stringToNewUTF8(purchase.status),
                        stringToNewUTF8(purchase.updatedAt == null ? "" : purchase.updatedAt),
                    );
            }).on('OFFRAMP_SALE_CREATED', (event) =>{
            console.log("sale"); // todo remove

            const sale = event.payload.sale;
            Module.dynCall_viiiiiiiiidi(RampLib.sellCallback,
                requestId,
                stringToNewUTF8(sale.createdAt == null ? "" : sale.createdAt),
                stringToNewUTF8(sale.crypto.amount == null ? "" : sale.crypto.amount),
                stringToNewUTF8(sale.crypto.assetInfo.address == null ? "" : sale.crypto.assetInfo.address),
                stringToNewUTF8(sale.crypto.assetInfo.chain == null ? "" : sale.crypto.assetInfo.chain),
                stringToNewUTF8(sale.crypto.assetInfo.decimals == null ? "" : sale.crypto.assetInfo.decimals),
                stringToNewUTF8(sale.crypto.assetInfo.name == null ? "" : sale.crypto.assetInfo.name),
                stringToNewUTF8(sale.crypto.assetInfo.symbol == null ? "" : sale.crypto.assetInfo.symbol),
                stringToNewUTF8(sale.crypto.assetInfo.type == null ? "" : sale.crypto.assetInfo.type),
                sale.fiat.amount,
                stringToNewUTF8(sale.fiat.currencySymbol == null ? "" : sale.fiat.currencySymbol),
            );      
        })
            .show();
    },
    
   
};

autoAddDeps(RampLib, '$RampLib');
mergeInto(LibraryManager.library, RampLib);