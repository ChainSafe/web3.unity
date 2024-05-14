using System;
using System.Collections.Generic;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Gelato.Types;
using UnityEngine;

public enum ServiceType
{
    Ramp = 0,
    Gelato = 1,
    Multicall = 2
}

public class DisableGameObjectIfServiceNotActive : MonoBehaviour
{
    [SerializeField] private ServiceType serviceType;
    private readonly Dictionary<ServiceType, Type> _typesDictionary = new()
    {
#if RAMP_AVAILABLE
        {ServiceType.Ramp, typeof(ChainSafe.Gaming.Exchangers.Ramp.IRampExchanger)},
#endif
        {ServiceType.Gelato, typeof(IGelato)},
        {ServiceType.Multicall, typeof(IMultiCall)}
    };

    private void Awake()
    {
        if (serviceType != ServiceType.Ramp)
            gameObject.SetActive(Web3Accessor.Web3.ServiceProvider.GetService(_typesDictionary[serviceType]) != null);
        else
            CheckRamp();
    }

    private void CheckRamp()
    {
#if RAMP_AVAILABLE
            gameObject.SetActive(Web3Accessor.Web3.ServiceProvider.GetService(_typesDictionary[serviceType]) != null);
#else 
        gameObject.SetActive(false);
#endif
    }
}