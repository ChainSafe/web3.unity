using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;
using WalletConnectSharp.Common;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Events;
using WalletConnectSharp.Events.Model;
using WalletConnectSharp.Network;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Network.Websocket;

public class WalletConnectWebSocket : MonoBehaviour, IJsonRpcConnection, IModule
{
    WebSocket _socket;

    private EventDelegator _delegator;

    [SerializeField] private string url;
    private bool _registering;
    private Guid _context;
    private TaskCompletionSource<bool> beginConnect;
    private bool begunConnect;

    /// <summary>
    /// The Open timeout
    /// </summary>
    public TimeSpan OpenTimeout = TimeSpan.FromSeconds(60);

    /// <summary>
    /// The Url to connect to
    /// </summary>
    public string Url
    {
        get { return url; }
        set => url = value;
    }

    public bool IsPaused { get; private set; }

    /// <summary>
    /// The name of this websocket connection module
    /// </summary>
    public string Name
    {
        get { return "websocket-connection"; }
    }

    /// <summary>
    /// The context string of this Websocket module
    /// </summary>
    public string Context
    {
        get { return _context.ToString(); }
    }

    /// <summary>
    /// The EventDelegator this Websocket connection module is using
    /// </summary>
    public EventDelegator Events
    {
        get { return _delegator; }
    }

    /// <summary>
    /// Whether this websocket connection is connected
    /// </summary>
    public bool Connected
    {
        get
        {
            bool connected = _socket != null && _socket.State == WebSocketState.Open;
            WCLogger.Log("Websocket Connected State: " + connected);
            return connected;
        }
    }

    /// <summary>
    /// Whether this websocket connection is currently connecting
    /// </summary>
    public bool Connecting
    {
        get { return _registering; }
    }

    public async void Dispose()
    {
        if (Connected)
        {
            WCLogger.Log("Socket is being disposed, cleanup");
            await Close();
        }
    }

    public async Task Open()
    {
        await Register(this.url);
    }

    public Task Open<T>(T options)
    {
        if (typeof(string).IsAssignableFrom(typeof(T)))
        {
            return Register(options as string);
        }

        return Open();
    }

    public async Task Close()
    {
        if (_socket == null) throw new IOException("Connection already closed");

        await _socket.Close();

        WCLogger.Log("Closing websocket due to Close() being called");
        OnClose(WebSocketCloseCode.Normal);
    }

    public async Task SendRequest<T>(IJsonRpcRequest<T> requestPayload, object context)
    {
        if (_socket == null) _socket = await Register(this.url);

        try
        {
            Debug.Log($"[WCWebSocket-{_context}] Sending request {JsonConvert.SerializeObject(requestPayload)}");

            await _socket.SendText(JsonConvert.SerializeObject(requestPayload));
        }
        catch (Exception e)
        {
            Debug.LogError(e);

            OnError<T>(requestPayload, e);
        }
    }

    public async Task SendResult<T>(IJsonRpcResult<T> responsePayload, object context)
    {
        if (_socket == null) _socket = await Register(this.url);

        try
        {
            await _socket.SendText(JsonConvert.SerializeObject(responsePayload));
        }
        catch (Exception e)
        {
            OnError<T>(responsePayload, e);
        }
    }

    public async Task SendError(IJsonRpcError errorPayload, object context)
    {
        if (_socket == null) _socket = await Register(this.url);

        try
        {
            await _socket.SendText(JsonConvert.SerializeObject(errorPayload));
        }
        catch (Exception e)
        {
            OnError<object>(errorPayload, e);
        }
    }

    private void Awake()
    {
        _context = Guid.NewGuid();
        _delegator = new EventDelegator(this);
    }

