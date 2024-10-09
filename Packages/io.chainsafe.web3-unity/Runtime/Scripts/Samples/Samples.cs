using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

public class Samples : ServiceAdapter, ILightWeightServiceAdapter, IWeb3InitializedHandler
{
    [SerializeField] private Transform container;
    [SerializeField] private SampleContainer sampleContainerPrefab;
    [SerializeField] private Button buttonPrefab;

    private readonly List<SampleContainer> _sampleContainers = new List<SampleContainer>();

    private bool _initialized;

    private void Initialize()
    {
        var samples = GetComponentsInChildren<ISample>();

        foreach (var sample in samples)
        {
            var methods = sample.GetType().GetMethods().Where(m =>
                m.ReturnType == typeof(Task<string>)
                && m.GetParameters().Length == 0
                && m.IsPublic).ToArray();

            if (!methods.Any())
            {
                continue;
            }

            var sampleContainer = Instantiate(sampleContainerPrefab, container);

            sampleContainer.Initialize(sample);

            _sampleContainers.Add(sampleContainer);

            foreach (var method in methods)
            {
                var button = Instantiate(buttonPrefab, sampleContainer.Container);

                var buttonText = button.GetComponentInChildren<TMP_Text>();

                buttonText.text = method.Name;

                button.onClick.AddListener(delegate
                {
                    TryExecute(method, sample);
                });
            }
        }

        _initialized = true;
    }

    private async void TryExecute(MethodInfo method, ISample instance)
    {
        try
        {
            LoadingOverlay.ShowLoadingOverlay();

            string message = await Execute(method, instance);

            Debug.Log($"{message} \n {instance.Title} - {instance.GetType().Name}.{method.Name}");
        }
        // Todo: display error via error overlay
        catch (Exception e)
        {
            if (e is ServiceNotBoundWeb3Exception<ISigner>
                || e is ServiceNotBoundWeb3Exception<ITransactionExecutor>)
            {
                Web3Unity.ConnectModal.Open();

                throw new AggregateException(new Web3Exception("Connection not found. Please connect your wallet first."), e);
            }

            throw;
        }
        finally
        {
            LoadingOverlay.HideLoadingOverlay();
        }
    }

    private Task<string> Execute(MethodInfo method, ISample instance)
    {
        return (Task<string>)method.Invoke(instance, null);
    }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler>(this);
        });
    }

    public Task OnWeb3Initialized(CWeb3 web3)
    {
        if (!_initialized)
        {
            Initialize();
        }

        foreach (var sampleContainer in _sampleContainers)
        {
            sampleContainer.Web3Initialized(web3);
        }

        return Task.CompletedTask;
    }
}
