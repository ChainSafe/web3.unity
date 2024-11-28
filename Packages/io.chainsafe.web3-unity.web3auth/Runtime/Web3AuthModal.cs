using System;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.GUI;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Web3Auth modal used for displaying and selecting a Web3Auth provider.
/// </summary>
public class Web3AuthModal : MonoBehaviour
{
    [Serializable]
    public struct ProviderButtonPair
    {
        [field: SerializeField] public Provider Provider { get; private set; }

        [field: SerializeField] public Button Button { get; private set; }
    }

    [SerializeField] private ProviderButtonPair[] providers;

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

    public void Close()
    {
        _cancellationTokenSource?.Cancel();

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
