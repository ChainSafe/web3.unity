using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Gelato.Types;
using Microsoft.Extensions.DependencyInjection;
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

public class DisableGameObjectIfServiceNotActive : ServiceAdapter, IWeb3InitializedHandler, ILightWeightServiceAdapter
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
        gameObject.SetActive(false);
    }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler>(this);
        });
    }

    public Task OnWeb3Initialized(Web3 web3)
    {
        gameObject.SetActive(
            _typesDictionary.TryGetValue(serviceType, out var value)
            && web3.ServiceProvider.GetService(value) != null);

        return Task.CompletedTask;
    }
}