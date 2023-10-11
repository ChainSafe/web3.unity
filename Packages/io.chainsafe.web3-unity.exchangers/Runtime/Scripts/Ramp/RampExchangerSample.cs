using UnityEngine;

namespace ChainSafe.Gaming.Exchangers
{
    public class RampExchangerSample : MonoBehaviour
    {
        [SerializeField] private RampData rampData;
        private RampExchanger _ramp;

        private void Awake()
        {
            _ramp = RampExchangerFactory.CreateRampExchanger(rampData);
            RampExchanger.OnRampPurchaseEvent += OnRampPurchase;
            RampExchanger.OffRampSaleEvent += OffRampSaleEvent;
        }

        private void OnDestroy()
        {
            RampExchanger.OnRampPurchaseEvent -= OnRampPurchase;
            RampExchanger.OffRampSaleEvent -= OffRampSaleEvent;
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

        public void ButtonClicked()
        {
            _ramp.OpenRamp();
        }
    }
}