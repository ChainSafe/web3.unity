#define RAMP_AVAILABLE

using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampExchangerUniversal : IRampExchanger, ILifecycleParticipant
    {
        private readonly IRampExchangerConfig config;
        private readonly ISigner signer;

        public event Action<OnRampPurchaseData> OnRampPurchaseCreated;
        public event Action<OffRampSaleData> OffRampSaleCreated;

        private IRampExchanger platformImplementation;

        public RampExchangerUniversal(IRampExchangerConfig config, ISigner signer)
        {
            this.signer = signer;
            this.config = config;
        }

        public ValueTask WillStartAsync()
        {
            platformImplementation = RampExchangerFactory.CreateRampExchanger(config, signer);
            platformImplementation.OnRampPurchaseCreated += InvokeOnRampPurchaseCreated;
            platformImplementation.OffRampSaleCreated += InvokeOffRampSaleCreated;
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask WillStopAsync()
        {
            platformImplementation.OnRampPurchaseCreated -= InvokeOnRampPurchaseCreated;
            platformImplementation.OffRampSaleCreated -= InvokeOffRampSaleCreated;
            platformImplementation = null;
            return new ValueTask(Task.CompletedTask);
        }

        public Task<OnRampPurchaseData> BuyCrypto(RampBuyWidgetSettings settings)
            => platformImplementation.BuyCrypto(settings);

        public Task<OffRampSaleData> SellCrypto(RampSellWidgetSettings settings)
            => platformImplementation.SellCrypto(settings);

        public Task<RampTransactionData> BuyOrSellCrypto(RampBuyOrSellWidgetSettings settings)
            => platformImplementation.BuyOrSellCrypto(settings);

        private void InvokeOnRampPurchaseCreated(OnRampPurchaseData obj)
        {
            Debug.Log("Invoking this mf");
            OnRampPurchaseCreated?.Invoke(obj);
        }

        private void InvokeOffRampSaleCreated(OffRampSaleData obj)
        {
            Debug.Log("Invoking this other mf");
            OffRampSaleCreated?.Invoke(obj);
        }
    }
}