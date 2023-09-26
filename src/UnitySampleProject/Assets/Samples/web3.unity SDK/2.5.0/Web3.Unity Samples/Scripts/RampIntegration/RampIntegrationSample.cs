using System;
using ChainSafe.GamingSdk.RampIntegration;
using ChainSafe.GamingSdk.ScriptableObjects;
using UnityEngine;

public class RampIntegrationSample : MonoBehaviour
{
    [SerializeField] private RampData rampData;
    private RampChainsafeIntegrationBase _ramp;

    private void Awake()
    {
#if UNITY_IOS
        _ramp = new RampChainsafeIntegrationiOS(rampData);
#endif
        RampChainsafeIntegrationBase.OnRampPurchaseEvent += OnRampPurchase;
        RampChainsafeIntegrationBase.OffRampSaleEvent += OffRampSaleEvent;
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
        RampChainsafeIntegrationBase.OnRampPurchaseEvent -= OnRampPurchase;
        RampChainsafeIntegrationBase.OffRampSaleEvent -= OffRampSaleEvent;
    }

    public void ButtonClicked()
    {
        _ramp.OpenRamp();
    }
}