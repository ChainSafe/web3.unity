using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Unity;
using UnityEngine;
using WalletConnectSharp.Network;
using WalletConnectSharp.Network.Interfaces;

public class WalletConnectWebSocketBuilder : MonoBehaviour, IConnectionBuilder
{
    public Task<IJsonRpcConnection> CreateConnection(string url)
    {
        TaskCompletionSource<IJsonRpcConnection> taskCompletionSource =
            new TaskCompletionSource<IJsonRpcConnection>();
            
        Dispatcher.Instance().Enqueue(() =>
        {
            DontDestroyOnLoad(gameObject);
            
            Debug.Log("Building websocket with URL " + url);
            var websocket = gameObject.AddComponent<WalletConnectWebSocket>();
            websocket.Url = url;
            
            taskCompletionSource.TrySetResult(websocket);
        });

            
        return taskCompletionSource.Task;
    }
}
