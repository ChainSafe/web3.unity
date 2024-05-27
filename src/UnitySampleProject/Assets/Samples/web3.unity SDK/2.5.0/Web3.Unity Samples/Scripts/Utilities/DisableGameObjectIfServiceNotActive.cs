using System;
using System.Collections.Generic;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Gelato.Types;
using UnityEngine;

public enum ServiceType
{
    #if RAMP_AVAILABLE
    Ramp,
    #endif
    Gelato,
    Multicall
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
        if (_typesDictionary.TryGetValue(serviceType, out var value))
        {
            gameObject.SetActive(Web3Accessor.Web3.ServiceProvider.GetService(value) != null);
        }
    }


}