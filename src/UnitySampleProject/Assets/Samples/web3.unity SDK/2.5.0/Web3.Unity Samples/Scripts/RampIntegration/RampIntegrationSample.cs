using System;
using ChainSafe.Gaming.Exchangers;
using ChainSafe.GamingSdk.ScriptableObjects;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers
{


    public class RampIntegrationSample : MonoBehaviour
    {
        [SerializeField] private RampData rampData;
        private RampExchanger _ramp;

        private void Awake()
        {
#if UNITY_IOS
        _ramp = new RampExchangeriOS(rampData);
#endif
            RampExchanger.OnRampPurchaseEvent += OnRampPurchase;
            RampExchanger.OffRampSaleEvent += OffRampSaleEvent;
        }

        private void OffRampSaleEvent(OffRampSaleData obj)
        {
            //Write your stuff you need to happen when offRampSale happens
        }

        private void OnRampPurchase(OnRampPurchaseData obj)
        {
            //write your stuff you need to happen when OnRampPurchase happens
            //NOTE: Ramp purchases are not instant. It takes time for the transaction to be confirmed on the blockchain.
        }

        private void OnDestroy()
        {
            RampExchanger.OnRampPurchaseEvent -= OnRampPurchase;
            RampExchanger.OffRampSaleEvent -= OffRampSaleEvent;
        }

        public void ButtonClicked()
        {
            _ramp.OpenRamp();
        }
    }
}