    private async Task<WebSocket> Register(string _url)
    {
        if (!Validation.IsWsUrl(_url))
        {
            throw new ArgumentException("Provided URL is not compatible with WebSocket connection: " + _url);
        }

        if (_registering)
        {
            TaskCompletionSource<WebSocket> registeringTask =
                new TaskCompletionSource<WebSocket>(TaskCreationOptions.None);

            Events.ListenForOnce(WebsocketConnectionEvents.RegisterError,
                delegate(object sender, GenericEvent<Exception> @event)
                {
                    registeringTask.SetException(@event.EventData);
                });

            Events.ListenForOnce(WebsocketConnectionEvents.Open,
                delegate(object sender, GenericEvent<WebSocket> @event)
                {
                    registeringTask.SetResult(@event.EventData);
                });

            await registeringTask.Task;

            return registeringTask.Task.Result;
        }

        this.url = _url;
        this._registering = true;

        try
        {
            _socket = new WebSocket(this.url);
            //_socket = new WebsocketClient(new Uri(_url));

            await StartWebsocket(_socket).WithTimeout(OpenTimeout, "Unavailable WS RPC url at " + this.url);
            OnOpen(_socket);
            return _socket;
        }
        catch (Exception e)
        {
            Events.Trigger(WebsocketConnectionEvents.RegisterError, e);

            WCLogger.Log($"Calling close due to exception {e}");
            OnClose(WebSocketCloseCode.ServerError);

            throw;
        }
    }

    private Task StartWebsocket(WebSocket socket)
    {
        this._socket = socket;
        begunConnect = false;
        beginConnect = new TaskCompletionSource<bool>();

        return beginConnect.Task;
    }

    private void OnOpen(WebSocket socket)
    {
        socket.OnMessage += OnPayload;
        socket.OnClose += OnDisconnect;

        this._registering = false;
        Events.Trigger(WebsocketConnectionEvents.Open, _socket);
    }

    private void OnDisconnect(WebSocketCloseCode obj)
    {
        if (obj != WebSocketCloseCode.Normal) Events.Trigger(WebsocketConnectionEvents.Error, obj);

        WCLogger.Log("Socket closing due to Disconnect event from socket");
        OnClose(obj);
    }

    private void OnClose(WebSocketCloseCode obj)
    {
        if (this._socket == null) return;

        //_socket.Dispose();
        this._socket = null;
        this._registering = false;
        Events.Trigger(WebsocketConnectionEvents.Close, obj);
    }

    private void OnPayload(byte[] data)
    {
        string json = System.Text.Encoding.UTF8.GetString(data);

        Debug.Log($"[WCWebSocket-{_context}] Got payload {json}");

        if (string.IsNullOrWhiteSpace(json)) return;

        Debug.Log($"[WCWebsocket-{_context}] Triggering payload event with JSON {json}");

        Events.Trigger(WebsocketConnectionEvents.Payload, json);
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (_socket != null) _socket.DispatchMessageQueue();
#endif
    }

    private async void LateUpdate()
    {
        if (begunConnect || beginConnect == null || beginConnect.Task.IsCompleted) return;
        if (_socket == null) return;

        begunConnect = true;
        this._socket.OnOpen += SocketOnOnOpen;
        try
        {
            await this._socket.Connect();
        }
        catch (Exception e)
        {
            this.beginConnect.TrySetException(e);
        }
    }

    private void SocketOnOnOpen()
    {
        beginConnect.TrySetResult(true);
    }

    private string addressNotFoundError = "getaddrinfo ENOTFOUND";
    private string connectionRefusedError = "connect ECONNREFUSED";

    private void OnError<T>(IJsonRpcPayload ogPayload, Exception e)
    {
        var exception = e.Message.Contains(addressNotFoundError) || e.Message.Contains(connectionRefusedError)
            ? new IOException("Unavailable WS RPC url at " + this.url)
            : e;

        var message = exception.Message;
        var payload = new JsonRpcResponse<T>(ogPayload.Id,
            new Error() { Code = exception.HResult, Data = null, Message = message }, default(T));

        //Trigger the payload event, converting the new JsonRpcResponse object to JSON string
        Events.Trigger(WebsocketConnectionEvents.Payload, JsonConvert.SerializeObject(payload));

        Debug.LogError(e);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        IsPaused = pauseStatus;
    }
}