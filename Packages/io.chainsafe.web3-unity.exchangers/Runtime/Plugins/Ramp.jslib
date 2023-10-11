var RampLib = {
    $RampLib: {},

    testRamp__deps: ['toCSharpStringBuffer'],

    // todo namespace is shared between all .jslibs
    // make all names unique
    CS_RAMP_setOnRampPurchaseCallback: function (tableIndexOffset){
        RampLib.purchaseCallback = tableIndexOffset;
    },

    // Module.dynCall_v(RampLib.sellCallback);
    setOffRampSaleCallback: function (tableIndexOffset){
        RampLib.sellCallback = tableIndexOffset;
    },

    // todo assign ramp instance id
    injectRamp: function () {
        const scriptUrl = "https://cdn.jsdelivr.net/npm/@ramp-network/ramp-instant-sdk@2.5.0/dist/ramp-instant-sdk.umd.js";
        const script = document.createElement("script");
        script.src = scriptUrl;
        document.head.appendChild(script);
    },

    testRamp: function (hostApiKey) {
        var testBuffer1 = _toCSharpStringBuffer("test");
        var testBuffer2 = _toCSharpStringBuffer(null);
        const apiKeyString = UTF8ToString(hostApiKey);
        const sdkInstance = new rampInstantSdk.RampInstantSDK({
            hostAppName: 'ChainSafe',
            hostLogoUrl: 'https://imagedelivery.net/qdx9xDn6TxxInQGWsuRsVg/5646cd55-8803-4fec-bc22-95c20ad0dd00/public',
            hostApiKey: apiKeyString,
            url: 'https://app.demo.ramp.network',
        });
        sdkInstance
            .on('PURCHASE_CREATED', (event) => {
                // todo use event.payload.purchase
                var purchase = event.payload.purchase;
                
                // call callback
                Module.dynCall_vdiiiiiddiiiidiidiiii(
                    RampLib.purchaseCallback,
                    purchase.appliedFee,
                    _toCSharpStringBuffer(purchase.asset.address),
                    purchase.asset.decimals,
                    _toCSharpStringBuffer(purchase.asset.name),
                    _toCSharpStringBuffer(purchase.asset.symbol),
                    _toCSharpStringBuffer(purchase.asset.type),
                    purchase.assetExchangeRate,
                    purchase.baseRampFee,
                    _toCSharpStringBuffer(purchase.createdAt),
                    _toCSharpStringBuffer(purchase.cryptoAmount),
                    _toCSharpStringBuffer(purchase.endTime),
                    // skipped escrowAddress
                    // skipped escrowDetailsHash
                    _toCSharpStringBuffer(purchase.fiatCurrency),
                    purchase.fiatValue,
                    _toCSharpStringBuffer(purchase.finalTxHash),
                    _toCSharpStringBuffer(purchase.id),
                    purchase.networkFee,
                    _toCSharpStringBuffer(purchase.paymentMethodType),
                    _toCSharpStringBuffer(purchase.receiverAddress),
                    _toCSharpStringBuffer(purchase.status),
                    _toCSharpStringBuffer(purchase.updatedAt)
                    );
            })
            // .on('OFFRAMP_SALE_CREATED', (event) => {
            //     // todo use event.payload.sale
            //     _invokeOnSale();
            // })
            .show();
    },
    
    // todo try stringToNewUTF8()
    toCSharpStringBuffer: function(jsString) {
        if (jsString == null){
            return null;
        }        
        var bufferSize = lengthBytesUTF8(jsString) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(jsString, buffer, bufferSize);
        return buffer;
    }
};

autoAddDeps(RampLib, '$RampLib');
mergeInto(LibraryManager.library, RampLib);