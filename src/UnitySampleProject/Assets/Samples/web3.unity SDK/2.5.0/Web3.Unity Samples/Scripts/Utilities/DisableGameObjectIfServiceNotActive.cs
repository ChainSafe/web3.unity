using System;
using System.Collections.Generic;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Gelato.Types;
using UnityEngine;
#if MARKETPLACE_AVAILABLE
using ChainSafe.Gaming.Marketplace;
#endif

public enum ServiceType
{
    Ramp = 0,
    Gelato = 1,
    Multicall = 2,
    Marketplace = 3
}

public class DisableGameObjectIfServiceNotActive : MonoBehaviour
{
    [SerializeField] private ServiceType serviceType;
    private readonly Dictionary<ServiceType, Type> _typesDictionary = new()
    {
        #if RAMP_AVAILABLE
        {ServiceType.Ramp, typeof(ChainSafe.Gaming.Exchangers.Ramp.IRampExchanger)},
        #endif
        #if MARKETPLACE_AVAILABLE
        {ServiceType.Marketplace, typeof(MarketplaceClient)},
        #endif
        {ServiceType.Gelato, typeof(IGelato)},
        {ServiceType.Multicall, typeof(IMultiCall)}
    };

    private void Awake()
    {
        ShouldGameObjectBeDisabled();
    }

    private void ShouldGameObjectBeDisabled() => gameObject.SetActive(
        _typesDictionary.TryGetValue(serviceType, out var value)
        && Web3Accessor.Web3.ServiceProvider.GetService(value) != null);
}