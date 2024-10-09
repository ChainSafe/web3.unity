
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Unity;
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

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Custom web socket implementation for Wallet Connect, based on NativeWebSocket https://github.com/endel/NativeWebSocket.
    /// </summary>
    public class WebSocketConnection : IJsonRpcConnection, IModule
    {
        public string Name => "websocket-connection";

        public string Context { get; }

        public bool Connected { get; set; }

        public bool Connecting { get; private set; }

        public string Url { get; private set; }

        public bool IsPaused { get; private set; }

        public event EventHandler<string> PayloadReceived;
        public event EventHandler Closed;
        public event EventHandler<Exception> ErrorReceived;
        public event EventHandler<object> Opened;
        public event EventHandler<Exception> RegisterErrored;

        private WebSocket _socket;
        private bool _disposed;

        public WebSocketConnection(string url)
        {
            if (!Validation.IsWsUrl(url))
                throw new ArgumentException(
                    $"[WebSocketConnection] Provided URL is not compatible with WebSocket connection: {url}");

            Context = Guid.NewGuid().ToString();
            Url = url;
        }

        Task IJsonRpcConnection.Open()
        {
            Register();
            return Task.CompletedTask;
        }

        Task IJsonRpcConnection.Open<T>(T options)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(WebSocketConnection));

            if (typeof(string).IsAssignableFrom(typeof(T)))
            {
                var newUrl = options as string;

                if (!Validation.IsWsUrl(newUrl))
                    throw new ArgumentException(
                        $"[WebSocketConnection] Provided URL is not compatible with WebSocket connection: {newUrl}");

                Url = newUrl;
            }

            Register();
            return Task.CompletedTask;
        }

        async Task IJsonRpcConnection.Close()
        {
            if (!Connected || _socket == null)
                throw new IOException("Connection already closed");

            if (_disposed)
                throw new ObjectDisposedException(nameof(WebSocketConnection));

            WCLogger.Log("Closing websocket due to Close() being called");

            await _socket.Close();
            OnClose(WebSocketCloseCode.Normal);
        }

        async Task IJsonRpcConnection.SendRequest<T>(IJsonRpcRequest<T> requestPayload, object context)
        {
            if (_socket == null)
                throw new IOException("Connection is not open");

            if (_disposed)
                throw new ObjectDisposedException(nameof(WebSocketConnection));

            try
            {
                var json = JsonConvert.SerializeObject(requestPayload);
                WCLogger.Log($"[WebSocketConnection-{Context}] Sending request {json}");

                await _socket.SendText(json);

                PayloadReceived?.Invoke(this, json);
            }
            catch (Exception e)
            {
                OnError<T>(requestPayload, e);
            }
        }

        async Task IJsonRpcConnection.SendResult<T>(IJsonRpcResult<T> responsePayload, object context)
        {
            if (_socket == null)
                throw new IOException("Connection is not open");

            if (_disposed)
                throw new ObjectDisposedException(nameof(WebSocketConnection));

            try
            {
                await _socket.SendText(JsonConvert.SerializeObject(responsePayload));
            }
            catch (Exception e)
            {
                OnError<T>(responsePayload, e);
            }
        }

        async Task IJsonRpcConnection.SendError(IJsonRpcError errorPayload, object context)
        {
            if (_socket == null)
                throw new IOException("Connection is not open");

            try
            {
                await _socket.SendText(JsonConvert.SerializeObject(errorPayload));
            }
            catch (Exception e)
            {
                OnError<object>(errorPayload, e);
            }
        }

        private void Register()
        {
            if (!Validation.IsWsUrl(Url))
                throw new ArgumentException($"Provided URL is not compatible with WebSocket connection: {Url}");

            if (Connecting || Connected)
                throw new InvalidOperationException("WebSocket is already connecting or connected");

            Connecting = true;

            _socket = new WebSocket(Url);
            _socket.OnOpen += OnOpen;
            _socket.OnMessage += OnMessage;
            _socket.OnClose += OnDisconnect;
            _socket.OnError += OnError;

#if !UNITY_WEBGL || UNITY_EDITOR
            Dispatcher.Instance().OnTick += OnTick;
#endif
            Dispatcher.Instance().OnApplicationPaused += OnApplicationPause;

            try
            {
                Connect();
            }
            catch (Exception e)
            {
                OnError<IJsonRpcPayload>(null, e);
                throw;
            }
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private void OnTick()
        {
            _socket.DispatchMessageQueue();
        }
#endif

        private void OnApplicationPause(bool status)
        {
            IsPaused = status;
        }

        private void OnOpen()
        {
            Connected = true;
            Connecting = false;

            Opened?.Invoke(this, _socket);
        }

        private void OnMessage(byte[] data)
        {
            var json = System.Text.Encoding.UTF8.GetString(data);

            WCLogger.Log($"[WebSocketConnection-{Context}] Got payload: \n{json}");

            if (string.IsNullOrWhiteSpace(json))
                return;

            PayloadReceived?.Invoke(this, json);
        }

        private void OnDisconnect(WebSocketCloseCode obj)
        {
            if (obj != WebSocketCloseCode.Normal)
                ErrorReceived?.Invoke(this, new Exception($"Websocket closed with code {obj}"));

            OnClose(obj);
        }

        private void OnError(string message)
        {
            ErrorReceived?.Invoke(this, new Exception(message));
            WCLogger.LogError(Connecting
                ? $"[{Name}-{Context}] Error happened during connection. Make sure Project ID is valid. Error message: {message}"
                : $"[{Name}-{Context}] Error: {message}");
        }

        private void OnError<T>(IJsonRpcPayload ogPayload, Exception e)
        {
            if (ogPayload != null)
            {
                var payload = new JsonRpcResponse<T>(ogPayload.Id, new Error()
                {
                    Code = e.HResult,
                    Data = null,
                    Message = e.Message
                }, default);

                var json = JsonConvert.SerializeObject(payload);

                //Trigger the payload event, converting the new JsonRpcResponse object to JSON string
                PayloadReceived?.Invoke(this, json);
            }

            WCLogger.LogError(e);
        }

        private void OnClose(WebSocketCloseCode obj)
        {
            if (_socket == null)
                return;

            if (Dispatcher.Exists())
            {
#if !UNITY_WEBGL || UNITY_EDITOR
                Dispatcher.Instance().OnTick -= OnTick;
#endif
                Dispatcher.Instance().OnApplicationPaused -= OnApplicationPause;
            }

            _socket = null;
            Connected = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        // This is a workaround for NativeWebSocket.Connect() blocking the main thread
        private async void Connect()
        {
            await _socket.Connect();
        }

        public async void Dispose()
        {
            await _socket.Close();
        }
    }
}
