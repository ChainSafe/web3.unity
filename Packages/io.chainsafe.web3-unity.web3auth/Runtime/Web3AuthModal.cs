using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthModal : MonoBehaviour
{
    [Serializable]
    public struct ProviderButtonPair
    {
        [field: SerializeField] public Provider Provider { get; private set; }
        
        [field: SerializeField] public Button Button { get; private set; }
    }
    
    [SerializeField] private ProviderButtonPair[] providers;
    [SerializeField] private Button closeButton;
    // Closes modal when background is clicked
    [SerializeField] private Button closeFromBackgroundButton;

    private TaskCompletionSource<Provider> _getProviderTask;
    
    private CancellationTokenSource _cancellationTokenSource;

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;

    private void OnEnable()
    {
        _getProviderTask = new TaskCompletionSource<Provider>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void Start()
    {
        closeButton.onClick.AddListener(Close);
        closeFromBackgroundButton.onClick.AddListener(Close);
        
        foreach (var pair in providers)
        {
            pair.Button.onClick.AddListener(() =>
            {
                if (_getProviderTask.Task.IsCompleted)
                {
                    throw new Web3Exception("Connection already resolved.");
                }
                
                _getProviderTask.SetResult(pair.Provider);
            });
        }
    }

    public Task<Provider> SelectProvider()
    {
        return _getProviderTask.Task;
    }
    
    private void Close()
    {
        _cancellationTokenSource.Cancel();

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (!_getProviderTask.Task.IsCompleted)
        {
            _getProviderTask.SetCanceled();
        }
        
        _cancellationTokenSource.Dispose();
    }
}
